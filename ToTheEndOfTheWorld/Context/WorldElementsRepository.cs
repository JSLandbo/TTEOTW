using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Context
{
    public class WorldElementsRepository : Dictionary<int, (string Name, Texture2D Texture, int Frames, Block block)>
    {
        public WorldElementsRepository(ContentManager manager)
        {
            InitializeCollection(manager);
        }

        private void InitializeCollection(ContentManager manager)
        {
            AddRuntimeBlock(manager, GameIds.RuntimeBlocks.Breaking, "Breaking", "General/Breaking/Breaking", ethereal: true, weight: 0.0f);
            AddRuntimeBlock(manager, GameIds.RuntimeBlocks.Background, "Background", "Blocks/DirtBlockBackground", ethereal: true, weight: 0.0f);
            AddRuntimeBlock(manager, GameIds.RuntimeBlocks.Air, "Air", "Blocks/AirBlock", ethereal: true, minimumDepth: long.MinValue, maximumDepth: 10, occurrenceSpan: new Vector2(0f, 100f), weight: 0.0f);

            // Base terrain and very common early materials.
            AddTexturedBlock(manager, GameIds.Blocks.Grass, "Grass", "Blocks/GrassBlock", hardness: 0, health: 40, minimumDepth: 11, maximumDepth: 11, occurrenceSpan: new Vector2(0f, 100f), worth: 1, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Dirt, "Dirt", "Blocks/DirtBlock", hardness: 0, health: 12, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(0f, 100f), worth: 1, weight: 1.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Gravel, "Gravel", "Blocks/GravelBlock", hardness: 1, health: 12, minimumDepth: 15, maximumDepth: 220, occurrenceSpan: new Vector2(0f, 2f), worth: 2, weight: 1.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Rock, "Rock", "Blocks/StoneBlock", hardness: 250, health: 50, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(2f, 4f), worth: 2, weight: 2.3f);
            AddTexturedBlock(manager, GameIds.Blocks.Wood, "Wood", "Blocks/WoodBlock", hardness: 2, health: 18, minimumDepth: 11, maximumDepth: 80, occurrenceSpan: new Vector2(12.03f, 12.12f), worth: 2, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Coal, "Coal", "Blocks/CoalBlock", hardness: 2, health: 10, minimumDepth: 6, maximumDepth: 120000, occurrenceSpan: new Vector2(30.00f, 30.40f), worth: 2, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Water, "Water", "Blocks/WaterBlock", hardness: 1, health: 20, minimumDepth: 8, maximumDepth: 120, occurrenceSpan: new Vector2(16.35f, 16.45f), worth: 0.5f, weight: 0.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Amber, "Amber", "Blocks/AmberBlock", hardness: 3, health: 20, minimumDepth: 40, maximumDepth: 1500, occurrenceSpan: new Vector2(10.80f, 10.95f), worth: 4, weight: 0.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Tin, "Tin ore", "Blocks/TinBlock", hardness: 4, health: 35, minimumDepth: 80, maximumDepth: 3000, occurrenceSpan: new Vector2(9f, 9.5f), worth: 4, weight: 1.2f);
            AddTexturedBlock(manager, GameIds.Blocks.Granite, "Granite", "Blocks/GraniteBlock", hardness: 500, health: 140, minimumDepth: 3000, maximumDepth: 90000, occurrenceSpan: new Vector2(4f, 5f), worth: 4, weight: 2.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Apatite, "Apatite", "Blocks/ApatiteBlock", hardness: 5, health: 45, minimumDepth: 120, maximumDepth: 4000, occurrenceSpan: new Vector2(11.10f, 11.25f), worth: 5, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Calcite, "Calcite", "Blocks/CalciteBlock", hardness: 5, health: 45, minimumDepth: 160, maximumDepth: 4500, occurrenceSpan: new Vector2(11.88f, 12.03f), worth: 5, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Iron, "Iron ore", "Blocks/IronBlock", hardness: 10, health: 80, minimumDepth: 220, maximumDepth: 6500, occurrenceSpan: new Vector2(5f, 6f), worth: 5, weight: 1.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Lava, "Lava", "Blocks/LavaBlock", hardness: 20, health: 100, minimumDepth: 7000, maximumDepth: 250000, occurrenceSpan: new Vector2(13.43f, 13.53f), worth: 5, miningHeatGeneration: 20f, weight: 1.1f);
            AddTexturedBlock(manager, GameIds.Blocks.Lead, "Lead", "Blocks/LeadBlock", hardness: 4, health: 35, minimumDepth: 60, maximumDepth: 2500, occurrenceSpan: new Vector2(6f, 6.5f), worth: 6, weight: 2.3f);
            AddTexturedBlock(manager, GameIds.Blocks.Sulphur, "Sulphur", "Blocks/SulphurBlock", hardness: 7, health: 60, minimumDepth: 250, maximumDepth: 5000, occurrenceSpan: new Vector2(15.52f, 15.67f), worth: 6, weight: 0.9f);
            AddTexturedBlock(manager, GameIds.Blocks.Skeleton, "Skeleton", "Blocks/SkeletonBlock", hardness: 3, health: 25, minimumDepth: 40, maximumDepth: 6000, occurrenceSpan: new Vector2(15.45f, 15.52f), worth: 8, weight: 1.2f);
            AddTexturedBlock(manager, GameIds.Blocks.Aluminium, "Aluminium", "Blocks/AluminumBlock", hardness: 8, health: 70, minimumDepth: 320, maximumDepth: 7000, occurrenceSpan: new Vector2(6.5f, 7f), worth: 8, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Quartz, "Quartz", "Blocks/QuartzBlock", hardness: 10, health: 80, minimumDepth: 700, maximumDepth: 8000, occurrenceSpan: new Vector2(14.58f, 14.73f), worth: 9, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Zinc, "Zinc", "Blocks/ZincBlock", hardness: 10, health: 90, minimumDepth: 450, maximumDepth: 9000, occurrenceSpan: new Vector2(7f, 8f), worth: 9, weight: 1.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Copper, "Copper ore", "Blocks/CopperBlock", hardness: 5, health: 60, minimumDepth: 140, maximumDepth: 5000, occurrenceSpan: new Vector2(8f, 8.5f), worth: 10, weight: 1.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Silicon, "Silicon", "Blocks/SiliconBlock", hardness: 12, health: 90, minimumDepth: 900, maximumDepth: 10000, occurrenceSpan: new Vector2(15.30f, 15.45f), worth: 10, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Nickel, "Nickel", "Blocks/NickelBlock", hardness: 14, health: 110, minimumDepth: 900, maximumDepth: 12000, occurrenceSpan: new Vector2(8.5f, 9f), worth: 12, weight: 1.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Bismuth, "Bismuth", "Blocks/BismuthBlock", hardness: 18, health: 130, minimumDepth: 1400, maximumDepth: 15000, occurrenceSpan: new Vector2(11.66f, 11.82f), worth: 15, weight: 1.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Peridot, "Peridot", "Blocks/PeridotBlock", hardness: 16, health: 120, minimumDepth: 1100, maximumDepth: 12000, occurrenceSpan: new Vector2(14.13f, 14.28f), worth: 15, weight: 0.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Silver, "Silver ore", "Blocks/SilverBlock", hardness: 20, health: 150, minimumDepth: 1600, maximumDepth: 18000, occurrenceSpan: new Vector2(9.5f, 10f), worth: 18, weight: 1.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Malachite, "Malachite", "Blocks/MalachiteBlock", hardness: 22, health: 160, minimumDepth: 2000, maximumDepth: 20000, occurrenceSpan: new Vector2(13.53f, 13.68f), worth: 18, weight: 0.9f);
            AddTexturedBlock(manager, GameIds.Blocks.Aquamarine, "Aquamarine", "Blocks/AquamarineBlock", hardness: 24, health: 170, minimumDepth: 1800, maximumDepth: 18000, occurrenceSpan: new Vector2(11.25f, 11.40f), worth: 19, weight: 0.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Chest, "Chest", "Blocks/ChestBlock", hardness: 3, health: 25, minimumDepth: 25, maximumDepth: 5000, occurrenceSpan: new Vector2(16.45f, 16.55f), worth: 20, weight: 2.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Opal, "Opal", "Blocks/OpalBlock", hardness: 26, health: 180, minimumDepth: 2400, maximumDepth: 22000, occurrenceSpan: new Vector2(13.83f, 13.98f), worth: 20, weight: 0.5f);
            AddTexturedBlock(manager, GameIds.Blocks.Pyrite, "Pyrite", "Blocks/PyriteBlock", hardness: 18, health: 140, minimumDepth: 1300, maximumDepth: 15000, occurrenceSpan: new Vector2(14.43f, 14.58f), worth: 22, weight: 1.3f);
            AddTexturedBlock(manager, GameIds.Blocks.Citrine, "Citrine", "Blocks/CitrineBlock", hardness: 28, health: 190, minimumDepth: 2600, maximumDepth: 24000, occurrenceSpan: new Vector2(12.58f, 12.73f), worth: 22, weight: 0.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Cinnabar, "Cinnabar", "Blocks/CinnabarBlock", hardness: 30, health: 210, minimumDepth: 2800, maximumDepth: 26000, occurrenceSpan: new Vector2(12.28f, 12.43f), worth: 24, weight: 1.2f);
            AddTexturedBlock(manager, GameIds.Blocks.Garnet, "Garnet", "Blocks/GarnetBlock", hardness: 32, health: 220, minimumDepth: 3000, maximumDepth: 28000, occurrenceSpan: new Vector2(13.13f, 13.28f), worth: 26, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.LapisLazuli, "Lapis Lazuli", "Blocks/LapisLazuliBlock", hardness: 34, health: 230, minimumDepth: 3200, maximumDepth: 28000, occurrenceSpan: new Vector2(13.28f, 13.43f), worth: 28, weight: 0.8f);

            // Mid and late progression materials.
            AddTexturedBlock(manager, GameIds.Blocks.Gold, "Gold ore", "Blocks/GoldBlock", hardness: 30, health: 260, minimumDepth: 3600, maximumDepth: 32000, occurrenceSpan: new Vector2(10f, 10.25f), worth: 50, weight: 2.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Alexandrite, "Alexandrite", "Blocks/AlexandriteBlock", hardness: 36, health: 250, minimumDepth: 4000, maximumDepth: 34000, occurrenceSpan: new Vector2(10.625f, 10.80f), worth: 35, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Chromium, "Chromium", "Blocks/ChromiumBlock", hardness: 55, health: 360, minimumDepth: 5000, maximumDepth: 38000, occurrenceSpan: new Vector2(12.12f, 12.28f), worth: 35, weight: 2.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Amethyst, "Amethyst", "Blocks/AmethystBlock", hardness: 40, health: 280, minimumDepth: 4500, maximumDepth: 36000, occurrenceSpan: new Vector2(10.95f, 11.10f), worth: 38, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.DinosaurBone, "Dinosaur bone", "Blocks/DinosaurBoneBlock", hardness: 80, health: 500, minimumDepth: 6000, maximumDepth: 18000, occurrenceSpan: new Vector2(16.63f, 16.71f), worth: 40, weight: 1.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Cobalt, "Cobalt", "Blocks/CobaltBlock", hardness: 60, health: 380, minimumDepth: 5500, maximumDepth: 40000, occurrenceSpan: new Vector2(12.43f, 12.58f), worth: 42, weight: 1.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Topaz, "Topaz", "Blocks/TopazBlock", hardness: 70, health: 430, minimumDepth: 7000, maximumDepth: 45000, occurrenceSpan: new Vector2(15.82f, 15.97f), worth: 45, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Emerald, "Emerald", "Blocks/EmeraldBlock", hardness: 65, health: 400, minimumDepth: 6500, maximumDepth: 42000, occurrenceSpan: new Vector2(12.98f, 13.13f), worth: 50, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Ruby, "Ruby", "Blocks/RubyBlock", hardness: 75, health: 460, minimumDepth: 8000, maximumDepth: 48000, occurrenceSpan: new Vector2(15.00f, 15.15f), worth: 60, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Sapphire, "Sapphire", "Blocks/SapphireBlock", hardness: 78, health: 480, minimumDepth: 8500, maximumDepth: 50000, occurrenceSpan: new Vector2(15.15f, 15.30f), worth: 60, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Diode, "Diode", "Blocks/DiodeBlock", hardness: 110, health: 600, minimumDepth: 12000, maximumDepth: 60000, occurrenceSpan: new Vector2(12.88f, 12.98f), worth: 70, weight: 0.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Osmium, "Osmium", "Blocks/OsmiumBlock", hardness: 180, health: 1200, minimumDepth: 18000, maximumDepth: 70000, occurrenceSpan: new Vector2(13.98f, 14.13f), worth: 70, weight: 2.8f);
            AddTexturedBlock(manager, GameIds.Blocks.Tungsten, "Tungsten", "Blocks/TungstenBlock", hardness: 360, health: 2400, minimumDepth: 34000, maximumDepth: 90000, occurrenceSpan: new Vector2(16.05f, 16.20f), worth: 80, weight: 3.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Platinum, "Platinum", "Blocks/PlatinumBlock", hardness: 280, health: 1800, minimumDepth: 26000, maximumDepth: 75000, occurrenceSpan: new Vector2(14.28f, 14.43f), worth: 90, weight: 2.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Diamond, "Diamond", "Blocks/DiamondBlock", hardness: 170, health: 1100, minimumDepth: 14000, maximumDepth: 65000, occurrenceSpan: new Vector2(12.73f, 12.88f), worth: 100, weight: 0.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Titanium, "Titanium", "Blocks/TitaniumBlock", hardness: 320, health: 2100, minimumDepth: 30000, maximumDepth: 80000, occurrenceSpan: new Vector2(15.67f, 15.82f), worth: 110, weight: 2.6f);
            AddTexturedBlock(manager, GameIds.Blocks.Obsidian, "Obsidian", "Blocks/ObsidianBlock", hardness: 220, health: 1500, minimumDepth: 20000, maximumDepth: 70000, occurrenceSpan: new Vector2(13.68f, 13.83f), worth: 120, weight: 2.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Rainbow, "Rainbow", "Blocks/RainbowBlock", hardness: 850, health: 5000, minimumDepth: 52000, maximumDepth: 120000, occurrenceSpan: new Vector2(16.55f, 16.63f), worth: 125, weight: 0.7f);
            AddTexturedBlock(manager, GameIds.Blocks.Ring, "Ring", "Blocks/RingBlock", hardness: 260, health: 1700, minimumDepth: 24000, maximumDepth: 90000, occurrenceSpan: new Vector2(14.93f, 15.00f), worth: 150, weight: 0.3f);
            AddTexturedBlock(manager, GameIds.Blocks.Uranium, "Uranium", "Blocks/UraniumBlock", hardness: 380, health: 2800, minimumDepth: 38000, maximumDepth: 100000, occurrenceSpan: new Vector2(16.20f, 16.35f), worth: 175, weight: 2.2f);
            AddTexturedBlock(manager, GameIds.Blocks.Mythril, "Mythril", "Blocks/MythrilBlock", hardness: 1800, health: 9000, minimumDepth: 60000, maximumDepth: 130000, occurrenceSpan: new Vector2(10.25f, 10.50f), worth: 250, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Adamantium, "Adamantium", "Blocks/AdamantiumBlock", hardness: 3500, health: 15000, minimumDepth: 76000, maximumDepth: 150000, occurrenceSpan: new Vector2(10.50f, 10.625f), worth: 500, weight: 1.3f);
            AddTexturedBlock(manager, GameIds.Blocks.Treasure, "Treasure", "Blocks/TreasureBlock", hardness: 950, health: 5500, minimumDepth: 45000, maximumDepth: 160000, occurrenceSpan: new Vector2(15.97f, 16.05f), worth: 1200, weight: 1.5f);
            AddTexturedBlock(manager, GameIds.Blocks.RainbowGem, "Rainbow gem", "Blocks/RainbowGemBlock", hardness: 6500, health: 28000, minimumDepth: 110000, maximumDepth: 400000, occurrenceSpan: new Vector2(14.83f, 14.93f), worth: 2500, weight: 0.4f);
            AddTexturedBlock(manager, GameIds.Blocks.Artifact, "Artifact", "Blocks/ArtifactBlock", hardness: 4200, health: 18000, minimumDepth: 80000, maximumDepth: 300000, occurrenceSpan: new Vector2(11.40f, 11.48f), worth: 6000, weight: 0.8f);
            AddTexturedBlock(manager, GameIds.Blocks.WingOfDeath, "Wing of death", "Blocks/WingOfDeathBlock", hardness: 8000, health: 32000, minimumDepth: 125000, maximumDepth: 500000, occurrenceSpan: new Vector2(16.93f, 17.00f), worth: 8000, weight: 0.5f);
            AddTexturedBlock(manager, GameIds.Blocks.WingOfLife, "Wing of life", "Blocks/WingOfLifeBlock", hardness: 8000, health: 32000, minimumDepth: 125000, maximumDepth: 500000, occurrenceSpan: new Vector2(17.00f, 17.07f), worth: 8000, weight: 0.5f);
            AddTexturedBlock(manager, GameIds.Blocks.Artifacts, "Artifacts", "Blocks/ArtifactsBlock", hardness: 5200, health: 22000, minimumDepth: 95000, maximumDepth: 350000, occurrenceSpan: new Vector2(11.48f, 11.56f), worth: 10000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.ShardOfDeath, "Shard of death", "Blocks/ShardOfDeathBlock", hardness: 9000, health: 36000, minimumDepth: 140000, maximumDepth: 600000, occurrenceSpan: new Vector2(16.71f, 16.79f), worth: 10000, weight: 0.5f);
            AddTexturedBlock(manager, GameIds.Blocks.ShardOfLife, "Shard of life", "Blocks/ShardOfLifeBlock", hardness: 9000, health: 36000, minimumDepth: 140000, maximumDepth: 600000, occurrenceSpan: new Vector2(16.79f, 16.87f), worth: 10000, weight: 0.5f);
            AddTexturedBlock(manager, GameIds.Blocks.BlackHole, "Black hole", "Blocks/BlackHoleBlock", hardness: 50000f, health: 180000f, minimumDepth: 300000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.82f, 11.88f), worth: 25000, weight: 4.0f);
            AddTexturedBlock(manager, GameIds.Blocks.SuperNova, "Super nova", "Blocks/SuperNovaBlock", hardness: 100000f, health: 250000f, minimumDepth: 500000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.87f, 16.93f), worth: 50000, weight: 3.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Bedrock, "Bedrock", "Blocks/BedrockBlock", hardness: 25000f, health: 120000f, minimumDepth: 200000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.56f, 11.66f), worth: 0, weight: 5.0f);

            // Utility and ethereal overlay.
            AddTexturedBlock(manager, GameIds.Blocks.Hole, "Hole", "Blocks/DirtBlockBackground", hardness: 0, health: 0, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(17.07f, 30.00f), worth: 0, ethereal: true, weight: 0.0f);
        }

        private void AddRuntimeBlock(ContentManager manager, short id, string name, string assetPath, bool ethereal, long minimumDepth = long.MinValue, long maximumDepth = long.MaxValue, Vector2? occurrenceSpan = null, float weight = 1.0f, int frames = 1)
        {
            BlockInfo info = new(
                MinimumDepth: minimumDepth,
                MaximumDepth: maximumDepth,
                OccurrenceSpan: occurrenceSpan ?? new Vector2(),
                Weight: weight);

            Block block = new(ID: id, Ethereal: ethereal, Info: info);
            Add(id, (name, manager.Load<Texture2D>(assetPath), frames, block));
        }

        private void AddTexturedBlock(ContentManager manager, short id, string name, string assetPath, float hardness, float health, long minimumDepth, long maximumDepth, Vector2 occurrenceSpan, float worth = 0, bool ethereal = false, float miningHeatGeneration = 0.1f, float weight = 1.0f, int frames = 1)
        {
            BlockInfo info = new(
                MinimumDepth: minimumDepth,
                MaximumDepth: maximumDepth,
                OccurrenceSpan: occurrenceSpan,
                Weight: weight,
                MiningHeatGeneration: miningHeatGeneration);

            Block block = new(ID: id, Ethereal: ethereal, Hardness: hardness, Health: health, Worth: worth, Info: info);
            Add(id, (name, manager.Load<Texture2D>(assetPath), frames, block));
        }
    }
}
