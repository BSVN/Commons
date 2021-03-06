﻿using BSN.Commons.Infrastructure;
using BSN.Commons.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : DbContext, IDbContext
    {
        public UnitTestContext(DbContextOptions options) : base(options) 
        {

        }

        public UnitTestContext()
        {

        }

        public static UnitTestContext Create(DbContextOptions options)
        {
            return new UnitTestContext(options);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        public override int SaveChanges()
		{
            return base.SaveChanges();
		}
	}
}
