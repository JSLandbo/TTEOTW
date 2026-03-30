using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace UtilityLibrary
{
    public static class Extensions
    {
        private static readonly JsonSerializerOptions JsonCopyOptions = new()
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public static T CopyObject<T>(this object objSource)
        {
            string json = JsonSerializer.Serialize(objSource, objSource.GetType(), JsonCopyOptions);
            return JsonSerializer.Deserialize<T>(json, JsonCopyOptions)!;
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            using MemoryStream ms = new();
            XmlSerializer serializer = new(obj!.GetType());
            serializer.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            return (T)serializer.Deserialize(ms)!;
        }

        public static object DeepCopyJson(object o)
        {
            string json = JsonSerializer.Serialize(o, o.GetType(), JsonCopyOptions);
            return JsonSerializer.Deserialize(json, o.GetType(), JsonCopyOptions)!;
        }

        public static string Compress(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            using MemoryStream msi = new(bytes);
            using MemoryStream mso = new();
            using (GZipStream gs = new(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return Convert.ToBase64String(mso.ToArray());
        }

        public static string Decompress(string s)
        {
            byte[] bytes = Convert.FromBase64String(s);
            using MemoryStream msi = new(bytes);
            using MemoryStream mso = new();
            using (GZipStream gs = new(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }

            return Encoding.Unicode.GetString(mso.ToArray());
        }
    }
}
