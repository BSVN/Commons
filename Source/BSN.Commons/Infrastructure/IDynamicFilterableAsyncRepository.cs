using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Adds dynamic filterable query capabilities to the
    /// <see cref="IAsyncRepository{T}"/> pattern.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public interface IDynamicFilterableAsyncRepository<T> : IAsyncRepository<T>
        where T : class
    {
        /// <summary>
        /// Asynchronously gets a paginated list of objects based on <paramref name="filters"/>.
        /// </summary>
        /// <remarks>
        /// This method retrieves objects based on filters and applies pagination.
        /// To retrieve all objects that match the filters, you need to iterate through all pages.
        /// </remarks>
        /// <param name="filters">
        /// A comma-delimited list of <c>{Name}{Operator}{Value}</c>.
        /// </param>
        /// <param name="sorts">
        /// A comma-delimited ordered list of property names to sort by.
        /// Adding a <c>-</c> before the name switches to descending order.
        /// </param>
        /// <param name="pageNumber">The number of the page to return.</param>
        /// <param name="pageSize">The number of items returned per page.</param>
        /// <param name="cancellationToken">
        /// A token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A task containing a paginated list of matched objects.
        /// </returns>
        Task<PagedEntityCollection<T>> GetManyAsync(
            string filters,
            string sorts,
            uint pageNumber,
            uint pageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously gets a paginated list of objects based on
        /// <paramref name="filters"/> within the scope of <paramref name="where"/>.
        /// </summary>
        /// <param name="where">A function to test each element for a condition.</param>
        /// <param name="filters">A comma-delimited list of dynamic filters.</param>
        /// <param name="sorts">
        /// A comma-delimited ordered list of property names to sort by.
        /// </param>
        /// <param name="pageNumber">The number of the page to return.</param>
        /// <param name="pageSize">The number of items returned per page.</param>
        /// <param name="cancellationToken">
        /// A token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A task containing a paginated list of matched objects.
        /// </returns>
        Task<PagedEntityCollection<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            string filters,
            string sorts,
            uint pageNumber,
            uint pageSize,
            CancellationToken cancellationToken = default);
    }
}