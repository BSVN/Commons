using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using BSN.Commons.Orm.Redis.Tests.Dto;

namespace BSN.Commons.Orm.Redis.Tests.Mock
{
    public sealed class UserRepository : RepositoryBase<User>, IRepository<User>
    {
        public UserRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        { }
    }
}
