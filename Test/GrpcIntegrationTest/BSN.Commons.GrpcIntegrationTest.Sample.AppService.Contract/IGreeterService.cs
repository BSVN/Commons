using BSN.Commons.PresentationInfrastructure;
using BSN.Commons.Responses;
using ProtoBuf.Grpc;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract
{
    [DataContract]
    public class SayHelloViewModel
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }

    [DataContract]
    public class HelloRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
    }

    [ServiceContract]
    public interface IGreeterService
    {
        [OperationContract]
        Response SayHello(HelloRequest request,
            CallContext context = default);
    }
}
