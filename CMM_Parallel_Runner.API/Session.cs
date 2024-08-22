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

        private readonly AsyncQueue<GrpcCmmExportRequest> _queue;

        public Session(IServerStreamWriter<GrpcExportResult> responseStream, ICmmProcessor cmmProcessor)
        {
            _responseStream = responseStream;
            _cmmProcessor = cmmProcessor;
            _queue = new AsyncQueue<GrpcCmmExportRequest>();
        }

        public async Task Process(CancellationToken cancellationToken)
        {
            while (true)
            {
                GrpcCmmExportRequest grpcExportRequest = await _queue.DequeueAsync(cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                CmmExportRequest exportRequest = new CmmExportRequest(grpcExportRequest);

                CmmExportResult cmmExportResult = _cmmProcessor.DoCmmExport(exportRequest);

                GrpcExportResult grpcExportResult = new GrpcExportResult
                {
                    Result = cmmExportResult.Success ? eGrpcExportResult.Ok : eGrpcExportResult.Fail,
                    Parameters = {cmmExportResult.CreateFileInfos.Select(f => f.AsGrpc())},
                    Index = grpcExportRequest.Index
                };

                await _responseStream.WriteAsync(grpcExportResult);
            }
        }

        public GrpcCmmExportResponse AddCmmExportRequest(GrpcCmmExportRequest grpcRequest)
        {
            _queue.Enqueue(grpcRequest);

            return new GrpcCmmExportResponse {Result = eGrpcExportResult.Ok, Message = "Ok", Index = grpcRequest.Index};
        }

        public void Dispose()
        {
            _queue.Dispose();
        }
    }
}