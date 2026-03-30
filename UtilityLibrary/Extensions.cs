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
            var json = JsonSerializer.Serialize(objSource, objSource.GetType(), JsonCopyOptions);
            return JsonSerializer.Deserialize<T>(json, JsonCopyOptions)!;
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            using var ms = new MemoryStream();
            var serializer = new XmlSerializer(obj!.GetType());
            serializer.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);
            return (T)serializer.Deserialize(ms)!;
        }

        public static object DeepCopyJson(object o)
        {
            var json = JsonSerializer.Serialize(o, o.GetType(), JsonCopyOptions);
            return JsonSerializer.Deserialize(json, o.GetType(), JsonCopyOptions)!;
        }

        public static string Compress(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                msi.CopyTo(gs);
            }

            return Convert.ToBase64String(mso.ToArray());
        }

        public static string Decompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using var msi = new MemoryStream(bytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                gs.CopyTo(mso);
            }

            return Encoding.Unicode.GetString(mso.ToArray());
        }
    }
}
