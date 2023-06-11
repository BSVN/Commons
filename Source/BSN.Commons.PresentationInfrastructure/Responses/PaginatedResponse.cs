using BSN.Commons.Converters;
using BSN.Commons.PresentationInfrastructure;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Generic response base for paginated data.
    /// </summary>
    /// <remarks>
    /// Paginated response provides metadata for navigation purposes.
    /// </remarks>
    /// <typeparam name="T">Data type.</typeparam>
    [DataContract]
    public class PaginatedResponse<T> where T : class
    {
        public PaginatedResponse() { }

        /// <summary>
        /// Corresponding HttpStatusCode.
        /// </summary>
        [DataMember(Order = 2)]
        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        /// <summary>
        /// Data payload (Collection).
        /// </summary>
        [DataMember(Order = 1)]
        public CollectionViewModel<T> Data { get; set; }

        /// <summary>
        /// Human-readable message for the End-User.
        /// </summary>
        [DataMember(Order = 3)]
        public string Message { get; set; }

        /// <summary>
        /// Pagination metadata used by the client for data navigation purposes.
        /// </summary>
        [DataMember(Order = 4)]
        public PaginationMetadata Meta { get; set; }

        /// <summary>
        /// Invalid items of the request object.
        /// </summary>
        [DataMember(Order = 5)]
        public IList<InvalidItem> InvalidItems { get; set; }

        /// <summary>
        /// Distinction between successful and unsuccessful result.
        /// </summary>
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }
}
