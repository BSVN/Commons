using System.Collections.Generic;
using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaProducerFactory<T> : IKafkaProducerFactory<T>
    {
        /// TODO: Ebrahim: Use Lazy Pattern
        /// <param name="options">Default Options for KafkaProducers</param>
        public KafkaProducerFactory(IKafkaProducerOptions options)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
            };
            
            _sharedProducerEngine = new ProducerBuilder<Null, T>(producerConfig).Build();
            _producers = new Dictionary<string, KafkaProducer<T>>();
        }

        /// <inheritdoc />
        public IKafkaProducer<T> Create(string topic)
        {
            if (_producers.ContainsKey(topic))
            {
                return _producers[topic];
            }
            
            var producer = new KafkaProducer<T>(_sharedProducerEngine, topic);
            
            _producers.Add(topic, producer);
            
            return producer;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _sharedProducerEngine?.Dispose();
        }

        private readonly IProducer<Null, T> _sharedProducerEngine;
        private readonly Dictionary<string, KafkaProducer<T>> _producers;
    }
}