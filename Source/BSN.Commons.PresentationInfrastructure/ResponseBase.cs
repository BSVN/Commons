using BSN.Commons.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BSN.Commons.PresentationInfrastructure
{
    [Obsolete("Due to incompatability with Grpc this response type is only used for backward compatibility.")]
    [DataContract]
    public class ResponseBase : IResponse<ValidationResult>
    {
        [DataMember(Order = 1)]
        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        [DataMember(Order = 2)]
        public string Message { get; set; }

        [DataMember(Order = 3)]
        public IList<ValidationResult> InvalidItems { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the HTTP response was successful.
        /// </summary>
        /// <remarks>
        /// If the server doesn't return a successful HttpStatusCode in the Successful range (200-299) for the request, then the responseObject.IsSuccess property is set to false
        /// </remarks>
        /// More info: https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpresponsemessage.issuccessstatuscode?view=winrt-19041
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
    }

    [Obsolete("Due to incompatability with Grpc this response type is only used for backward compatibility.")]
    [DataContract]
    public class ErrorResponseBase : ResponseBase
    {
    }
}
