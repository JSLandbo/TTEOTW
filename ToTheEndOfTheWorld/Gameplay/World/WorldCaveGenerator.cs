using System;
using System.Collections.Generic;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldCaveGenerator
    {
        private const int CaveRegionSize = 28;
        private const int MaxCachedRegions = 256;
        private static readonly int[] EarlyCaveOreIds = [GameIds.Blocks.Coal, GameIds.Blocks.Tin, GameIds.Blocks.Copper, GameIds.Blocks.Iron];
        private static readonly int[] EarlyMidCaveOreIds = [GameIds.Blocks.Iron, GameIds.Blocks.Zinc, GameIds.Blocks.Nickel, GameIds.Blocks.Silver];
        private static readonly int[] MidCaveOreIds = [GameIds.Blocks.Gold, GameIds.Blocks.Alexandrite, GameIds.Blocks.Amethyst, GameIds.Blocks.Cobalt, GameIds.Blocks.Chromium];
        private static readonly int[] MidDeepCaveOreIds = [GameIds.Blocks.Amethyst, GameIds.Blocks.Cobalt, GameIds.Blocks.Topaz, GameIds.Blocks.Emerald, GameIds.Blocks.Ruby, GameIds.Blocks.Sapphire];
        private static readonly int[] DeepCaveOreIds = [GameIds.Blocks.Diamond, GameIds.Blocks.Diode, GameIds.Blocks.Obsidian, GameIds.Blocks.Osmium, GameIds.Blocks.Platinum];
        private static readonly int[] DeeperCaveOreIds = [GameIds.Blocks.Platinum, GameIds.Blocks.Titanium, GameIds.Blocks.Tungsten, GameIds.Blocks.Ring, GameIds.Blocks.Uranium];
        private static readonly int[] LateCaveOreIds = [GameIds.Blocks.Uranium, GameIds.Blocks.Treasure, GameIds.Blocks.Rainbow, GameIds.Blocks.Titanium, GameIds.Blocks.Tungsten];
        private static readonly int[] EndgameCaveOreIds = [GameIds.Blocks.Mythril, GameIds.Blocks.Adamantium, GameIds.Blocks.Artifact, GameIds.Blocks.Artifacts, GameIds.Blocks.Treasure];
        private readonly Dictionary<(long X, long Y), CavePocket> pocketCache = [];

        public bool TryResolveBlockId(float x, float y, out int blockId)
        {
            blockId = default;

            long tileX = (long)MathF.Floor(x);
            long tileY = (long)MathF.Floor(y);
            if (tileY < 80)
            {
                return false;
            }

            long regionX = FloorDiv(tileX, CaveRegionSize);
            long regionY = FloorDiv(tileY, CaveRegionSize);
            CavePocket pocket = GetOrCreatePocket(regionX, regionY);
            if (!pocket.Exists)
            {
                return false;
            }

            float offsetX = x - pocket.CenterX;
            float offsetY = y - pocket.CenterY;
            if (MathF.Abs(offsetX) > pocket.MaxRangeX || MathF.Abs(offsetY) > pocket.MaxRangeY)
            {
                return false;
            }

            for (int i = 0; i < pocket.OreNodes.Length; i++)
            {
                OreNode oreNode = pocket.OreNodes[i];
                float oreDx = x - oreNode.X;
                float oreDy = y - oreNode.Y;

                if ((oreDx * oreDx) + (oreDy * oreDy) <= oreNode.RadiusSquared)
                {
                    blockId = oreNode.BlockId;
                    return true;
                }
            }

            if (MathF.Abs(offsetX) > pocket.RadiusX || MathF.Abs(offsetY) > pocket.RadiusY)
            {
                return false;
            }

            float ellipse = ((offsetX * offsetX) / pocket.RadiusXSquared) + ((offsetY * offsetY) / pocket.RadiusYSquared);
            if (ellipse > 1.0f)
            {
                return false;
            }

            blockId = GameIds.Blocks.Hole;
            return true;
        }

        private CavePocket GetOrCreatePocket(long regionX, long regionY)
        {
            (long X, long Y) key = (regionX, regionY);
            if (pocketCache.TryGetValue(key, out CavePocket pocket))
            {
                return pocket;
            }

            pocket = CreatePocket(regionX, regionY);
            if (pocketCache.Count >= MaxCachedRegions)
            {
                pocketCache.Clear();
            }

            pocketCache[key] = pocket;
            return pocket;
        }

        private static CavePocket CreatePocket(long regionX, long regionY)
        {
            uint caveSeed = Hash(regionX, regionY, 77123);
            if ((caveSeed % 100u) >= 4u)
            {
                return CavePocket.None;
            }

            float centerX = (regionX * CaveRegionSize) + 7 + (float)(caveSeed % 14u);
            float centerY = (regionY * CaveRegionSize) + 7 + (float)((caveSeed / 17u) % 14u);
            float radiusX = 3.2f + (float)((caveSeed / 31u) % 3u);
            float radiusY = 3.0f + (float)((caveSeed / 59u) % 3u);
            int[] caveOreIds = GetCaveOreIds((regionY * CaveRegionSize) + (CaveRegionSize / 2));
            int caveOreCount = 8 + (int)((caveSeed / 97u) % 9u);
            OreNode[] oreNodes = new OreNode[caveOreCount];
            float maxRangeX = radiusX;
            float maxRangeY = radiusY;

            for (int i = 0; i < caveOreCount; i++)
            {
                uint oreSeed = Hash(regionX, regionY, 88001 + i);
                float angle = (((oreSeed % 10000u) / 9999.0f) * MathF.PI * 2.0f);
                float edgeScale = 1.03f + (float)(((oreSeed / 10000u) % 100u) / 99.0f) * 0.22f;
                float oreX = centerX + (MathF.Cos(angle) * radiusX * edgeScale);
                float oreY = centerY + (MathF.Sin(angle) * radiusY * edgeScale);
                float oreRadius = 0.52f + (float)((oreSeed / 37u) % 2u) * 0.12f;
                oreNodes[i] = new OreNode(
                    caveOreIds[(int)((oreSeed / 13u) % (uint)caveOreIds.Length)],
                    oreX,
                    oreY,
                    oreRadius * oreRadius);

                maxRangeX = MathF.Max(maxRangeX, MathF.Abs(oreX - centerX) + oreRadius);
                maxRangeY = MathF.Max(maxRangeY, MathF.Abs(oreY - centerY) + oreRadius);
            }

            return new CavePocket(centerX, centerY, radiusX, radiusY, maxRangeX, maxRangeY, oreNodes);
        }

        private static int[] GetCaveOreIds(long depth)
        {
            if (depth < 400)
            {
                return EarlyCaveOreIds;
            }

            if (depth < 1500)
            {
                return EarlyMidCaveOreIds;
            }

            if (depth < 5000)
            {
                return MidCaveOreIds;
            }

            if (depth < 12000)
            {
                return MidDeepCaveOreIds;
            }

            if (depth < 22000)
            {
                return DeepCaveOreIds;
            }

            if (depth < 42000)
            {
                return DeeperCaveOreIds;
            }

            if (depth < 80000)
            {
                return LateCaveOreIds;
            }

            return EndgameCaveOreIds;
        }

        private static long FloorDiv(long value, int divisor)
        {
            long quotient = value / divisor;
            long remainder = value % divisor;
            return remainder < 0 ? quotient - 1 : quotient;
        }

        private static uint Hash(long x, long y, int salt)
        {
            unchecked
            {
                ulong value = (ulong)(x * 73856093L) ^ (ulong)(y * 19349663L) ^ (uint)salt;
                value ^= value >> 33;
                value *= 0xff51afd7ed558ccdUL;
                value ^= value >> 33;
                value *= 0xc4ceb9fe1a85ec53UL;
                value ^= value >> 33;
                return (uint)value;
            }
        }

        private sealed class CavePocket
        {
            public static readonly CavePocket None = new(0f, 0f, 0f, 0f, 0f, 0f, []);

            public CavePocket(float centerX, float centerY, float radiusX, float radiusY, float maxRangeX, float maxRangeY, OreNode[] oreNodes)
            {
                Exists = oreNodes.Length > 0;
                CenterX = centerX;
                CenterY = centerY;
                RadiusX = radiusX;
                RadiusY = radiusY;
                RadiusXSquared = radiusX * radiusX;
                RadiusYSquared = radiusY * radiusY;
                MaxRangeX = maxRangeX;
                MaxRangeY = maxRangeY;
                OreNodes = oreNodes;
            }

            public bool Exists { get; }
            public float CenterX { get; }
            public float CenterY { get; }
            public float RadiusX { get; }
            public float RadiusY { get; }
            public float RadiusXSquared { get; }
            public float RadiusYSquared { get; }
            public float MaxRangeX { get; }
            public float MaxRangeY { get; }
            public OreNode[] OreNodes { get; }
        }

        private readonly struct OreNode(int blockId, float x, float y, float radiusSquared)
        {
            public int BlockId { get; } = blockId;
            public float X { get; } = x;
            public float Y { get; } = y;
            public float RadiusSquared { get; } = radiusSquared;
        }
    }
}
