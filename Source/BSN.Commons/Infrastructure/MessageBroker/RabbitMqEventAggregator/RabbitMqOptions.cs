namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public class RabbitMqOptions
    {
        public RabbitMqOptions()
        {
        }

        public RabbitMqOptions(ExchangeOptions exchangeOprion, QueueOptions queueOption, ConsumeOptions cunsumeOption)
        {
            ExchangeOptions = exchangeOprion;
            QueueOptions = queueOption;
            CunsumeOptions = cunsumeOption;
        }

        public ExchangeOptions ExchangeOptions { get; set; }

        public QueueOptions QueueOptions { get; set; }

        public ConsumeOptions CunsumeOptions { get; set; }

        public int RetryCount { get; set; } = 5;
    }
}
