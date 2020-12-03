using BSN.Commons.Test.Infrastructure;
using BSN.Commons.Infrastructure;
using NUnit.Framework;
using BSN.Commons.Test.Mock;
using BSN.Commons.Tests;

namespace BSN.Commons.Test
{
    [TestFixture]
    public class UnitOfWorkTest : UnitOfWorkTestBase
    {
        public override UnitOfWorkArrangementsAbstractFactory ArrangemenetAbstractFactoryMethod()
        {
            return new EFCoreUnitOfWorkTestFactory();
        }
    }

    public class EFCoreUnitOfWorkTestFactory : UnitOfWorkArrangementsAbstractFactory
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