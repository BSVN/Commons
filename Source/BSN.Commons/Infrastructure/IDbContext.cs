using System;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Interface for Database Context
    /// </summary>
    public interface IDbContext : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Save changes to the database.
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Save changes to the database asynchronously.
        /// </summary>
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}