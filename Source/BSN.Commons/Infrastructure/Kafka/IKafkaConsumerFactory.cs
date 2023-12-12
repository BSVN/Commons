using System;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents a factory class for IKafkaConsumer classes
    /// </summary>
    /// <typeparam name="T">the type which the created consumer classes are based on</typeparam>
    public interface IKafkaConsumerFactory<T> : IDisposable
    { 
        /// <summary>
        /// Creates a new Kafka consumer with the specified topic and group id.
        /// this returns a thread safe consumer and can be used in multi-threaded environments.
        /// for multiple calls with the same topic and group id, the same consumer will be returned.
        /// </summary>
        /// <param name="topic">Represents the topic which the consumer will subscribe on</param>
        /// <param name="groupId">
        /// Represents the groupId which the consumer will be a part of,
        /// it is used by kafka provider to deliver a message to each group only once.
        /// </param>
        /// <returns>An IKafkaConsumer instance</returns>
        IKafkaConsumer<T> Create(string topic, string groupId);
    }
}