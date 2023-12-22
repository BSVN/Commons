using BSN.Commons.GrpcIntegrationTest.Sample.Service.Services;
using ProtoBuf.Grpc.Server;

namespace BSN.Commons.GrpcIntegrationTest.Sample.Service
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            services.AddCodeFirstGrpc();
        }

        public void Configure<TApp>(TApp app, IWebHostEnvironment env) where TApp : IApplicationBuilder
        {
            // Configure the HTTP request pipeline.
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                                   "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
        }
    }
}
