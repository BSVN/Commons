using BSN.Commons.Infrastructure;
using BSN.Commons.Test.Infrastructure;
using BSN.Commons.Test.Mock;
using BSN.Commons.Tests;
using NUnit.Framework;

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

        public override IAsyncRepository<User> CreateAsyncUserRepository(IDatabaseFactory databaseFactory)
        {
            return new UserRepository(databaseFactory);
        }
    }
}
