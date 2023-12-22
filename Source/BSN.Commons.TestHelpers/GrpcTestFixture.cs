using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace BSN.Commons.TestHelpers
{
    public delegate void LogMessage(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception? exception);

    public class GrpcTestFixture<TStartup> : IDisposable where TStartup : class
    {
        private TestServer? _server;
        private HttpMessageHandler? _handler;
        private Action<IWebHostBuilder>? _configureWebHost;
        private readonly WebApplicationFactory<TStartup> _factory;

        public event LogMessage? LoggedMessage;

        public GrpcTestFixture()
        {
            LoggerFactory = new LoggerFactory();
            LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) =>
            {
                LoggedMessage?.Invoke(logLevel, category, eventId, message, exception);
            }));

            _factory = new WebApplicationFactory<TStartup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<ILoggerFactory>(LoggerFactory);
                    });
                });
        }

        public void ConfigureWebHost(Action<IWebHostBuilder> configure)
        {
            _configureWebHost = configure;
        }

        private void EnsureServer()
        {
            if (_server == null)
            {
                _server = _factory.Server;
                _handler = _server.CreateHandler();
            }
        }

        public LoggerFactory LoggerFactory { get; }

        public HttpMessageHandler Handler
        {
            get
            {
                EnsureServer();
                return _handler!;
            }
        }

        public void Dispose()
        {
            _handler?.Dispose();
            _server?.Dispose();
        }

        public IDisposable GetTestContext(TextWriter outputHelper)
        {
            return new GrpcTestContext<TStartup>(this, outputHelper);
        }
    }
}
