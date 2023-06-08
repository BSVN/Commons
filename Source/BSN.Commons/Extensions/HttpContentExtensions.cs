using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BSN.Commons.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsyncCaseInsensitive<T>(this HttpContent content) where T : class
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

            return JsonSerializer.Deserialize<T>(await content.ReadAsStringAsync(), options);
        }
    }
}
