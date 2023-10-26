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
            _consumers = new Dictionary<string, KafkaConsumer<T>>();
        }

        /// <summary>
        /// Creates a new Kafka consumer with the specified topic and group id.
        /// this returns a thread safe consumer and can be used in multi-threaded environments.
        /// for multiple calls with the same topic and group id, the same consumer will be returned.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IKafkaConsumer<T> Create(string topic, string groupId)
        {
            var consumerKey = topic + ":" + groupId;

            if (_consumers.ContainsKey(consumerKey))
            {
                return _consumers[consumerKey];
            }

            var config = new ConsumerConfig() 
            {
                BootstrapServers = _defaultConsumerOptions.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };


            // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
            // is int and can not accept high values that we expect
            config.Set("receive.message.max.bytes", _defaultConsumerOptions.ReceiveMessageMaxBytes);

            var consumerEngine = new ConsumerBuilder<Null, T>(config).Build();
            consumerEngine.Subscribe(topic);

            var consumer = new KafkaConsumer<T>(consumerEngine);
                
            _consumers.Add(consumerKey, consumer);

            return consumer;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Value.Dispose();
            }
        }
        
        private readonly Dictionary<string, KafkaConsumer<T>> _consumers;
        private readonly IKafkaConsumerOptions _defaultConsumerOptions;
    }
}