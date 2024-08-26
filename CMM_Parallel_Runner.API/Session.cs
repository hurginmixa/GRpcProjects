using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMM_Parallel_Runner.API.Contracts;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    public class Session : IDisposable
    {
        private readonly string _sessionId;
        private readonly IServerStreamWriter<GrpcExportResult> _responseStream;
        private readonly ICmmProcessor _cmmProcessor;
        private readonly CancellationToken _serverCancellationToken;
        private readonly CancellationToken _sessionCancellationToken;
        private readonly CancellationTokenSource _sessionCancellationTokenSource;

        public delegate void SessionDelegate(string sessionId, Session session);
        public delegate void SessionAddDelegate(string sessionId, Session session, bool add);
        public event SessionAddDelegate OnSessionChanged;

        private readonly AsyncQueue<GrpcCmmExportRequest> _queue;

        public Session(string sessionId, ICmmProcessor cmmProcessor, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            _sessionId = sessionId;
            _responseStream = responseStream;
            _cmmProcessor = cmmProcessor;
            
            _serverCancellationToken = context.CancellationToken;
            _sessionCancellationTokenSource = new CancellationTokenSource();
            _sessionCancellationToken = _sessionCancellationTokenSource.Token;

            _queue = new AsyncQueue<GrpcCmmExportRequest>();
        }

        public GrpcCmmResult AddCmmExportRequest(GrpcCmmExportRequest grpcRequest)
        {
            _queue.Enqueue(grpcRequest);

            OnSessionChanged?.Invoke(_sessionId, this, true);

            return new GrpcCmmResult {Result = eGrpcExportResult.Ok, Message = "Ok", Index = grpcRequest.Index};
        }

        public string SessionId => _sessionId;

        public int RequestCount => _queue.Count;

        public async Task Process()
        {
            CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_serverCancellationToken, _sessionCancellationToken);

            CancellationToken commonCancellationToken = linkedTokenSource.Token;

            while (true)
            {
                GrpcCmmExportRequest grpcExportRequest;

                try
                {
                    grpcExportRequest = await _queue.DequeueAsync(commonCancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    break;
                }

                CmmExportRequest cmmExportRequest = new CmmExportRequest(grpcExportRequest);

                CmmExportResult cmmExportResult = _cmmProcessor.DoCmmExport(cmmExportRequest);

                GrpcExportResult grpcExportResult = new GrpcExportResult
                {
                    Result = cmmExportResult.Success ? eGrpcExportResult.Ok : eGrpcExportResult.Fail,
                    Parameters = {cmmExportResult.CreateFileInfos.Select(f => f.AsGrpc())},
                    Index = grpcExportRequest.Index
                };

                await _responseStream.WriteAsync(grpcExportResult);

                if (commonCancellationToken.IsCancellationRequested)
                {
                    break;
                }

                OnSessionChanged?.Invoke(_sessionId, this, false);
            }
        }

        public void Dispose()
        {
            Stop();

            _queue.Dispose();
        }

        public void Stop()
        {
            _sessionCancellationTokenSource.Cancel();
        }
    }
}