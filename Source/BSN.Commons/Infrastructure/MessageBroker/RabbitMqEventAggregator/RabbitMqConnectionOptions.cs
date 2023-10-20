namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class RabbitMqConnectionOptions
    {
        public string HostName { get; set; }

        public bool DispatchConsumersAsync { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string VirtualHost { get; set; }

        public int RetryCount { get; set; }
    }
}
