namespace BSN.Commons.Infrastructure.Redis
{
    /// <summary>
    /// Redis Connection Options
    /// </summary>
    public class RedisConnectionOptions
    {
        /// <summary>
        /// Contains the name of the configuration section to bind to. In this scenario,
        /// the options object provides the name of its configuration section.
        /// </summary>
        public const string ConfigurationSectionName = "RedisConnectionOptions";

        /// <summary>
        /// Connection String to connect to Redis for example: localhost:6379
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
