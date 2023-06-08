using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace BSN.Commons.Extensions
{
    public static class CsvExtensions
    {
        public static string ToCsv<T>(this IEnumerable<T> collection) where T : class
        {
            StringBuilder csvContentBuilder = new StringBuilder();

            Type genericType = Activator.CreateInstance<T>().GetType();
            PropertyInfo[] properties = genericType.GetProperties();

            StringBuilder csvHeaderBuilder = new StringBuilder();
            for (int i = 0; i < properties.Length; i++)
            {
                if (i + 1 < properties.Length)
                {
                    var hasNameAttribute = properties[i].GetCustomAttribute(typeof(JsonPropertyNameAttribute)) != null;

                    if (hasNameAttribute)
                    {
                        var serializationName = properties[i].GetCustomAttribute<JsonPropertyNameAttribute>().Name;

                        csvContentBuilder.Append(serializationName);
                    }
                    else
                    {
                        csvContentBuilder.Append(properties[i].Name);
                    }

                    csvContentBuilder.Append(",");
                }
                else
                {
                    csvContentBuilder.Append(properties[i].Name);
                }
            }

            csvContentBuilder.AppendLine(csvHeaderBuilder.ToString());

            foreach (T item in collection)
            {
                StringBuilder csvRecordBuilder = new StringBuilder();
                for (int i = 0; i < properties.Length; i++)
                {
                    object value = genericType.GetProperty(properties[i].Name).GetValue(item, null);

                    if (i + 1 < properties.Length)
                    {
                        if (value != null)
                        {
                            csvRecordBuilder.Append(value.ToString());
                            csvRecordBuilder.Append(",");
                        }
                        else
                        {
                            csvRecordBuilder.Append(",");
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            csvRecordBuilder.Append(value.ToString());
                        }
                    }
                }

                csvContentBuilder.AppendLine(csvRecordBuilder.ToString());
            }

            return csvContentBuilder.ToString();
        }
    }
}
