using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class ConsumeOptions
    {
        public bool AutoAck { get; set; } = false;
        public bool NoLocal { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public DeliveryMode DeliveryMode { get; set; } = DeliveryMode.Persistent;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
