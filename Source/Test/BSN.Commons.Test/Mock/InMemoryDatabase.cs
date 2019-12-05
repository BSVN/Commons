using Commons.Infrastructure;
using Effort;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace BSN.Commons.Test
{

    internal class InMemoryDatabaseFactory : Disposable, IDatabaseFactory
    {
        private DbContext _dataContext;

        static InMemoryDatabaseFactory()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
        }

        public DbContext Get()
        {
            if (_dataContext == null)
            {
                var InMemoryconnection = DbConnectionFactory.CreateTransient();
                _dataContext = new Context(InMemoryconnection);
                _dataContext.Database.Initialize(false);
                return _dataContext;
            }
            else
                return _dataContext;
        }

        public new void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
                _dataContext = null;
                GC.SuppressFinalize(this);
            }
        }

        protected override void DisposeCore()
        {
            if (_dataContext != null)
                _dataContext.Dispose();
        }
    }


    public class User
    {
        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Document Document { get; set; }
        public string Password { get; set; }
    }

    public class Document
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
    }

    public class Context : DbContext
    {
        public Context(System.Data.Common.DbConnection Connection) : base(Connection, true)        
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasRequired(P => P.Document);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Document> Documents { get; set; }
    }

}
