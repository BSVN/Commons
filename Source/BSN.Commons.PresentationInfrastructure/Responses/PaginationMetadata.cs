namespace BSN.Commons.Responses
{
    /// <summary>
    /// Stores the paginated meta data.
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public uint Page { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public uint PageCount { get; set; }

        /// <summary>
        /// Number of records per page
        /// </summary>
        public uint PageSize { get; set; }

        /// <summary>
        /// Total number of records that exist
        /// </summary>
        public uint RecordCount { get; set; }
    }
}
