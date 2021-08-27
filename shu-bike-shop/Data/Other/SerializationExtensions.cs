using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace shu_bike_shop
{
    public static class SerializationExtensions
    {
        public static string ToJson(this object o, bool pretty = true)
        {
            return JsonConvert.SerializeObject(o, pretty ? Formatting.Indented : Formatting.None);
        }

        public static string ToBson(this object o)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, o);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static IEnumerable<T> FromBson<T>(string s)
        {
            byte[] data = Convert.FromBase64String(s);

            MemoryStream ms = new MemoryStream(data);
            using (BsonDataReader reader = new BsonDataReader(ms))
            {
                reader.ReadRootValueAsArray = true;
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<IList<T>>(reader);
            }
        }
    }
}
