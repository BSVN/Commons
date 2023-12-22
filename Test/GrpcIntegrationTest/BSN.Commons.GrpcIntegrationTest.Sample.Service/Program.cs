using BSN.Commons.GrpcIntegrationTest.Sample.Service.Services;
using ProtoBuf.Grpc.Server;

namespace BSN.Commons.GrpcIntegrationTest.Sample.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var startup = new Startup();
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            startup.Configure(app, builder.Environment);

            app.Run();
        }
    }
}