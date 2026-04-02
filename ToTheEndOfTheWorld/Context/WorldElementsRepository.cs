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

            AddTexturedBlock(manager, GameIds.Blocks.Grass, "Grass", "Blocks/GrassBlock", hardness: 0, health: 50, minimumDepth: 11, maximumDepth: 11, occurrenceSpan: new Vector2(0f, 100f), worth: 5, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Dirt, "Dirt", "Blocks/DirtBlock", hardness: 0, health: 10, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(0f, 100f), worth: 1, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Gravel, "Gravel", "Blocks/GravelBlock", hardness: 1, health: 5, minimumDepth: 15, maximumDepth: 220, occurrenceSpan: new Vector2(0f, 2f), worth: 2, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Rock, "Rock", "Blocks/StoneBlock", hardness: 20, health: 50, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(2f, 4f), worth: 5, weight: 2.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Granite, "Granite", "Blocks/GraniteBlock", hardness: 50, health: 100, minimumDepth: 8000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(4f, 5f), worth: 8, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Iron, "Iron ore", "Blocks/IronBlock", hardness: 15, health: 75, minimumDepth: 100, maximumDepth: 800000, occurrenceSpan: new Vector2(5f, 6f), worth: 10, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Lead, "Lead", "Blocks/LeadBlock", hardness: 5, health: 50, minimumDepth: 200, maximumDepth: 40000, occurrenceSpan: new Vector2(6f, 6.5f), worth: 12, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Aluminium, "Aluminium", "Blocks/AluminumBlock", hardness: 15, health: 100, minimumDepth: 300, maximumDepth: 60000, occurrenceSpan: new Vector2(6.5f, 7f), worth: 15, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Zinc, "Zinc", "Blocks/ZincBlock", hardness: 20, health: 150, minimumDepth: 500, maximumDepth: 80000, occurrenceSpan: new Vector2(7f, 8f), worth: 18, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Copper, "Copper ore", "Blocks/CopperBlock", hardness: 25, health: 200, minimumDepth: 800, maximumDepth: 100000, occurrenceSpan: new Vector2(8f, 8.5f), worth: 20, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Nickel, "Nickel", "Blocks/NickelBlock", hardness: 35, health: 300, minimumDepth: 1200, maximumDepth: 140000, occurrenceSpan: new Vector2(8.5f, 9f), worth: 25, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Tin, "Tin ore", "Blocks/TinBlock", hardness: 10, health: 50, minimumDepth: 400, maximumDepth: 20000, occurrenceSpan: new Vector2(9f, 9.5f), worth: 8, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Silver, "Silver ore", "Blocks/SilverBlock", hardness: 20, health: 85, minimumDepth: 1600, maximumDepth: 200000, occurrenceSpan: new Vector2(9.5f, 10f), worth: 35, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Gold, "Gold ore", "Blocks/GoldBlock", hardness: 20, health: 250, minimumDepth: 2400, maximumDepth: 400000, occurrenceSpan: new Vector2(10f, 10.25f), worth: 100, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Mythril, "Mythril", "Blocks/MythrilBlock", hardness: 500, health: 2500, minimumDepth: 6000, maximumDepth: 800000, occurrenceSpan: new Vector2(10.25f, 10.50f), worth: 250, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Adamantium, "Adamantium", "Blocks/AdamantiumBlock", hardness: 1500, health: 8500, minimumDepth: 12000, maximumDepth: 1600000, occurrenceSpan: new Vector2(10.50f, 10.625f), worth: 1000, weight: 1.0f);

            AddTexturedBlock(manager, GameIds.Blocks.Alexandrite, "Alexandrite", "Blocks/AlexandriteBlock", hardness: 70, health: 300, minimumDepth: 2600, maximumDepth: 500000, occurrenceSpan: new Vector2(10.625f, 10.80f), worth: 80, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Amber, "Amber", "Blocks/AmberBlock", hardness: 12, health: 30, minimumDepth: 40, maximumDepth: 2400, occurrenceSpan: new Vector2(10.80f, 10.95f), worth: 6, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Amethyst, "Amethyst", "Blocks/AmethystBlock", hardness: 80, health: 350, minimumDepth: 3000, maximumDepth: 600000, occurrenceSpan: new Vector2(10.95f, 11.10f), worth: 90, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Apatite, "Apatite", "Blocks/ApatiteBlock", hardness: 18, health: 60, minimumDepth: 200, maximumDepth: 12000, occurrenceSpan: new Vector2(11.10f, 11.25f), worth: 10, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Aquamarine, "Aquamarine", "Blocks/AquamarineBlock", hardness: 45, health: 180, minimumDepth: 1800, maximumDepth: 180000, occurrenceSpan: new Vector2(11.25f, 11.40f), worth: 45, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Artifact, "Artifact", "Blocks/ArtifactBlock", hardness: 1200, health: 6000, minimumDepth: 9000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.40f, 11.48f), worth: 30000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Artifacts, "Artifacts", "Blocks/ArtifactsBlock", hardness: 1300, health: 7000, minimumDepth: 12000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.48f, 11.56f), worth: 60000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Bedrock, "Bedrock", "Blocks/BedrockBlock", hardness: 100000f, health: 100000f, minimumDepth: 20000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.56f, 11.66f), worth: 1600, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Bismuth, "Bismuth", "Blocks/BismuthBlock", hardness: 45, health: 240, minimumDepth: 1500, maximumDepth: 150000, occurrenceSpan: new Vector2(11.66f, 11.82f), worth: 30, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.BlackHole, "Black hole", "Blocks/BlackHoleBlock", hardness: 50000f, health: 50000f, minimumDepth: 16000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.82f, 11.88f), worth: 50000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Calcite, "Calcite", "Blocks/CalciteBlock", hardness: 16, health: 55, minimumDepth: 300, maximumDepth: 14000, occurrenceSpan: new Vector2(11.88f, 12.03f), worth: 9, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Chest, "Chest", "Blocks/ChestBlock", hardness: 5, health: 25, minimumDepth: 25, maximumDepth: 4000, occurrenceSpan: new Vector2(12.03f, 12.12f), worth: 120, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Chromium, "Chromium", "Blocks/ChromiumBlock", hardness: 75, health: 320, minimumDepth: 2800, maximumDepth: 280000, occurrenceSpan: new Vector2(12.12f, 12.28f), worth: 70, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Cinnabar, "Cinnabar", "Blocks/CinnabarBlock", hardness: 60, health: 260, minimumDepth: 2200, maximumDepth: 220000, occurrenceSpan: new Vector2(12.28f, 12.43f), worth: 55, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Cobalt, "Cobalt", "Blocks/CobaltBlock", hardness: 85, health: 350, minimumDepth: 3200, maximumDepth: 300000, occurrenceSpan: new Vector2(12.43f, 12.58f), worth: 85, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Citrine, "Citrine", "Blocks/CitrineBlock", hardness: 55, health: 220, minimumDepth: 1900, maximumDepth: 180000, occurrenceSpan: new Vector2(12.58f, 12.73f), worth: 50, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Diamond, "Diamond", "Blocks/DiamondBlock", hardness: 400, health: 1200, minimumDepth: 4400, maximumDepth: 900000, occurrenceSpan: new Vector2(12.73f, 12.88f), worth: 200, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Diode, "Diode", "Blocks/DiodeBlock", hardness: 140, health: 420, minimumDepth: 3800, maximumDepth: 350000, occurrenceSpan: new Vector2(12.88f, 12.98f), worth: 110, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Emerald, "Emerald", "Blocks/EmeraldBlock", hardness: 95, health: 360, minimumDepth: 3400, maximumDepth: 320000, occurrenceSpan: new Vector2(12.98f, 13.13f), worth: 110, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Garnet, "Garnet", "Blocks/GarnetBlock", hardness: 70, health: 260, minimumDepth: 2400, maximumDepth: 220000, occurrenceSpan: new Vector2(13.13f, 13.28f), worth: 60, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.LapisLazuli, "Lapis Lazuli", "Blocks/LapisLazuliBlock", hardness: 65, health: 240, minimumDepth: 2100, maximumDepth: 210000, occurrenceSpan: new Vector2(13.28f, 13.43f), worth: 55, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Lava, "Lava", "Blocks/LavaBlock", hardness: 40, health: 100, minimumDepth: 4500, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(13.43f, 13.53f), worth: 15, miningHeatGeneration: 20f, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Malachite, "Malachite", "Blocks/MalachiteBlock", hardness: 50, health: 210, minimumDepth: 1700, maximumDepth: 170000, occurrenceSpan: new Vector2(13.53f, 13.68f), worth: 40, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Obsidian, "Obsidian", "Blocks/ObsidianBlock", hardness: 650, health: 2600, minimumDepth: 5200, maximumDepth: 1000000, occurrenceSpan: new Vector2(13.68f, 13.83f), worth: 320, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Opal, "Opal", "Blocks/OpalBlock", hardness: 58, health: 210, minimumDepth: 2100, maximumDepth: 180000, occurrenceSpan: new Vector2(13.83f, 13.98f), worth: 48, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Osmium, "Osmium", "Blocks/OsmiumBlock", hardness: 160, health: 600, minimumDepth: 4800, maximumDepth: 500000, occurrenceSpan: new Vector2(13.98f, 14.13f), worth: 140, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Peridot, "Peridot", "Blocks/PeridotBlock", hardness: 48, health: 180, minimumDepth: 1600, maximumDepth: 140000, occurrenceSpan: new Vector2(14.13f, 14.28f), worth: 36, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Platinum, "Platinum", "Blocks/PlatinumBlock", hardness: 200, health: 700, minimumDepth: 5000, maximumDepth: 600000, occurrenceSpan: new Vector2(14.28f, 14.43f), worth: 180, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Pyrite, "Pyrite", "Blocks/PyriteBlock", hardness: 52, health: 200, minimumDepth: 1900, maximumDepth: 160000, occurrenceSpan: new Vector2(14.43f, 14.58f), worth: 26, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Quartz, "Quartz", "Blocks/QuartzBlock", hardness: 42, health: 160, minimumDepth: 1200, maximumDepth: 120000, occurrenceSpan: new Vector2(14.58f, 14.73f), worth: 22, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.RainbowGem, "Rainbow gem", "Blocks/RainbowGemBlock", hardness: 2200, health: 12000, minimumDepth: 18000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(14.83f, 14.93f), worth: 7000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Ring, "Ring", "Blocks/RingBlock", hardness: 200, health: 500, minimumDepth: 6000, maximumDepth: 700000, occurrenceSpan: new Vector2(14.93f, 15.00f), worth: 500, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Ruby, "Ruby", "Blocks/RubyBlock", hardness: 100, health: 380, minimumDepth: 3600, maximumDepth: 360000, occurrenceSpan: new Vector2(15.00f, 15.15f), worth: 120, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Sapphire, "Sapphire", "Blocks/SapphireBlock", hardness: 105, health: 390, minimumDepth: 3700, maximumDepth: 370000, occurrenceSpan: new Vector2(15.15f, 15.30f), worth: 120, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Silicon, "Silicon", "Blocks/SiliconBlock", hardness: 45, health: 170, minimumDepth: 1400, maximumDepth: 120000, occurrenceSpan: new Vector2(15.30f, 15.45f), worth: 20, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Skeleton, "Skeleton", "Blocks/SkeletonBlock", hardness: 8, health: 25, minimumDepth: 40, maximumDepth: 7000, occurrenceSpan: new Vector2(15.45f, 15.52f), worth: 35, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Sulphur, "Sulphur", "Blocks/SulphurBlock", hardness: 25, health: 90, minimumDepth: 500, maximumDepth: 30000, occurrenceSpan: new Vector2(15.52f, 15.67f), worth: 16, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Titanium, "Titanium", "Blocks/TitaniumBlock", hardness: 240, health: 900, minimumDepth: 6200, maximumDepth: 700000, occurrenceSpan: new Vector2(15.67f, 15.82f), worth: 220, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Topaz, "Topaz", "Blocks/TopazBlock", hardness: 82, health: 330, minimumDepth: 3000, maximumDepth: 300000, occurrenceSpan: new Vector2(15.82f, 15.97f), worth: 85, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Treasure, "Treasure", "Blocks/TreasureBlock", hardness: 250, health: 700, minimumDepth: 6500, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(15.97f, 16.05f), worth: 2000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Tungsten, "Tungsten", "Blocks/TungstenBlock", hardness: 300, health: 1100, minimumDepth: 7000, maximumDepth: 800000, occurrenceSpan: new Vector2(16.05f, 16.20f), worth: 160, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Uranium, "Uranium", "Blocks/UraniumBlock", hardness: 380, health: 1400, minimumDepth: 7600, maximumDepth: 900000, occurrenceSpan: new Vector2(16.20f, 16.35f), worth: 350, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Water, "Water", "Blocks/WaterBlock", hardness: 1, health: 20, minimumDepth: 8, maximumDepth: 80, occurrenceSpan: new Vector2(16.35f, 16.45f), weight: 0.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Wood, "Wood", "Blocks/WoodBlock", hardness: 3, health: 15, minimumDepth: 11, maximumDepth: 40, occurrenceSpan: new Vector2(16.45f, 16.55f), worth: 4, weight: 1.0f);
            
            AddTexturedBlock(manager, GameIds.Blocks.Rainbow, "Rainbow", "Blocks/RainbowBlock", hardness: 2600, health: 14000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.55f, 16.63f), worth: 12000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.DinosaurBone, "Dinosaur bone", "Blocks/DinosaurBoneBlock", hardness: 80, health: 300, minimumDepth: 3200, maximumDepth: 90000, occurrenceSpan: new Vector2(16.63f, 16.71f), worth: 75, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.ShardOfDeath, "Shard of death", "Blocks/ShardOfDeathBlock", hardness: 3200, health: 18000, minimumDepth: 24000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.71f, 16.79f), worth: 18000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.ShardOfLife, "Shard of life", "Blocks/ShardOfLifeBlock", hardness: 3200, health: 18000, minimumDepth: 24000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.79f, 16.87f), worth: 18000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.SuperNova, "Super nova", "Blocks/SuperNovaBlock", hardness: 75000f, health: 75000f, minimumDepth: 30000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.87f, 16.93f), worth: 100000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.WingOfDeath, "Wing of death", "Blocks/WingOfDeathBlock", hardness: 2800, health: 16000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.93f, 17.00f), worth: 15000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.WingOfLife, "Wing of life", "Blocks/WingOfLifeBlock", hardness: 2800, health: 16000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(17.00f, 17.07f), worth: 15000, weight: 1.0f);
            AddTexturedBlock(manager, GameIds.Blocks.Hole, "Hole", "Blocks/DirtBlockBackground", hardness: 0, health: 0, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(17.07f, 30.00f), worth: 0, ethereal: true, weight: 0.0f);

            AddTexturedBlock(manager, GameIds.Blocks.Coal, "Coal", "Blocks/CoalBlock", hardness: 2, health: 5, minimumDepth: 6, maximumDepth: 12000, occurrenceSpan: new Vector2(30.00f, 30.40f), worth: 2, weight: 0.2f);
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

