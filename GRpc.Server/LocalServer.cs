using System.Threading.Tasks;
using GRpc.API;
using Grpc.Core;
using GrpcGreeter;

namespace GRpc.Server
{
    public class LocalServer
    {
        private const string DefaultHost = "localhost";
        private const int DefaultPort = 50051;

        private readonly Grpc.Core.Server _grpcServer;

        public LocalServer(string serverHost = DefaultHost, int serverPort = DefaultPort)
        {
            _grpcServer = new Grpc.Core.Server()
            {
                Services = { Greeter.BindService(new GreetServiceImpl()) },
                Ports = { new ServerPort(serverHost, serverPort,ServerCredentials.Insecure) }
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
