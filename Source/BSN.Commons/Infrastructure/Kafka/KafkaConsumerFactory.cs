using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerFactory<T> : IKafkaConsumerFactory<T>
    {
        /// TODO: Ebrahim: Use Lazy Pattern
        /// <param name="options">Default Options for KafkaConsumers</param>
        public KafkaConsumerFactory(IKafkaConsumerOptions options)
        {
            _defaultConsumerConfig = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            
            // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
            // is int and can not accept high values that we expect
            _defaultConsumerConfig.Set("receive.message.max.bytes", options.ReceiveMessageMaxBytes);
        }

        /// <inheritdoc />
        public IKafkaConsumer<T> Create(string topic, string groupId)
        {
            lock (this)
            {
                var config = new ConsumerConfig(_defaultConsumerConfig) { GroupId = groupId };
                var consumer = new ConsumerBuilder<Null, T>(config).Build();
                consumer.Subscribe(topic);
                
                return new KafkaConsumer<T>(consumer);
            }
        }

        private readonly ConsumerConfig _defaultConsumerConfig;
    }
}