using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaProducer<T> : IKafkaProducer<T>
    {
        /// <param name="producer">the producer engine provided by creator for example factory class</param>
        /// <param name="topic">Represents the topic which the producer will produce on,
        /// Read https://dattell.com/data-architecture-blog/what-is-a-kafka-topic/ for further info.</param>
        public KafkaProducer(IProducer<Null, T> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }

        /// <inheritdoc />
        public async void ProduceAsync(T message)
        {
            var messageObject = new Message<Null, T> { Value = message };

            try
            {
                await _producer.ProduceAsync(_topic, messageObject);
            }
            catch (ProduceException<Null, T> e)
            {
                throw new KafkaProduceException<T>(e.Error.Reason);
            }
        }

        private readonly string _topic;

        private readonly IProducer<Null, T> _producer;
    }
}