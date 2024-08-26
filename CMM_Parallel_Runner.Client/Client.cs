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
        private readonly Task _task;

        public Client()
        {
            Channel channel =  Helpers.MakeChannel(Helpers.DefaultHost, Helpers.DefaultPort);
            _client = new CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceClient(channel);

            _cancellationTokenSource = new CancellationTokenSource();

            _task = Task.Run(ReadStreamTask);
        }

        private async Task ReadStreamTask()
        {
            AsyncServerStreamingCall<GrpcExportResult> handShaking = _client.GrpcHandShaking(new GrpcHandShakingRequest() {RequestorName = "Mixa"});

            IAsyncStreamReader<GrpcExportResult> stream = handShaking.ResponseStream;

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
                        break;
                }
            }
        }

        public void Stop()
        {
            if (_task.IsCompleted)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _task.Wait();
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