using BSN.Commons.Converters;
using BSN.Commons.PresentationInfrastructure;
using BSN.Commons.Utilities;
using ProtoBuf;
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
    // TODO: [ProtoInclude(100, typeof(ErrorResponse))]
    // TODO: [ProtoInclude(101, typeof(Response<>))]
    public class Response: IResponse<InvalidItem>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Response() { }

        /// <inheritdoc/>
        [DataMember(Order = 1)]
        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        /// <inheritdoc/>
        [DataMember(Order = 2)]
        public string Message { get; set; }

        /// <inheritdoc/>
        [DataMember(Order = 3)]
        public IList<InvalidItem> InvalidItems { get; set; }

        /// <inheritdoc/>
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
    // TODO: [ProtoImplement(typeof(Response))]
    public class Response<T> : Response where T : class
    {
        /// <summary>
        /// Data payload.
        /// </summary>
        [DataMember(Order = 2)]
        public T Data { get; set; }
    }

    /// <summary>
    /// Generic error response type for command/query services to return the error results.
    /// </summary>
    [ProtoImplement(typeof(Response))]
    public class ErrorResponse : Response
    {
    }
}
