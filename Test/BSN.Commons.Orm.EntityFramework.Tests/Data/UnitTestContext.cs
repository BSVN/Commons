using BSN.Commons.Infrastructure;
using BSN.Commons.Tests;
using System.Data.Entity;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : DbContext, IDbContext
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

		public override int SaveChanges()
		{
            return base.SaveChanges();
		}
	}
}
