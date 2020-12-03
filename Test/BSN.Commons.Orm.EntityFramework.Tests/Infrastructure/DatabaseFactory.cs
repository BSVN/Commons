using BSN.Commons.Test.Data;
using BSN.Commons.Infrastructure;
using Effort;
using Effort.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BSN.Commons.Test.Infrastructure
{
    internal class InMemoryDatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext _dataContext;

        static InMemoryDatabaseFactory()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

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
                EffortConnection InMemoryconnection = DbConnectionFactory.CreateTransient();
                _dataContext = UnitTestContext.Create(InMemoryconnection);
                _dataContext.Database.Initialize(false);
                return (IDbContext)_dataContext;
            }
            else
                return (IDbContext)_dataContext;
        }
    }
}
