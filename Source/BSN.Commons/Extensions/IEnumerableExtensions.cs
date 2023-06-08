using System.Linq;
using System.Collections.Generic;

namespace BSN.Commons.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool HasDuplicates<TSource>(this IEnumerable<TSource> source)
        {
            return source.Count() != source.Distinct().Count();
        }

        public static IEnumerable<TSource> FindDuplicates<TSource>(this IEnumerable<TSource> source)
        {
            return source?.GroupBy(P => P)
                         .Where(Q => Q.Count() > 1)
                         .Select(Z => Z.Key);
        }

        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }
    }
}
