using System.Runtime.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Pagination meta data.
    /// </summary>
    [DataContract]
    public class PaginationMetadata
    {
        public PaginationMetadata() { }

        /// <summary>
        /// Current page number
        /// </summary>
        [DataMember(Order = 1)]
        public uint Page { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        [DataMember(Order = 2)]
        public uint PageCount { get; set; }

        /// <summary>
        /// Number of records per page
        /// </summary>
        [DataMember(Order = 3)]
        public uint PageSize { get; set; }

        /// <summary>
        /// Total number of records
        /// </summary>
        [DataMember(Order = 4)]
        public uint RecordCount { get; set; }
    }
}
