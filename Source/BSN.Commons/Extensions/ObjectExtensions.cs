using System.Text.Json;

namespace BSN.Commons.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeToJson(this object @object)
        {
            return JsonSerializer.Serialize(@object);
        }
    }
}
