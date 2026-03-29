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

        public KeyValuePair<int, (string Name, Texture2D Texture, Block Block)> GetWorldBlock(float x, float y)
        {
            var simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;

            foreach (var block in blocks.OrderByDescending(e => e.Key))
            {
                var info = block.Value.block.Info;

                if (y > info.MaximumDepth || y < info.MinimumDepth)
                {
                    continue;
                }

                if (simplex >= info.OccurrenceSpan.X && simplex <= info.OccurrenceSpan.Y)
                {
                    var keyValuePair = new KeyValuePair<int, (string Name, Texture2D Texture, Block Block)>
                    (
                        block.Key, (block.Value.Name, block.Value.Texture, new Block(block.Value.block))
                    );

                    if (block.Key == 2 && x > 0)
                    {
                        keyValuePair.Value.Block.CurrentHealth += 0.01f * x;
                        keyValuePair.Value.Block.MaximumHealth += 0.01f * x;
                    }

                    return keyValuePair;
                }
            }

            return new KeyValuePair<int, (string Name, Texture2D Texture, Block Block)>(-1, (null, null, null));
        }

        public bool IsObstructed(World world, Vector2 worldPosition)
        {
            var block = GetWorldBlock(worldPosition.X, worldPosition.Y).Value.Block;

            if (block.Ethereal || world.WorldTrails.ContainsKey(worldPosition))
            {
                return false;
            }

            return true;
        }
    }
}
