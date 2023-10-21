using System;
using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class QueueOptions
    {
        public string DefaultQueueName { get; set; } = Guid.NewGuid().ToString();
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = true;
        public IDictionary<string, object> Arguments { get; set; } = null;
    }
}
