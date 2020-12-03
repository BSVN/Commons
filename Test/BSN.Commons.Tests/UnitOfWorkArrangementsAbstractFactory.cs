using BSN.Commons.Infrastructure;

namespace BSN.Commons.Tests
{
    public abstract class UnitOfWorkArrangementsAbstractFactory
    {
        public abstract IDatabaseFactory CreateDatabaseFactory();
        public abstract IRepository<User> CreateUserRepository(IDatabaseFactory databaseFactory);
    }

}
