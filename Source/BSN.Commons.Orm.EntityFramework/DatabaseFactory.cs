using BSN.Commons.Infrastructure;
using System.Data.Entity;

namespace BSN.Commons.Orm.EntityFramework
{
    public class DatabaseFactory<TContext> : Disposable, IDatabaseFactory where TContext : DbContext, IDbContext, IAsyncDbContext
    {
        public DatabaseFactory(TContext context)
        {
            _dataContext = context;
        }

        public IDbContext Get()
        {
            return _dataContext;
        }

        public void Dispose()
        {
            _dataContext?.Dispose();
        }

        public IAsyncDbContext GetAsyncContext()
        {
            return _dataContext;
        }
        protected TContext _dataContext;
    }
}