using System.Threading.Tasks;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    public class GrpcServer
    {
        private readonly Grpc.Core.Server _grpcServer;

        public GrpcServer(ServerServiceDefinition serviceDefinition, string serverHost, int serverPort)
        {
            _grpcServer = new Grpc.Core.Server()
            {
                Services = {serviceDefinition},
                Ports = {new ServerPort(serverHost, serverPort, ServerCredentials.Insecure)}
            };
        }

        public void Start() 
        {
            _grpcServer.Start();
        }

        public void Stop() 
        {
            StopAsync().Wait();
        }

        public async Task StopAsync()
        {
            await _grpcServer.KillAsync();
        }
    }
}
