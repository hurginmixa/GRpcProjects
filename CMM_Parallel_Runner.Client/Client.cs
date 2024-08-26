using System;
using System.Threading;
using System.Threading.Tasks;
using CMM_Parallel_Runner.API;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.Client
{
    public class Client
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceClient _client;
        private readonly Task _readingStreamTask;

        public Client()
        {
            Channel channel =  Helpers.MakeChannel(Helpers.DefaultHost, Helpers.DefaultPort);
            _client = new CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceClient(channel);

            _cancellationTokenSource = new CancellationTokenSource();

            AsyncServerStreamingCall<GrpcExportResult> handShaking = _client.GrpcHandShaking(new GrpcHandShakingRequest() {RequestorName = "Mixa"});

            IAsyncStreamReader<GrpcExportResult> stream = handShaking.ResponseStream;

            _readingStreamTask = Task.Run(() => ReadingStreamAsync(stream));
        }

        private async Task ReadingStreamAsync(IAsyncStreamReader<GrpcExportResult> stream)
        {
            try
            {
                while (await stream.MoveNext(_cancellationTokenSource.Token))
                {
                    GrpcExportResult current = stream.Current;
                }
            }
            catch (RpcException ex)
            {
                switch (ex.Status.StatusCode)
                {
                    case StatusCode.Cancelled:
                        break;

                    default:
                        throw;
                }
            }
        }

        public void Stop()
        {
            if (_readingStreamTask.IsCompleted)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _readingStreamTask.Wait();
        }

        public void SendRequest(int index)
        {
            if (_client == null)
            {
                throw new Exception("Client was not connected");
            }

            GrpcCmmResult r1 = _client.GrpcDoCmmExport(new GrpcCmmExportRequest() {Index = index});
        }
    }
}