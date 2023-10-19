namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class RabbitMqOptions
    {
        public RabbitMqOptions()
        {
        }

        public RabbitMqOptions(ExchangeOptions exchangeOption, QueueOptions queueOption, ConsumeOptions consumeOption)
        {
            ExchangeOptions = exchangeOption;
            QueueOptions = queueOption;
            ConsumeOptions = consumeOption;
        }

        public ExchangeOptions ExchangeOptions { get; set; }

        public QueueOptions QueueOptions { get; set; }

        public ConsumeOptions ConsumeOptions { get; set; }

        public int RetryCount { get; set; } = 5;
    }
}
