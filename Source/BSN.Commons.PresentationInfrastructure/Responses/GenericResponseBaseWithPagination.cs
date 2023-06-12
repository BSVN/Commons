using System;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Generic response base for paginated data.
    /// </summary>
    /// <remarks>
    /// Paginated response provides metadata for navigation purpose.
    /// </remarks>
    /// <typeparam name="T">Data type.</typeparam>
    [Obsolete("Due to incompatability with Grpc this response type is only used for backward compatibility.")]
    public class GenericResponseBaseWithPagination<T> : GenericResponseBase<T> where T : class
    {
        public GenericResponseBaseWithPagination() { }

        /// <summary>
        /// Pagination metadata used by the client for data navigation purposes.
        /// </summary>
        public PaginationMetadata Meta { get; set; }
    }
}
