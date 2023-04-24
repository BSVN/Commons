using BSN.Commons.Converters;
using BSN.Commons.PresentationInfrastructure;
using System.Text.Json.Serialization;

namespace BSN.Commons.Responses
{
    /// <summary>
    /// During serialization and deserialization operations, enumeration values are always converted to and from strings.
    /// This is possible through registering the 'JsonStringEnumConverter' in our DI infrastructure. There is once exception,
    /// namely the 'StatusCode' property of the 'ResponseBase' class which should keep it's default numeral value when being converted.
    /// </summary>
    public class Response : ResponseBase
    {
        public new bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        [JsonConverter(typeof(JsonForceDefaultConverter<ResponseStatusCode>))]
        public new ResponseStatusCode StatusCode { get; set; }
    }
}
