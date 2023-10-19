using System.Threading.Tasks;
using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    public class KafkaProducer<T> : IKafkaProducer<T>
    {
        public KafkaProducer(IProducer<Null, T> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }

        public Task<bool> ProduceAsync(T message)
        {
            var messageObject = new Message<Null, T> { Value = message };

            return Task.Run(async () =>
            {
                DeliveryResult<Null, T> deliveryResult = await _producer.ProduceAsync(_topic, messageObject);

                return deliveryResult.Status == PersistenceStatus.Persisted;
            });
        }

        private readonly string _topic;

        private readonly IProducer<Null, T> _producer;
    }
}