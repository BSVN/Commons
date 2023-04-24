using BSN.Commons.Dto;
using System;
using System.Linq;

namespace BSN.Commons.Extensions
{
    public static partial class IQueryableExtensions
    {
        /// <summary>
        /// Paginate IQueryable of <T>
        /// with given pageNumber and pageSize
        /// </summary>
        public static PagedEntityCollection<T> Paginate<T>(this IQueryable<T> query, uint pageNumber, uint pageSize)
        {
            if (pageNumber == 0)
                throw new ArgumentException("Must be greater than zero.", nameof(pageNumber));

            if (pageSize == 0)
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
    }
}
