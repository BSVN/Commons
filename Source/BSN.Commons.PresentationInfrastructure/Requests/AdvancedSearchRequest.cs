using System.Runtime.Serialization;

namespace BSN.Commons.Requests
{
    /// <summary>
    /// Advanced search request for pagination and Sieve processing.
    /// <see href="https://github.com/Biarity/Sieve"/>
    /// </summary>
    [DataContract]
    public class AdvancedSearchRequest
    {
        /// <summary>
        /// Encoded sieve filters.
        /// </summary>
        [DataMember(Order = 1)]
        public string Filters { get; set; }

        /// <summary>
        /// Encoded sieve sorts.
        /// </summary>
        [DataMember(Order = 2)] 
        public string Sorts { get; set; }

        /// <summary>
        /// Page number (Start from 1).
        /// </summary>
        [DataMember(Order = 3)]
        public uint PageNumber { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        [DataMember(Order = 4)]
        public uint PageSize { get; set; }
    }
}
