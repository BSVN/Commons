using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BSN.Commons.Converters
{
    /// <summary>
    ///  Force to convert an object or value to or from JSON in default form. 
    /// </summary>
    /// <typeparam name="T"> The type of object or value handled by the converter. </typeparam> 
    public class JsonForceDefaultConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
