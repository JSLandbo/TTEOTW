using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Blocks;
using System;
using System.Collections.Generic;
using ToTheEndOfTheWorld.Context;
using UtilityLibrary;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldBlockDefinitionResolver
    {
        private readonly WorldElementsRepository blocks;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)>[] orderedBlockDefinitions;

        public WorldBlockDefinitionResolver(WorldElementsRepository blocks)
        {
            this.blocks = blocks;
            orderedBlockDefinitions = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>[blocks.Count];

            var index = 0;
            foreach (var block in blocks)
            {
                orderedBlockDefinitions[index++] = block;
            }

            Array.Sort(orderedBlockDefinitions, (left, right) => right.Key.CompareTo(left.Key));
        }

        public KeyValuePair<int, (string Name, Texture2D Texture, Block block)> GetWorldBlock(float x, float y)
        {
            var simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;

            foreach (var block in orderedBlockDefinitions)
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

        public bool IsObstructed(World world, Vector2 worldPosition)
        {
            var block = GetWorldBlock(worldPosition.X, worldPosition.Y).Value.block;

            if (block.Ethereal || world.WorldTrails.ContainsKey(worldPosition))
            {
                return false;
            }

            return true;
        }
    }
}
