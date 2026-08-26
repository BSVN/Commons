using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BSN.Commons.Infrastructure
{
    /// <summary>
    /// Async Repository Pattern Interface for abstract communicating with DataBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IAsyncRepository<T> where T : class
	{
        /// <summary>
        /// Add new object asynchronously.
        /// </summary>
        Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Add a range of objects asynchronously.
        /// </summary>
        Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Get object by identifier asynchronously.
        /// </summary>
        Task<T> GetByIdAsync<KeyType>(
            KeyType id,            
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Get object using expression asynchronously.
        /// </summary>
        Task<T> GetAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Get all objects asynchronously.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync(
            bool asNoTracking = false,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// Get objects using expression asynchronously.
        /// </summary>
        Task<IEnumerable<T>> GetManyAsync(
            Expression<Func<T, bool>> where,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default);
    }
}
