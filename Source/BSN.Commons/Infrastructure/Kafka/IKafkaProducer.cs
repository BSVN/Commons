using System;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents an Exception that is raised when Kafka Produce was not successful
    /// </summary>
    /// <typeparam name="T">The object type of the producer</typeparam>
    public class KafkaProduceException<T> : Exception {}
    
    /// <summary>
    /// Represents A Kafka Api compatible producer class
    /// </summary>
    /// <typeparam name="T">The object type that is being produced</typeparam>
    public interface IKafkaProducer<T>
    {
        /// <summary>
        /// Produces the Kafka api, raises exceptions for errors
        /// </summary>
        /// <param name="message">The message that is being produced</param>
        /// <exception cref="KafkaProduceException{T}"></exception>
        /// <returns>Whether the produce operation was successful or not</returns>
        void ProduceAsync(T message);
    }
}