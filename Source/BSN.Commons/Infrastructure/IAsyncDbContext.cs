using System;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Async Interface for Database Context
    /// </summary>
    public interface IAsyncDbContext
    {
        /// <summary>
        /// Save changes to the database asynchronously.
        /// </summary>
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}