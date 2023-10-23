using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerFactory<T> : IKafkaConsumerFactory<T>
    {
        /// TODO: Ebrahim: Use Lazy Pattern
        /// <param name="options">Default Options for KafkaConsumers</param>
        public KafkaConsumerFactory(IKafkaConsumerOptions options)
        {
            _defaultConsumerOptions = options;
        }

        /// <inheritdoc />
        public IKafkaConsumer<T> Create(string topic, string groupId)
        {
            var consumerKey = topic + ":" + groupId;

            if (_consumers.ContainsKey(consumerKey))
            {
                return new KafkaConsumer<T>(_consumers[consumerKey]);
            }

            var config = new ConsumerConfig() 
            {
                BootstrapServers = _defaultConsumerOptions.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };


            // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
            // is int and can not accept high values that we expect
            config.Set("receive.message.max.bytes", _defaultConsumerOptions.ReceiveMessageMaxBytes);

            var consumer = new ConsumerBuilder<Null, T>(config).Build();
            consumer.Subscribe(topic);

            _consumers.Add(consumerKey, consumer);
                
            return new KafkaConsumer<T>(consumer);
        }

        private readonly Dictionary<string, IConsumer<Null, T>> _consumers;
        private IKafkaConsumerOptions _defaultConsumerOptions;

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Value.Dispose();
            }
        }
    }
}