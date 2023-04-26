using System.Collections.Generic;

namespace BSN.Commons
{
    /// <summary>
    /// Stores the paginated data for the given entity type.
    /// </summary>
    /// <typeparam name="T">Type of Entity for which pagination is being implemented.</typeparam>
    public class PagedEntityCollection<T>
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public uint CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public uint PageCount { get; set; }

        /// <summary>
        /// Number of records per page
        /// </summary>
        public uint PageSize { get; set; }

        /// <summary>
        /// Total number of records that exist for the query
        /// </summary>
        public uint RecordCount { get; set; }

        /// <summary>
        /// IEnumerable of the paginated data
        /// </summary>
        public IEnumerable<T> Results { get; set; }
    }
}
