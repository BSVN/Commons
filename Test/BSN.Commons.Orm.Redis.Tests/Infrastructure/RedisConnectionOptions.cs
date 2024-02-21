using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSN.Commons.Orm.Redis.Tests.Infrastructure
{
    public class RedisConnectionOptions
    {
        public RedisConnectionOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
    }
}
