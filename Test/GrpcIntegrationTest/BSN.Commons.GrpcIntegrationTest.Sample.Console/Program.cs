using BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BSN.Commons.GrpcIntegrationTest.Sample.Console
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {

            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            using var channel = GrpcChannel.ForAddress("http://localhost:5279");
            var client = channel.CreateGrpcService<IGreeterService>();

            var reply = client.SayHello(
                new HelloRequest { Name = "GreeterClient" });

            System.Console.WriteLine($"Greeting: {reply.Message}");
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }

        private static bool ValidateServerCertificate(
                       object sender,
                       X509Certificate? certificate,
                       X509Chain? chain,
                       SslPolicyErrors sslPolicyErrors)
        {
            ArgumentNullException.ThrowIfNull(sender);

            return true; // Always accept
        }
    }
}
