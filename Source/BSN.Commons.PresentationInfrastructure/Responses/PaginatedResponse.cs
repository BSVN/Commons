using BSN.Commons.Converters;
using BSN.Commons.PresentationInfrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public IEnumerable<T> Data { get; set; }

        [DataMember(Order = 3)]
        public string Message { get; set; }

        [DataMember(Order = 4)]
        public PaginationMetadata Meta { get; set; }

        [DataMember(Order = 5)]
        public IList<ValidationResult> InvalidItems { get; set; }

        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }

}
