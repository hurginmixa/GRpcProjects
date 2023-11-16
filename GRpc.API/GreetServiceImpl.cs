using System.Threading.Tasks;
using Grpc.Core;
using GrpcGreeter;

namespace GRpc.API
{
    public class GreetServiceImpl : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply {Message = $"Text : '{request.Name}'", Count = request.Name.Length});
        }
    }
}
