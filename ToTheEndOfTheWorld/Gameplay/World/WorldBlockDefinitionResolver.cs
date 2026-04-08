using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Blocks;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Ids;
using UtilityLibrary;
using BlockDefinition = System.Collections.Generic.KeyValuePair<int, (string Name, Microsoft.Xna.Framework.Graphics.Texture2D Texture, int Frames, ModelLibrary.Concrete.Blocks.Block block)>;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockDefinitionResolver
    {
        private readonly Dictionary<int, BlockDefinition> definitions;
        private readonly BlockDefinition[] overlayBlockDefinitions;

        public WorldBlockDefinitionResolver(WorldElementsRepository blocks)
        {
            definitions = [];
            List<BlockDefinition> overlays = [];

            foreach (BlockDefinition block in blocks)
            {
                definitions[block.Key] = block;

                if (IsOverlayBlock(block.Key))
                {
                    overlays.Add(block);
                }
            }

            overlayBlockDefinitions = [.. overlays];
            Array.Sort(overlayBlockDefinitions, static (left, right) => right.Key.CompareTo(left.Key));
        }

        public BlockDefinition GetWorldBlock(float x, float y)
        {
            BlockDefinition air = GetDefinition(GameIds.RuntimeBlocks.Air);
            if (y <= air.Value.block.Info.MaximumDepth)
            {
                return air;
            }

            BlockDefinition grass = GetDefinition(GameIds.Blocks.Grass);
            if (y == grass.Value.block.Info.MinimumDepth)
            {
                return grass;
            }

            foreach (BlockDefinition block in overlayBlockDefinitions)
            {
                if (MatchesDefinition(block, x, y))
                {
                    return block;
                }
            }

            return ResolveBaseTerrain(x, y);
        }

        private BlockDefinition ResolveBaseTerrain(float x, float y)
        {
            float waterNoise = (float)SimplexNoise.Singleton.Noise01((x * 0.08f) + 173.0f, (y * 0.08f) + 421.0f) * 100.0f;


            BlockDefinition water = GetDefinition(GameIds.Blocks.Water);
            if (IsWithinDepth(y, water) && waterNoise <= 5.0f)
            {
                return water;
            }

            float lavaNoise = (float)SimplexNoise.Singleton.Noise01((x * 0.08f) + 241.0f, (y * 0.08f) + 503.0f) * 100.0f;
            float lavaChance = y < 5000.0f ? 0.0f : MathHelper.Clamp((y - 5000.0f) * 0.0025f, 0.0f, 12.0f);

            BlockDefinition lava = GetDefinition(GameIds.Blocks.Lava);
            if (IsWithinDepth(y, lava) && lavaNoise <= lavaChance)
            {
                return lava;
            }

            float rockNoise = (float)SimplexNoise.Singleton.Noise01((x * 0.12f) + 91.0f, (y * 0.12f) + 37.0f) * 100.0f;
            float rockChance = MathHelper.Clamp(6.0f + ((y - 12.0f) * 0.012f), 6.0f, 78.0f);

            BlockDefinition rock = GetDefinition(GameIds.Blocks.Rock);
            if (IsWithinDepth(y, rock) && rockNoise <= rockChance)
            {
                return GetDefinition(GameIds.Blocks.Rock);
            }

            return GetDefinition(GameIds.Blocks.Dirt);
        }

        public bool IsObstructed(ModelWorld world, Vector2 worldPosition)
        {
            Block block = GetWorldBlock(worldPosition.X, worldPosition.Y).Value.block;
            return !block.Ethereal && !world.WorldTrails.ContainsKey(worldPosition);
        }


        private BlockDefinition GetDefinition(int blockId)
        {
            return definitions[blockId];
        }

        private static bool IsWithinDepth(float y, BlockDefinition block)
        {
            return y >= block.Value.block.Info.MinimumDepth && y <= block.Value.block.Info.MaximumDepth;
        }

        private static bool MatchesDefinition(BlockDefinition block, float x, float y)
        {
            ABlockInfo info = block.Value.block.Info;

            if (y < info.MinimumDepth || y > info.MaximumDepth)
            {
                return false;
            }

            float simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;
            return simplex >= info.OccurrenceSpan.X && simplex <= info.OccurrenceSpan.Y;
        }

        private static bool IsOverlayBlock(int blockId)
        {
            return blockId != GameIds.RuntimeBlocks.Breaking
                && blockId != GameIds.RuntimeBlocks.Background
                && blockId != GameIds.RuntimeBlocks.Air
                && blockId != GameIds.Blocks.Water
                && blockId != GameIds.Blocks.Grass
                && blockId != GameIds.Blocks.Dirt
                && blockId != GameIds.Blocks.Rock
                && blockId != GameIds.Blocks.Lava;
        }
    }
}
