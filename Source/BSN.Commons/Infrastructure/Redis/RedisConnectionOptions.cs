namespace BSN.Commons.Infrastructure.Redis
{
    /// <summary>
    /// Redis Connection Options
    /// </summary>
    public class RedisConnectionOptions
    {
        /// <summary>
        /// Connection String to connect to Redis for example: localhost:6379
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
