using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Blocks;
using System.Collections.Generic;
using System.Linq;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using UtilityLibrary;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldQueryService
    {
        private readonly WorldElementsRepository blocks;

        public WorldQueryService(WorldElementsRepository blocks)
        {
            this.blocks = blocks;
        }

        public KeyValuePair<int, (string Name, Texture2D Texture, Block block)> GetWorldBlock(float x, float y)
        {
            return FindBlockDefinition(x, y);
        }

        public Block CreateMutableWorldBlock(float x, float y)
        {
            var definition = FindBlockDefinition(x, y);
            var block = new Block(definition.Value.block);

            if (definition.Key == 2 && x > 0)
            {
                block.CurrentHealth += 0.01f * x;
                block.MaximumHealth += 0.01f * x;
            }

            return block;
        }

        public bool IsObstructed(World world, Vector2 worldPosition)
        {
            var block = GetWorldBlock(worldPosition.X, worldPosition.Y).Value.block;

            if (block.Ethereal || world.WorldTrails.ContainsKey(worldPosition))
            {
                return false;
            }

            return true;
        }

        private KeyValuePair<int, (string Name, Texture2D Texture, Block block)> FindBlockDefinition(float x, float y)
        {
            var simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;

            foreach (var block in blocks.OrderByDescending(entry => entry.Key))
            {
                var info = block.Value.block.Info;

                if (y > info.MaximumDepth || y < info.MinimumDepth)
                {
                    continue;
                }

                if (simplex >= info.OccurrenceSpan.X && simplex <= info.OccurrenceSpan.Y)
                {
                    return block;
                }
            }

            return new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(-1, blocks[-1]);
        }
    }
}
