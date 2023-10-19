using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    public class KafkaProducerFactory<T> : IKafkaProducerFactory<T>
    {
        public KafkaProducerFactory(IKafkaProducerOptions options)
        {
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
            };
            
            // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
            // is int and can not accept high values that we expect
            producerConfig.Set("receive.message.max.bytes", options.ReceiveMessageMaxBytes);
            
            _sharedProducer = new ProducerBuilder<Null, T>(producerConfig).Build();
        }

        public IKafkaProducer<T> Create(string topic)
        {
            return new KafkaProducer<T>(_sharedProducer, topic);
        }

        private readonly IProducer<Null, T> _sharedProducer;
    }
}