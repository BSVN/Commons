using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using Microsoft.Extensions.Options;
using Redis.OM;
using Redis.OM.Contracts;
using Redis.OM.Searching;
using StackExchange.Redis;

namespace BSN.Commons.Orm.Redis
{
    /// <summary>
    /// Redis Database Context
    /// </summary>
    public class DbContext : RedisConnectionProvider, IDbContext, ICreatable<IOptions<RedisConnectionOptions>, DbContext>
    {
        /// <summary>
        /// Constructor of Redis Database Context
        /// </summary>
        /// <param name="options">Redis Connection Options</param>
        public DbContext(IOptions<RedisConnectionOptions> options)
            : base(options.Value.ConnectionString)
        {
        }

        /// <summary>
        /// Static factory method to create a new instance of the <see cref="DbContext"/>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static DbContext Create(IOptions<RedisConnectionOptions> options)
        {
            return new DbContext(options);
        }

        /// <inheritdoc/>
        public virtual int SaveChanges()
        {
            throw new System.NotImplementedException("We don't have a way to save changes on redis om yet.");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}