using BSN.Commons.Extensions;
using BSN.Commons.Infrastructure;
using BSN.Commons.Orm.EntityFrameworkCore.Extensions;
using Sieve.Exceptions;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BSN.Commons.Orm.EntityFrameworkCore
{
    /// <summary>
    /// Default implementation of <see cref="IDynamicFilterableRepository{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicFilterableRepositoryBase<T> : RepositoryBase<T>, IDynamicFilterableRepository<T> where T : class
    {
        /// <inheritdoc />
        protected DynamicFilterableRepositoryBase(IDatabaseFactory databaseFactory, ISieveProcessor sieveProcessor) : base(databaseFactory)
        {
            SieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        /// <inheritdoc />
        public PagedEntityCollection<T> GetMany(Expression<Func<T, bool>> where, string filters, string sorts, uint pageNumber, uint pageSize)
        {
            if (pageNumber == 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageNumber));

            if (pageSize == 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageSize));

            IQueryable<T> query = dbSet.Where(where);

            try
            {
                query = SieveProcessor.Apply(new SieveModel() { Filters = filters, Sorts = sorts, PageSize = (int?)pageSize, Page = (int?)pageNumber }, query, applyPagination: false);
            }
            catch (SieveException ex)
            {
                throw new InvalidOperationException(message: ex.ExtractMessage());
            }

            return query.Paginate(pageNumber, pageSize);
        }

        /// <inheritdoc />
        public PagedEntityCollection<T> GetMany(string filters, string sorts, uint pageNumber, uint pageSize)
        {
            return GetMany((entity) => true, filters, sorts, pageNumber, pageSize);
        }

        /// <summary>
        /// The engine of filtering 
        /// </summary>
        /// <remarks>
        /// So if you want to any extra filtering or changing behaviour of current filtering, you have to use this engine
        /// </remarks>
        protected ISieveProcessor SieveProcessor { get; }
    }
}