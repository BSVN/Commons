using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Text;
using System.Transactions;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : DbContext, global::Commons.Infrastructure.IDbContext
    {
        public UnitTestContext(System.Data.Common.DbConnection dbConnection) : base(dbConnection, false) 
        {
            
        }

        public UnitTestContext()
        {

        }

        public static UnitTestContext Create(System.Data.Common.DbConnection dbConnection)
        {
            return new UnitTestContext(dbConnection);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UnitTestContext>(new DropCreateDatabaseAlways<UnitTestContext>());

            modelBuilder.Entity<User>().HasOptional(P => P.Document).WithRequired(Q => Q.User);
            base.OnModelCreating(modelBuilder);
        }

		int global::Commons.Infrastructure.IDbContext.SaveChanges()
		{
            return this.SaveChanges();
		}
	}
}
