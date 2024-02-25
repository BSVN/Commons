using Microsoft.Extensions.Options;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace BSN.Commons.Infrastructure.Redis
{
    /// <summary>
    /// Redis Database Context
    /// </summary>
    public class RedisDbContext : IRedisDbContext
    {
        /// <summary>
        /// Constructor of Redis Database Context
        /// </summary>
        /// <param name="options">Redis Connection Options</param>
        public RedisDbContext(IOptions<RedisConnectionOptions> options)
        {
            _provider = new RedisConnectionProvider(options.Value.ConnectionString);
        }

        /// <inheritdoc/>
        public virtual int SaveChanges()
        {
            throw new System.NotImplementedException("We don't have a way to save changes on redis om yet.");
        }

        /// <inheritdoc/>
        public IRedisConnectionProvider GetConnectionProvider()
        {
            return _provider;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // TODO release managed resources here
        }

        private IRedisConnectionProvider _provider;
    }
}