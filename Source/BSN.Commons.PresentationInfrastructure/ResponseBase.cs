using BSN.Commons.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BSN.Commons.PresentationInfrastructure
{
    public class ResponseBase : IResponse<ValidationResult>
    {
        /// <summary>
        /// Gets a value that indicates whether the HTTP response was successful.
        /// </summary>
        /// <remarks>
        /// If the server doesn't return a successful HttpStatusCode in the Successful range (200-299) for the request, then the responseObject.IsSuccess property is set to false
        /// </remarks>
        /// More info: https://docs.microsoft.com/en-us/uwp/api/windows.web.http.httpresponsemessage.issuccessstatuscode?view=winrt-19041
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public string Message { get; set; }

        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public ResponseStatusCode StatusCode { get; set; }

        public IList<ValidationResult> InvalidItems { get; set; }
    }
}
