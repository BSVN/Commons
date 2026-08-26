using System;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Interface for Database Context
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Save changes to the database.
        /// </summary>
        int SaveChanges();        
    }
}