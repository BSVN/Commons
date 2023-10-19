using System.Threading.Tasks;
using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        public KafkaConsumer(IConsumer<Null, T> consumer)
        {
            _consumer = consumer;
        }

        public Task<T> ConsumeAsync()
        {
            return Task.Run(() =>
            {
                ConsumeResult<Null, T> result = _consumer.Consume();

                return result.Message.Value;
            });
        }
        
        private readonly IConsumer<Null, T> _consumer;
    }
}