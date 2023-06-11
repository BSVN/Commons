using BSN.Commons.Converters;
using BSN.Commons.PresentationInfrastructure;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// Basic response for command services to return the result of an operation.
    /// </summary>
    /// <remark>
    /// During serialization and deserialization operations, enumeration values are always converted to and from strings.
    /// This is possible through registering the 'JsonStringEnumConverter' in our DI infrastructure. There is once exception,
    /// namely the 'StatusCode' property of the 'ResponseBase' class which should keep it's default numeral value when being converted.
    /// </remark>
    [DataContract]
    public class Response
    {
        public Response() { }

        /// <summary>
        /// Corresponding HttpStatusCode.
        /// </summary>
        [DataMember(Order = 1)]
        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        /// <summary>
        /// Human-readable message for the End-User.
        /// </summary>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// Invalid items of the request object.
        /// </summary>
        [DataMember(Order = 3)]
        public IList<InvalidItem> InvalidItems { get; set; }

        /// <summary>
        /// Distinction between successful and unsuccessful result.
        /// </summary>
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }

    /// <summary>
    /// Generic response type for command/query services to return the results.
    /// </summary>
    /// <remark>
    /// During serialization and deserialization operations, enumeration values are always converted to and from strings.
    /// This is possible through registering the 'JsonStringEnumConverter' in our DI infrastructure. There is once exception,
    /// namely the 'StatusCode' property of the 'ResponseBase' class which should keep it's default numeral value when being converted.
    /// </remark>
    [DataContract]
    public class Response<T> where T : class
    {
        /// <summary>
        /// Corresponding HttpStatusCode.
        /// </summary>
        [DataMember(Order = 1)]
        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        /// <summary>
        /// Data payload.
        /// </summary>
        [DataMember(Order = 2)]
        public T Data { get; set; }

        /// <summary>
        /// Human-readable message for the End-User.
        /// </summary>
        [DataMember(Order = 3)]
        public string Message { get; set; }

        /// <summary>
        /// Invalid items of the request object.
        /// </summary>
        [DataMember(Order = 4)]
        public IList<InvalidItem> InvalidItems { get; set; }

        /// <summary>
        /// Distinction between successful and unsuccessful result.
        /// </summary>
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }
}
