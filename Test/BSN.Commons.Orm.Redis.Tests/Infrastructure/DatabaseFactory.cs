using BSN.Commons.Test.Data;
using BSN.Commons.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using BSN.Commons.Infrastructure.Redis;
using Redis.OM;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using BSN.Commons.Orm.Redis;

namespace BSN.Commons.Test.Infrastructure
{
    internal class InMemoryDatabaseFactory : DatabaseFactory<UnitTestContext>
    {
        public InMemoryDatabaseFactory() : base(Options.Create(new RedisConnectionOptions
        {
            ConnectionString = "redis://localhost:6379"
        }))
        {

        }

        private RedisConnectionProvider _dataContext;

        protected override void DisposeCore()
        {
        }

        public new void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext = null;
                GC.SuppressFinalize(this);
            }
        }

        public IDbContext Get()
        {
            if (_dataContext == null)
            {
                // TODO: Use UseInMemoryDatabase after implemented https://github.com/redis/redis-om-dotnet/issues/437
                _dataContext = new UnitTestContext(RedisConnectionOptions);

                return (IDbContext)_dataContext;
            }
            else
                return (IDbContext)_dataContext;
        }
    }
}
