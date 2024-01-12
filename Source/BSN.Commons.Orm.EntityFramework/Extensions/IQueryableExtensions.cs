using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BSN.Commons.Extensions
{
    /// <summary>
    /// The place for IQuerubable extenstions
    /// </summary>
    public static partial class IQueryableExtensions
    {
        /// <summary>
        /// Paginate IQueryable of <typeparamref name="T"/>
        /// with given pageNumber and pageSize
        /// </summary>
        public static PagedEntityCollection<T> Paginate<T>(this IQueryable<T> query, uint pageNumber, uint pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageNumber));

            if (pageSize <= 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageSize));

            var result = new PagedEntityCollection<T>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                RecordCount = (uint)query.Count(),
                Results = query.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize).ToList()
            };

            result.PageCount = (uint)Math.Ceiling((double)result.RecordCount / pageSize);

            return result;
        }

        /// <summary>
        /// Paginate IQueryable of <typeparamref name="T"/>
        /// with given pageNumber and pageSize
        /// </summary>
        public static async Task<PagedEntityCollection<T>> PaginateAsync<T>(this IQueryable<T> query, uint pageNumber, uint pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageNumber));

            if (pageSize <= 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageSize));

            var result = new PagedEntityCollection<T>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                RecordCount = (uint) await query.CountAsync(),
                Results = await query.Skip((int)((pageNumber - 1) * pageSize)).Take((int)pageSize).ToListAsync()
            };

            result.PageCount = (uint)Math.Ceiling((double)result.RecordCount / pageSize);

            return result;
        }
    }
}
