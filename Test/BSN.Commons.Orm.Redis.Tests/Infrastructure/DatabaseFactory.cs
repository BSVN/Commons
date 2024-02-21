using BSN.Commons.Infrastructure;
using Microsoft.Extensions.Options;
using Redis.OM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.Redis.Tests.Infrastructure
{
    public class DatabaseFactory : IRedisDatabaseFactory
    {
        public DatabaseFactory(IOptions<RedisConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }
        public RedisConnectionProvider Get()
        {
            string connectionString = _connectionOptions.ConnectionString;
            return _provider ?? (_provider = new RedisConnectionProvider(connectionString));
        }
        private RedisConnectionProvider _provider;
        private readonly RedisConnectionOptions _connectionOptions;
    }
}
