using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        /// <param name="consumer">the consumer engine provided by creator for example factory class</param>
        public KafkaConsumer(IConsumer<Null, T> consumer)
        {
            _consumer = consumer;
        }

        /// <inheritdoc />
        public Task<T> ConsumeAsync(CancellationToken ct = default)
        {
            return Task.Run(() =>
            {
                ConsumeResult<Null, T> result = _consumer.Consume(ct);

                return result.Message.Value;
            });
        }
        
        private readonly IConsumer<Null, T> _consumer;

        /// <inheritdoc />
        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}