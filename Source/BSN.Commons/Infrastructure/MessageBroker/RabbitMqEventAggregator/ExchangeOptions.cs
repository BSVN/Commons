using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Represents options for configuring a RabbitMQ exchange.
    /// </summary>
    public class ExchangeOptions
    {
        /// <summary>
        /// Gets or sets the name of the RabbitMQ exchange.
        /// </summary>
        public string BrokerName { get; set; } = "Resa_event_bus";

        /// <summary>
        /// Gets or sets the type of RabbitMQ exchange, such as direct, topic, headers, or fanout.
        /// </summary>
        public BrokerType ExchangeType { get; set; } = BrokerType.direct;

        /// <summary>
        /// Gets or sets whether the exchange is durable, meaning it survives server restarts.
        /// </summary>
        public bool Durable { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the exchange is exclusive, meaning it can only be accessed by the declaring connection.
        /// </summary>
        public bool Exclusive { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the exchange is automatically deleted when the last consumer unsubscribes.
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        /// Gets or sets additional arguments for the exchange.
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
