using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Database Factory for Redis
    /// </summary>
    public class DatabaseFactory<TDbContext> : Disposable, IDatabaseFactory where TDbContext : DbContext, ICreatable<IOptions<RedisConnectionOptions>, TDbContext>
    {
        /// <summary>
        /// Constructor of Redis Database Factory
        /// </summary>
        /// <param name="configuration">App configuration</param>
        public DatabaseFactory(IConfiguration configuration)
        {
            redisConnectionOptions = Options.Create(configuration.GetSection("Redis").Get<RedisConnectionOptions>());
        }

        /// <summary>
        /// Constructor of Redis Database Factory
        /// </summary>
        /// <param name="options"></param>
        public DatabaseFactory(IOptions<RedisConnectionOptions> options)
        {
            redisConnectionOptions = options;
        }

        /// <inheritdoc/>
        public IDbContext Get()
        {
            return _redisDbContext ?? (_redisDbContext = TDbContext.Create(RedisConnectionOptions));
        }
        /// <inheritdoc/>
        public IAsyncDbContext GetAsyncContext()
        {
            return _asyncRedisDbContext ?? (_asyncRedisDbContext = TDbContext.Create(RedisConnectionOptions));
        }

        /// <summary>
        /// Redis Connection Options
        /// </summary>
        protected IOptions<RedisConnectionOptions> RedisConnectionOptions => redisConnectionOptions;

        private readonly IOptions<RedisConnectionOptions> redisConnectionOptions;
        private IDbContext _redisDbContext;
        private IAsyncDbContext _asyncRedisDbContext;
    }
}
