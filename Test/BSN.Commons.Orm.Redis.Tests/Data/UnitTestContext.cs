using BSN.Commons.Infrastructure.Redis;
using BSN.Commons.Orm.Redis;
using BSN.Commons.Tests;
using Microsoft.Extensions.Options;
using Redis.OM.Searching;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : DbContext, ICreatable<IOptions<RedisConnectionOptions>, UnitTestContext>
    {
        public UnitTestContext(IOptions<RedisConnectionOptions> options) : base(options) { }

        public new static UnitTestContext Create(IOptions<RedisConnectionOptions> options) => new UnitTestContext(options);

        public IRedisCollection<User> Users { get; set; }
        public IRedisCollection<Document> Documents { get; set; }
    }
}
