using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents A Kafka Api compatible producer class
    /// </summary>
    /// <typeparam name="T">The object type that is being produced</typeparam>
    public interface IKafkaProducer<T>
    {
        /// <summary>
        /// Produces the Kafka api
        /// </summary>
        /// <param name="message">The message that is being produced</param>
        /// <returns>Whether the produce operation was successful or not</returns>
        Task<bool> ProduceAsync(T message);
    }
}