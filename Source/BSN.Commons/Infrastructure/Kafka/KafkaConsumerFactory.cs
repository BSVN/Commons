using Confluent.Kafka;
using System;
using System.Collections.Concurrent;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerFactory<T> : IKafkaConsumerFactory<T>
    {
        /// TODO: Ebrahim: Use Lazy Pattern
        /// <param name="options">Default Options for KafkaConsumers</param>
        public KafkaConsumerFactory(IKafkaConsumerOptions options)
        {
            _defaultConsumerOptions = options ?? throw new ArgumentNullException(nameof(options));
            _consumers = new ConcurrentDictionary<string, KafkaConsumer<T>>();
        }

        /// <inheritdoc/>
        public IKafkaConsumer<T> Create(string topic, string groupId)
        {
            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentException("Topic cannot be null or empty.", nameof(topic));

            if (string.IsNullOrWhiteSpace(groupId))
                throw new ArgumentException("GroupId cannot be null or empty.", nameof(groupId));

            var consumerKey = $"{topic}:{groupId}";

            return _consumers.GetOrAdd(consumerKey, _ =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = _defaultConsumerOptions.BootstrapServers,
                    AutoOffsetReset = AutoOffsetReset.Earliest,
                    GroupId = groupId
                };


                // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
                // is int and can not accept high values that we expect
                config.Set(
                    "receive.message.max.bytes",
                    _defaultConsumerOptions.ReceiveMessageMaxBytes);

                // Here Null means that the key in kafka message is null
                // it helps equal distribution of messages in the kafka cluster
                var consumerEngine = new ConsumerBuilder<Null, T>(config).Build();

                consumerEngine.Subscribe(topic);

                return new KafkaConsumer<T>(consumerEngine);
            });
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Value.Dispose();
            }
            _consumers.Clear();
        }
        
        private readonly ConcurrentDictionary<string, KafkaConsumer<T>> _consumers;
        private readonly IKafkaConsumerOptions _defaultConsumerOptions;
    }
}