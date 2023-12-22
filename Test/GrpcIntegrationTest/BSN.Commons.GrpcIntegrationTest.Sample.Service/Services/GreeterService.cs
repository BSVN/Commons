using BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract;
using ProtoBuf.Grpc;

namespace BSN.Commons.GrpcIntegrationTest.Sample.Service.Services
{
    public class GreeterService : IGreeterService
    {
        public Task<HelloReply> SayHelloAsync(HelloRequest request, CallContext context = default)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello {request.Name}"
            });
        }
    }
}
