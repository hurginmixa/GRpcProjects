using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CMM_Parallel_Runner.API.Contracts;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    internal class Session : IDisposable
    {
        private readonly IServerStreamWriter<GrpcExportResult> _responseStream;
        private readonly ICmmProcessor _cmmProcessor;
        private readonly CancellationToken _serverCancellationToken;
        private readonly CancellationToken _sessionCancellationToken;
        private readonly CancellationTokenSource _sessionCancellationTokenSource;

        private readonly AsyncQueue<GrpcCmmExportRequest> _queue;

        public Session(ICmmProcessor cmmProcessor, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
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

            return new GrpcCmmResult {Result = eGrpcExportResult.Ok, Message = "Ok", Index = grpcRequest.Index};
        }

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
                catch (OperationCanceledException)
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