using Redis.OM.Contracts;

namespace BSN.Commons.Infrastructure.Redis
{
    /// <summary>
    /// Interface for Redis Database Context
    /// </summary>
    public interface IRedisDbContext : IDbContext
    {
        /// <summary>
        /// Get the connection provider for Redis
        /// </summary>
        IRedisConnectionProvider GetConnectionProvider();
    }
}
