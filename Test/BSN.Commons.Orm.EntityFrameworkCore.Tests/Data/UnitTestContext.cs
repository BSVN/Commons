using BSN.Commons.Infrastructure;
using BSN.Commons.Tests;
using Microsoft.EntityFrameworkCore;

namespace BSN.Commons.Test.Data
{
    public class UnitTestContext : DbContext, IDbContext, IAsyncDbContext
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
    }
}
