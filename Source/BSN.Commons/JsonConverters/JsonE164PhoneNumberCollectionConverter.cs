using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BSN.Commons.JsonConverters
{
    public class JsonE164PhoneNumberCollectionConverter : JsonConverter<string[]>
    {
        public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var list = JsonSerializer.Deserialize<string[]>(ref reader, options);

            if (list == null)
                return null;

            return list.Select(P => JsonE164PhoneNumberConverter.Deserialize(P)).ToArray();
        }

        public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Select(P => JsonE164PhoneNumberConverter.Serialize(P)), options);
        }
    }
}
