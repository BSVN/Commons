using BSN.Commons.Test.Infrastructure;
using BSN.Commons.Infrastructure;
using NUnit.Framework;
using BSN.Commons.Test.Data;
using System.Collections.Generic;
using BSN.Commons.Test.Mock;
using System;
using System.Threading.Tasks;
using System.Linq;
using BSN.Commons.Tests;

namespace BSN.Commons.Test
{
    [TestFixture]
    public class UnitOfWorkTest : UnitOfWorkTestBase
    {
        public override UnitOfWorkArrangementsAbstractFactory ArrangemenetAbstractFactoryMethod()
        {
            return new EFUnitOfWorkTestFactory();
        }
    }

    public class EFUnitOfWorkTestFactory : UnitOfWorkArrangementsAbstractFactory
    {
        public override IDatabaseFactory CreateDatabaseFactory()
        {
            return new InMemoryDatabaseFactory();
        }

        public override IRepository<User> CreateUserRepository(IDatabaseFactory databaseFactory)
        {
            return new UserRepository(databaseFactory);
        }
    }
}
