namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Represents configuration options for RabbitMQ event aggregation.
    /// </summary>
    public class RabbitMqOptions
    {
        public RabbitMqOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqOptions"/> class with exchange, queue, and consume options.
        /// </summary>
        /// <param name="exchangeOption">The exchange options.</param>
        /// <param name="queueOption">The queue options.</param>
        /// <param name="consumeOption">The consume options.</param>
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
