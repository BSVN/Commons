using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Text;

namespace BSN.Commons.Test.Data
{
    public class TestContext : DbContext, global::Commons.Infrastructure.IDbContext
    {
        public TestContext(string connString) : base(connString)
        {

        }

        public static TestContext Create()
        {
            return new TestContext("Server=(localdb)\\mssqllocaldb;Database=TestContext;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<TestContext>(new DropCreateDatabaseAlways<TestContext>());

            modelBuilder.Entity<User>().HasOptional(P => P.Document).WithRequired(Q => Q.User);
            base.OnModelCreating(modelBuilder);
        }

		void global::Commons.Infrastructure.IDbContext.SaveChanges()
		{
            this.SaveChanges();
		}

		global::Commons.Infrastructure.IDbSet<TEntity> global::Commons.Infrastructure.IDbContext.Set<TEntity>()
		{
            return (global::Commons.Infrastructure.IDbSet<TEntity>)this.Set<TEntity>();
		}
	}
}
