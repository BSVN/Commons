using BSN.Commons.GrpcIntegrationTest.Sample.AppService.Contract;
using BSN.Commons.GrpcIntegrationTest.Sample.Service.Services;
using BSN.Commons.PresentationInfrastructure;
using BSN.Commons.Responses;
using BSN.Commons.Utilities;
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
                //GrpcPolymorphismActivator.Enable(typeof(Startup).Assembly);
                //GrpcPolymorphismActivator.Enable(typeof(Response).Assembly);
                ProtoBuf.Meta.RuntimeTypeModel.Default.Add(typeof(Response), false)
                    .Add(1, nameof(Response.StatusCode))
                    .Add(2, nameof(Response.Message))
                    .Add(3, nameof(Response.InvalidItems))
                    .AddSubType(100, typeof(ErrorResponse))
                    .AddSubType(101, typeof(Response<SayHelloViewModel>));

                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                                   "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
        }
    }
}
