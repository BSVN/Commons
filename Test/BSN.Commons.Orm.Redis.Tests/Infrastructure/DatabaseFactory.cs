using BSN.Commons.Test.Data;
using BSN.Commons.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using BSN.Commons.Infrastructure.Redis;

namespace BSN.Commons.Test.Infrastructure
{
    internal class InMemoryDatabaseFactory : Disposable, IDatabaseFactory
    {
        private RedisDbContext _dataContext;

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }

        public new void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
                GC.SuppressFinalize(this);
            }
        }

        IDbContext IDatabaseFactory.Get()
        {
            if (_dataContext == null)
            {
                // TODO: Use UseInMemoryDatabase after implemented https://github.com/redis/redis-om-dotnet/issues/437
                _dataContext = new UnitTestContext(new DbContextOptionsBuilder()
                                                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                       .Options);

                return (IDbContext)_dataContext;
            }
            else
                return (IDbContext)_dataContext;
        }
    }
}
