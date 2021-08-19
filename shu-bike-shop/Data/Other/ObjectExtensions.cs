using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace shu_bike_shop
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object o, bool pretty = true)
        {
            return JsonConvert.SerializeObject(o, pretty ? Formatting.Indented : Formatting.None);
        }
    }
}
