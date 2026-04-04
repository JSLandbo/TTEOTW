using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ToTheEndOfTheWorld.Context
{
    public static class ContextHandler
    {
        private static string BasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TTEOTW");

        public static void SaveWorld(ModelWorld world)
        {
            string file = GetWorldFilePath();
            string content = SerializeWorld(world);
            File.WriteAllText(file, content);
        }

        public static async Task SaveWorldAsync(ModelWorld world)
        {
            string file = GetWorldFilePath();
            string content = await Task.Run(() => SerializeWorld(world));
            await File.WriteAllTextAsync(file, content);
        }

        public static ModelWorld? LoadWorld()
        {
            string file = GetWorldFilePath();

            if (new FileInfo(file).Length == 0)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ModelWorld>(UtilityLibrary.Extensions.Decompress(File.ReadAllText(file)), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            })!;
        }

        private static string SerializeWorld(ModelWorld world)
        {
            world.SavedPlayerWorldPosition = ResolvePlayerWorldPosition(world);

            return UtilityLibrary.Extensions.Compress(JsonConvert.SerializeObject(world, typeof(ModelWorld),
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }

        private static string GetWorldFilePath()
        {
            string file = Path.Combine(BasePath, "World.txt");
            CreateDirectoryIfNotExists(file);
            CreateFileIfNotExists(file);

            return file;
        }

        private static Vector2 ResolvePlayerWorldPosition(ModelWorld world)
        {
            if (world.WorldRender == null || world.WorldRender.Count == 0)
            {
                return world.SavedPlayerWorldPosition;
            }

            return world.WorldRender[new Vector2(world.Player.Coordinates.X, world.Player.Coordinates.Y)];
        }

        private static void CreateDirectoryIfNotExists(string file)
        {
            string directory = Path.GetDirectoryName(file);

            if (string.IsNullOrWhiteSpace(directory) || Directory.Exists(directory))
            {
                return;
            }

            Directory.CreateDirectory(directory);
        }

        private static void CreateFileIfNotExists(string file)
        {
            if (File.Exists(file))
            {
                return;
            }

            using FileStream f = File.Create(file);
            f.Close();
        }
    }
}