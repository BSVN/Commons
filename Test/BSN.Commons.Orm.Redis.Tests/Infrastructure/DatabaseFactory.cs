using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using BSN.Commons.Orm.Redis;
using BSN.Commons.Test.Data;
using Microsoft.Extensions.Options;
using Redis.OM;

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
