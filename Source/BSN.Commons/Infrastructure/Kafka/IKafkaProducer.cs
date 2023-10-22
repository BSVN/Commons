using System;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents an Exception that is raised when Kafka Produce was not successful
    /// </summary>
    /// <typeparam name="T">The object type of the producer</typeparam>
    public class KafkaProduceException<T> : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="KafkaProduceException{T}"/>
        /// </summary>
        /// <param name="message">The error message of the exception</param>
        public KafkaProduceException(string message) : base(message) {}
    }
    
    /// <summary>
    /// Represents A Kafka Api compatible producer class
    /// </summary>
    /// <typeparam name="T">The object type that is being produced</typeparam>
    public interface IKafkaProducer<T> : IDisposable
    {
        /// <summary>
        /// Produces the Kafka api, raises exceptions for errors
        /// </summary>
        /// <param name="message">The message that is being produced</param>
        /// <exception cref="KafkaProduceException{T}"></exception>
        Task ProduceAsync(T message);
        
        /// <summary>
        /// Produces the Kafka api, raises exceptions for errors
        /// </summary>
        /// <param name="message">The message that is being produced</param>
        /// <exception cref="KafkaProduceException{T}"></exception>
        void Produce(T message);
    }
}