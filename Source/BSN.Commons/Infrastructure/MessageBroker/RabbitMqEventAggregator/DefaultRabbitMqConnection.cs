using System;
using System.IO;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class DefaultRabbitMqConnection : IRabbitMqConnection
    {
        public DefaultRabbitMqConnection(IOptions<RabbitMqConnectionOptions> options, ILogger logger)
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = options.Value.HostName,
                UserName = options.Value.UserName,
                Password = options.Value.Password,
                VirtualHost = options.Value.VirtualHost,
                DispatchConsumersAsync = true
            };

            _retryCount = options.Value.RetryCount;
            _logger = logger;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            var policy = RetryPolicy.Handle<SocketException>().Or<BrokerUnreachableException>()
                   .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                   {
                       _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                   }
               );

            policy.Execute(() =>
            {
                _connection = _connectionFactory.CreateConnection();
            });

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                return false;
            }
        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_isDisposed)
                return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        private void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_isDisposed)
                return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_isDisposed)
                return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                _connection.Dispose();

                _isDisposed = true;
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.Message);
            }
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_isDisposed;
        public IConnection _connection { get; private set; }
        public string UserName => _connectionFactory.UserName;
        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private readonly ILogger _logger;
        private bool _isDisposed;
    }
}
