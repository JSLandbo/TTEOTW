using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;
using UtilityLibrary;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockDefinitionResolver
    {
        private const int AirBlockId = 0;
        private const int GrassBlockId = 1;
        private const int DirtBlockId = 2;
        private const int RockBlockId = 4;
        private const int LavaBlockId = 38;

        private readonly WorldElementsRepository blocks;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)>[] overlayBlockDefinitions;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)> airBlockDefinition;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)> grassBlockDefinition;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)> dirtBlockDefinition;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)> rockBlockDefinition;
        private readonly KeyValuePair<int, (string Name, Texture2D Texture, Block block)> lavaBlockDefinition;

        public WorldBlockDefinitionResolver(WorldElementsRepository blocks)
        {
            this.blocks = blocks;
            airBlockDefinition = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(AirBlockId, blocks[AirBlockId]);
            grassBlockDefinition = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(GrassBlockId, blocks[GrassBlockId]);
            dirtBlockDefinition = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(DirtBlockId, blocks[DirtBlockId]);
            rockBlockDefinition = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(RockBlockId, blocks[RockBlockId]);
            lavaBlockDefinition = new KeyValuePair<int, (string Name, Texture2D Texture, Block block)>(LavaBlockId, blocks[LavaBlockId]);

            List<KeyValuePair<int, (string Name, Texture2D Texture, Block block)>> overlayDefinitions = [];
            int index = 0;
            foreach (KeyValuePair<int, (string Name, Texture2D Texture, Block block)> block in blocks)
            {
                if (IsOverlayBlock(block.Key))
                {
                    overlayDefinitions.Add(block);
                }

                index++;
            }

            overlayBlockDefinitions = overlayDefinitions.ToArray();
            Array.Sort(overlayBlockDefinitions, (left, right) => right.Key.CompareTo(left.Key));
        }

        public KeyValuePair<int, (string Name, Texture2D Texture, Block block)> GetWorldBlock(float x, float y)
        {
            if (y <= airBlockDefinition.Value.block.Info.MaximumDepth)
            {
                return airBlockDefinition;
            }

            if (y == grassBlockDefinition.Value.block.Info.MinimumDepth)
            {
                return grassBlockDefinition;
            }

            KeyValuePair<int, (string Name, Texture2D Texture, Block block)> baseTerrain = ResolveBaseTerrain(x, y);

            foreach (KeyValuePair<int, (string Name, Texture2D Texture, Block block)> block in overlayBlockDefinitions)
            {
                if (MatchesDefinition(block, x, y))
                {
                    return block;
                }
            }

            return baseTerrain;
        }

        public bool IsObstructed(ModelWorld world, Vector2 worldPosition)
        {
            Block block = GetWorldBlock(worldPosition.X, worldPosition.Y).Value.block;

            if (block.Ethereal || world.WorldTrails.ContainsKey(worldPosition))
            {
                return false;
            }

            return true;
        }

        private KeyValuePair<int, (string Name, Texture2D Texture, Block block)> ResolveBaseTerrain(float x, float y)
        {
            float rockNoise = (float)SimplexNoise.Singleton.Noise01((x * 0.12f) + 91.0f, (y * 0.12f) + 37.0f) * 100.0f;
            float lavaNoise = (float)SimplexNoise.Singleton.Noise01((x * 0.08f) + 241.0f, (y * 0.08f) + 503.0f) * 100.0f;

            float rockChance = MathHelper.Clamp(6.0f + ((y - 12.0f) * 0.012f), 6.0f, 78.0f);
            float lavaChance = y < 2000.0f
                ? 0.0f
                : MathHelper.Clamp((y - 2000.0f) * 0.004f, 0.0f, 18.0f);

            if (y >= lavaBlockDefinition.Value.block.Info.MinimumDepth && lavaNoise <= lavaChance)
            {
                return lavaBlockDefinition;
            }

            if (rockNoise <= rockChance)
            {
                return rockBlockDefinition;
            }

            return dirtBlockDefinition;
        }

        private static bool MatchesDefinition(KeyValuePair<int, (string Name, Texture2D Texture, Block block)> block, float x, float y)
        {
            ModelLibrary.Abstract.Blocks.ABlockInfo info = block.Value.block.Info;

            if (y > info.MaximumDepth || y < info.MinimumDepth)
            {
                return false;
            }

            float simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;
            return simplex >= info.OccurrenceSpan.X && simplex <= info.OccurrenceSpan.Y;
        }

        private static bool IsOverlayBlock(int blockId)
        {
            return blockId != -2
                && blockId != -1
                && blockId != AirBlockId
                && blockId != GrassBlockId
                && blockId != DirtBlockId
                && blockId != RockBlockId
                && blockId != LavaBlockId;
        }
    }
}
