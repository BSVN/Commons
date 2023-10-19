using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class ExchangeOptions
    {
        public string BrokerName { get; set; } = "Resa_event_bus";
        public string BrokerType { get; set; } = "direct";
        public bool Durable { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
