using BSN.Commons.Infrastructure;
using BSN.Commons.Test.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BSN.Commons.Test.Infrastructure
{
    internal class InMemoryDatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext _dataContext;

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
                _dataContext = new UnitTestContext(new DbContextOptionsBuilder()
                                                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                       .Options);

                return (IDbContext)_dataContext;
            }
            else
                return (IDbContext)_dataContext;
        }

        IAsyncDbContext IDatabaseFactory.GetAsyncContext()
        {            
            if (_dataContext == null)
            {
                _dataContext = new UnitTestContext(new DbContextOptionsBuilder()
                                                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                       .Options);

                return (IAsyncDbContext)_dataContext;
            }
            else
                return (IAsyncDbContext)_dataContext;
        }
    }
}
