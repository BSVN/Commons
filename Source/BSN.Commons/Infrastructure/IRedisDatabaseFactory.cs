using System;
using System.Collections.Generic;
using System.Text;
using Redis.OM;

namespace BSN.Commons.Infrastructure
{
    public interface IRedisDatabaseFactory
    {
        RedisConnectionProvider Get();
    }
}
