using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BSN.Resa.Vns.Commons.Converters
{
    public class JsonDateWithoutTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.SpecifyKind(DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }
    }
}
