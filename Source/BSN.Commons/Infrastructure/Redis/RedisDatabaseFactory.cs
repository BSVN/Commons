using BSN.Commons.Infrastructure;
using Microsoft.Extensions.Options;
using Redis.OM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redis.OM.Contracts;
using BSN.Commons.Infrastructure.Redis;

namespace BSN.Commons.Infrastructure.Redis
{
    /// <summary>
    /// Database Factory for Redis
    /// </summary>
    public class RedisDatabaseFactory : IDatabaseFactory
    {
        /// <summary>
        /// Constructor of Redis Database Factory
        /// </summary>
        /// <param name="redisDbContext">Redis Database Context</param>
        public RedisDatabaseFactory(IRedisDbContext redisDbContext)
        {
            _redisDbContext = redisDbContext;
        }

        /// <inheritdoc/>
        public IDbContext Get()
        {
            return _redisDbContext;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // TODO release managed resources here
        }

        private readonly IRedisDbContext _redisDbContext;
    }
}
