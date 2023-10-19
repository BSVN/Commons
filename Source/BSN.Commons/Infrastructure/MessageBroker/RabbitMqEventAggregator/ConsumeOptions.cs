using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Represents options for configuring a RabbitMQ exchange.
    /// </summary>
    public class ConsumeOptions
    {
        /// <summary>
        /// Gets or sets whether automatic acknowledgment (auto-ack) is enabled.
        /// </summary>
        public bool AutoAck { get; set; } = false;
        
        /// <summary>
        /// Gets or sets whether consuming messages from the same channel is prohibited.
        /// </summary>
        public bool NoLocal { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the consumer is exclusive, meaning it can only be accessed by the declaring connection.
        /// </summary>
        public bool Exclusive { get; set; } = false;

        /// <summary>
        /// Gets or sets the delivery mode for consumed messages, such as persistent or non-persistent.
        /// </summary>
        public DeliveryMode DeliveryMode { get; set; } = DeliveryMode.Persistent;

        /// <summary>
        /// Gets or sets additional arguments for message consumption configuration.
        /// </summary>
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
