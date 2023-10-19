using System;
using RabbitMQ.Client;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Represents an interface for managing connections to RabbitMQ.
    /// </summary>
    public interface IRabbitMqConnection : IDisposable
    {
        /// <summary>
        /// Indicates whether the connection to RabbitMQ is established.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Returns the username associated with the RabbitMQ connection.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Attempts to establish a connection to the RabbitMQ server.
        /// </summary>
        /// <returns><c>true</c> if the connection is established; otherwise, <c>false</c>.</returns>
        bool TryConnect();


        /// <summary>
        /// Creates a new RabbitMQ channel for message processing.
        /// </summary>
        /// <returns>A new RabbitMQ channel for message processing.</returns>
        IModel CreateModel();
    }
}
