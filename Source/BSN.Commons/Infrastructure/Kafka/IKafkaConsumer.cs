using System;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents A Kafka Api compatible consumer class
    /// </summary>
    /// <typeparam name="T">The object type that is being consumed</typeparam>
    public interface IKafkaConsumer<T> : IDisposable
    {
        /// <summary>
        /// Consumes the Kafka api 
        /// </summary>
        /// <returns>a task which will return the consumed message after its ready</returns>
        Task<T> ConsumeAsync(CancellationToken ct = default);
    }
}