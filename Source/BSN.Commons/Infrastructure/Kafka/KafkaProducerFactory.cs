using Confluent.Kafka;
using System.Collections.Concurrent;

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
            _producers = new ConcurrentDictionary<string, KafkaProducer<T>>();
        }

        /// <inheritdoc />
        public IKafkaProducer<T> Create(string topic)
        {
            return _producers.GetOrAdd(topic, t => new KafkaProducer<T>(_sharedProducerEngine, t));
        }

        /// <inheritdoc />
        public void Dispose() 
        {
            _sharedProducerEngine?.Dispose();
            _producers.Clear();
        }

        private readonly IProducer<Null, T> _sharedProducerEngine;
        private readonly ConcurrentDictionary<string, KafkaProducer<T>> _producers;
    }
}