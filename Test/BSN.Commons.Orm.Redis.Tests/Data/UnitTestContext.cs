﻿using BSN.Commons.Infrastructure;
using BSN.Commons.Infrastructure.Redis;
using BSN.Commons.Orm.Redis;
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
    public class UnitTestContext : Orm.Redis.DbContext, ICreatable<IOptions<RedisConnectionOptions>, UnitTestContext>
    {
        public UnitTestContext(IOptions<RedisConnectionOptions> options) : base(options) 
        {

        }

        public new static UnitTestContext Create(IOptions<RedisConnectionOptions> options) => new UnitTestContext(options);

        public IRedisCollection<User> Users { get; set; }
        public IRedisCollection<Document> Documents { get; set; }

        public override int SaveChanges()
		{
            return base.SaveChanges();
		}
	}
}
