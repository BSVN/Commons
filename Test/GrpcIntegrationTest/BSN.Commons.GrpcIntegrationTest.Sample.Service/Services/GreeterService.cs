using BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract;
using BSN.Commons.PresentationInfrastructure;
using BSN.Commons.Responses;
using ProtoBuf.Grpc;

namespace BSN.Commons.GrpcIntegrationTest.Sample.Service.Services
{
    public class GreeterService : IGreeterService
    {
        public IResponse<InvalidItem> SayHello(HelloRequest request, CallContext context = default)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ErrorResponse()
                {
                    InvalidItems = new List<InvalidItem> { new InvalidItem() { Name = nameof(request.Name), Reason = "Name is required" } },
                    Message = "Invalid request",
                    StatusCode = ResponseStatusCode.BadRequest
                };

            return new Response<SayHelloViewModel>()
            {
                Message = $"Hello {request.Name}"
            };
        }
    }
}
