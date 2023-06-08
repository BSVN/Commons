using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BSN.Commons.JsonConverters
{
    public class JsonE164PhoneNumberConverter : JsonConverter<string>
    {
        protected const string InternationalPrefixSymbol = "+";

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Deserialize(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Serialize(value));
        }

        public static string Serialize(string value)
        {
            if (value == null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.StartsWith(InternationalPrefixSymbol) ? value : $"{InternationalPrefixSymbol}{value}";
        }

        public static string Deserialize(string value)
        {
            return (value ?? string.Empty).StartsWith(InternationalPrefixSymbol) ? value.Substring(1) : value;
        }
    }
}
