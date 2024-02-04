using System.Threading.Tasks;
using Grpc.Core;

namespace GRpc.API
{
    public class GrpcServer
    {
        private readonly Server _grpcServer;

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
            _grpcServer.ShutdownAsync().Wait();
        }

        public async Task StopAsync()
        {
            await _grpcServer.ShutdownAsync();
        }
    }
}
