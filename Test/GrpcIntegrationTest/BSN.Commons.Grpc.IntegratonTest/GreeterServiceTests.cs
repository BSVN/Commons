using BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract;
using BSN.Commons.GrpcIntegrationTest.Sample.Service;
using BSN.Commons.GrpcIntegrationTest.Sample.Service.Services;
using BSN.Commons.TestHelpers;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Grpc.IntegratonTest
{
    [TestFixture]
    internal class GreeterServiceTests : IntegrationTestBase<GrpcIntegrationTest.Sample.Service.Startup>
    {
        public GreeterServiceTests() : base(new GrpcTestFixture<Startup>(), TestContext.Out)
        {
        }

        [Test]
        public async Task SayHelloTest()
        {
            // Arrenge
            var client = Channel.CreateGrpcService<IGreeterService>();

            // Act
            var reply = client.SayHello(new HelloRequest { Name = "GreeterClient" });

            // Assert
            Assert.That(reply, Is.Not.Null);
            Assert.That(reply.Message, Is.EqualTo("Hello GreeterClient"));
        }
    }
}
