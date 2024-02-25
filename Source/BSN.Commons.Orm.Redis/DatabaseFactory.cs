using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using System.Diagnostics.CodeAnalysis;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Database Factory for Redis
    /// </summary>
    public class DatabaseFactory<TDbContext> : Disposable, IDatabaseFactory where TDbContext : Redis.DbContext, ICreatable<IOptions<RedisConnectionOptions>, TDbContext>
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

        /// <summary>
        /// Redis Connection Options
        /// </summary>
        protected IOptions<RedisConnectionOptions> RedisConnectionOptions => redisConnectionOptions;

        private readonly IOptions<RedisConnectionOptions> redisConnectionOptions;
        private IDbContext _redisDbContext;
    }
}
