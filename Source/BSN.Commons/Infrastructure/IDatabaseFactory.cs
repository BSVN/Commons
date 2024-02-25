using System;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Interface for Database Factory
    /// </summary>
    public interface IDatabaseFactory : IDisposable
    {
        /// <summary>
        /// Get the database context
        /// </summary>
        /// <returns></returns>
        IDbContext Get();
    }
}
