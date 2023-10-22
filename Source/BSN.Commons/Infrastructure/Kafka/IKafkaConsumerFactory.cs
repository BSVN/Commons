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
        /// Creates a IKafkaConsumer
        /// </summary>
        /// <param name="topic">Represents the topic which the consumer will subscribe on</param>
        /// <param name="groupId">Represents the groupId which the consumer will be a part of</param>
        /// <returns>An IKafkaConsumer instance</returns>
        IKafkaConsumer<T> Create(string topic, string groupId);
    }
}