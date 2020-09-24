using BSN.Commons.Test.Data;
using Commons.Infrastructure;
using Commons.Orm.EntityFramework;

namespace BSN.Commons.Test.Mock
{
    public sealed class UserRepository : RepositoryBase<User>, IRepository<User>
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        { }
    }
}
