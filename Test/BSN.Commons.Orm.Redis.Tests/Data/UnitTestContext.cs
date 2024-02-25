using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using BSN.Commons.Tests;
using Microsoft.Extensions.Options;
using Redis.OM.Searching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : RedisDbContext
    {
        public UnitTestContext(IOptions<RedisConnectionOptions> options) : base(options) 
        {

        }

        public static UnitTestContext Create(IOptions<RedisConnectionOptions> options)
        {
            return new UnitTestContext(options);
        }

        public IRedisCollection<User> Users { get; set; }
        public IRedisCollection<Document> Documents { get; set; }

        public override int SaveChanges()
		{
            return base.SaveChanges();
		}
	}
}
