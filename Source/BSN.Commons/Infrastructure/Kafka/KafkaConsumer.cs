using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumer<T> : IKafkaConsumer<T>
    {
        /// <param name="consumer">the consumer engine provided by factory class</param>
        internal KafkaConsumer(IConsumer<Null, T> consumer)
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

        internal void Dispose()
        {
            _consumer.Dispose();
        }
        
        private readonly IConsumer<Null, T> _consumer;
    }
}