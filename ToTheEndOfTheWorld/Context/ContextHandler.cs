using Newtonsoft.Json;
using System.IO;

namespace ToTheEndOfTheWorld.Context
{
    public static class ContextHandler
    {
        private static string BasePath => Path.Combine(Path.GetTempPath(), "TTEOTW");

        public static void SaveWorld(ModelWorld world)
        {
            var file = GetWorldFilePath();
            File.WriteAllText(
                file,
                UtilityLibrary.Extensions.Compress(
                    JsonConvert.SerializeObject(
                        world,
                typeof(ModelWorld),
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        }
                    )
                )
            );
        }

        public static ModelWorld? LoadWorld()
        {
            var file = GetWorldFilePath();

            if (new FileInfo(file).Length == 0)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ModelWorld>(
                UtilityLibrary.Extensions.Decompress(
                    File.ReadAllText(file)
                ),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                }
            )!;
        }

        private static string GetWorldFilePath()
        {
            var file = Path.Combine(BasePath, "World.txt");
            CreateDirectoryIfNotExists(file);
            CreateFileIfNotExists(file);
            return file;
        }

        private static void CreateDirectoryIfNotExists(string file)
        {
            var directory = Path.GetDirectoryName(file);

            if (string.IsNullOrWhiteSpace(directory) || Directory.Exists(directory))
            {
                return;
            }

            Directory.CreateDirectory(directory);
        }

        private static void CreateFileIfNotExists(string file)
        {
            if (File.Exists(file)) return;
            using var f = File.Create(file);
            f.Close();
        }
    }
}
