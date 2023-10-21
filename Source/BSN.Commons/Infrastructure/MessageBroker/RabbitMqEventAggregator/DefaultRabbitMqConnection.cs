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
    /// <summary>
    /// Represents a default implementation of an RabbitMQ connection.
    /// </summary>
    public class DefaultRabbitMqConnection : IRabbitMqConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRabbitMqConnection"/> class with the specified options and logger.
        /// </summary>
        /// <param name="options">The RabbitMQ connection options.</param>
        /// <param name="logger">The logger for capturing log messages.</param>
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

        /// <inheritdoc />
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Disposes of the RabbitMQ connection and resources.
        /// </summary>
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

        /// <inheritdoc />
        public bool IsConnected => _connection != null && _connection.IsOpen && !_isDisposed;

        /// <summary>
        /// The underlying RabbitMQ connection instance.
        /// </summary>
        public IConnection _connection { get; private set; }

        /// <summary>
        /// Returns the username associated with the RabbitMQ connection.
        /// </summary>
        public string UserName => _connectionFactory.UserName;

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

        private readonly IConnectionFactory _connectionFactory;
        private readonly int _retryCount;
        private readonly ILogger _logger;
        private bool _isDisposed;
    }
}
