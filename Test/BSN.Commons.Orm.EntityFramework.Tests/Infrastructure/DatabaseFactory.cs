using BSN.Commons.Test.Data;
using Commons.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace BSN.Commons.Test.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext _dataContext;


        public IDbContext Get()
        {
            return (IDbContext)(_dataContext ?? (_dataContext = TestContext.Create()));
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }
}
