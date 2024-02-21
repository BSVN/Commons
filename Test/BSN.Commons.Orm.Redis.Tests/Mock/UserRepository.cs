using BSN.Commons.Infrastructure;
using BSN.Commons.Orm.Redis.Tests.Dto;

namespace BSN.Commons.Orm.Redis.Tests.Mock
{
    public sealed class UserRepository : RepositoryBase<User>, IRedisRepository<User>
    {
        public UserRepository(IRedisDatabaseFactory databaseFactory) : base(databaseFactory)
        { }
    }
}
