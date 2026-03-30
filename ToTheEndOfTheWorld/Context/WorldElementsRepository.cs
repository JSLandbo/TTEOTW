using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public class WorldElementsRepository : Dictionary<int, (string Name, Texture2D Texture, Block block)>
    {
        private readonly Dictionary<int, string> placeholderLabels = new();
        private int nextPlaceholderLabel = 1;

        public WorldElementsRepository(ContentManager manager)
        {
            InitializeCollection(manager);
        }

        public bool TryGetPlaceholderLabel(int blockId, out string label)
        {
            return placeholderLabels.TryGetValue(blockId, out label);
        }

        private void InitializeCollection(ContentManager manager)
        {
            var placeholderTexture = manager.Load<Texture2D>("Blocks/StoneBlock");

            AddRuntimeBlock(manager, -2, "Breaking", "General/Breaking/Breaking", ethereal: true);
            AddRuntimeBlock(manager, -1, "Background", "Blocks/DirtBlockBackground", ethereal: true);
            AddRuntimeBlock(manager, 0, "Air", "Blocks/AirBlock", ethereal: true, minimumDepth: long.MinValue, maximumDepth: 10, occurrenceSpan: new Vector2(0f, 100f));

            AddTexturedBlock(manager, 1, "Grass", "Blocks/GrassBlock", hardness: 0, health: 50, minimumDepth: 11, maximumDepth: 11, occurrenceSpan: new Vector2(0f, 100f));
            AddTexturedBlock(manager, 2, "Dirt", "Blocks/DirtBlock", hardness: 0, health: 10, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(0f, 100f));
            AddPlaceholderBlock(placeholderTexture, 3, "Gravel", hardness: 1, health: 5, minimumDepth: 15, maximumDepth: 220, occurrenceSpan: new Vector2(0f, 2f));
            AddTexturedBlock(manager, 4, "Rock", "Blocks/StoneBlock", hardness: 20, health: 50, minimumDepth: 12, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(2f, 4f));
            AddPlaceholderBlock(placeholderTexture, 5, "Granite", hardness: 50, health: 100, minimumDepth: 8000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(4f, 5f));
            AddTexturedBlock(manager, 6, "Iron ore", "Blocks/IronBlock", hardness: 15, health: 75, minimumDepth: 100, maximumDepth: 800000, occurrenceSpan: new Vector2(5f, 6f));
            AddPlaceholderBlock(placeholderTexture, 7, "Lead", hardness: 5, health: 50, minimumDepth: 200, maximumDepth: 40000, occurrenceSpan: new Vector2(6f, 6.5f));
            AddPlaceholderBlock(placeholderTexture, 8, "Aluminium", hardness: 15, health: 100, minimumDepth: 300, maximumDepth: 60000, occurrenceSpan: new Vector2(6.5f, 7f));
            AddPlaceholderBlock(placeholderTexture, 9, "Zinc", hardness: 20, health: 150, minimumDepth: 500, maximumDepth: 80000, occurrenceSpan: new Vector2(7f, 8f));
            AddTexturedBlock(manager, 10, "Copper ore", "Blocks/CopperBlock", hardness: 25, health: 200, minimumDepth: 800, maximumDepth: 100000, occurrenceSpan: new Vector2(8f, 8.5f));
            AddPlaceholderBlock(placeholderTexture, 11, "Nickel", hardness: 35, health: 300, minimumDepth: 1200, maximumDepth: 140000, occurrenceSpan: new Vector2(8.5f, 9f));
            AddPlaceholderBlock(placeholderTexture, 12, "Tin ore", hardness: 10, health: 50, minimumDepth: 400, maximumDepth: 20000, occurrenceSpan: new Vector2(9f, 9.5f));
            AddPlaceholderBlock(placeholderTexture, 13, "Silver ore", hardness: 20, health: 85, minimumDepth: 1600, maximumDepth: 200000, occurrenceSpan: new Vector2(9.5f, 10f));
            AddTexturedBlock(manager, 14, "Gold ore", "Blocks/GoldBlock", hardness: 20, health: 250, minimumDepth: 2400, maximumDepth: 400000, occurrenceSpan: new Vector2(10f, 10.25f));
            AddPlaceholderBlock(placeholderTexture, 15, "Mythril", hardness: 500, health: 2500, minimumDepth: 3600, maximumDepth: 800000, occurrenceSpan: new Vector2(10.25f, 10.50f));
            AddPlaceholderBlock(placeholderTexture, 16, "Adamantium", hardness: 1500, health: 8500, minimumDepth: 5400, maximumDepth: 1600000, occurrenceSpan: new Vector2(10.50f, 10.625f));

            AddPlaceholderBlock(placeholderTexture, 17, "Alexandrite", hardness: 70, health: 300, minimumDepth: 2600, maximumDepth: 500000, occurrenceSpan: new Vector2(10.625f, 10.80f));
            AddPlaceholderBlock(placeholderTexture, 18, "Amber", hardness: 12, health: 30, minimumDepth: 40, maximumDepth: 2400, occurrenceSpan: new Vector2(10.80f, 10.95f));
            AddTexturedBlock(manager, 19, "Amethyst", "Blocks/AmethystBlock", hardness: 80, health: 350, minimumDepth: 3000, maximumDepth: 600000, occurrenceSpan: new Vector2(10.95f, 11.10f));
            AddPlaceholderBlock(placeholderTexture, 20, "Apatite", hardness: 18, health: 60, minimumDepth: 200, maximumDepth: 12000, occurrenceSpan: new Vector2(11.10f, 11.25f));
            AddPlaceholderBlock(placeholderTexture, 21, "Aquamarine", hardness: 45, health: 180, minimumDepth: 1800, maximumDepth: 180000, occurrenceSpan: new Vector2(11.25f, 11.40f));
            AddPlaceholderBlock(placeholderTexture, 22, "Artifact", hardness: 1200, health: 6000, minimumDepth: 9000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.40f, 11.48f));
            AddPlaceholderBlock(placeholderTexture, 23, "Artifacts", hardness: 1300, health: 7000, minimumDepth: 12000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.48f, 11.56f));
            AddPlaceholderBlock(placeholderTexture, 24, "Bedrock", hardness: 100000f, health: 100000f, minimumDepth: 20000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.56f, 11.66f));
            AddPlaceholderBlock(placeholderTexture, 25, "Bismuth", hardness: 45, health: 240, minimumDepth: 1500, maximumDepth: 150000, occurrenceSpan: new Vector2(11.66f, 11.82f));
            AddTexturedBlock(manager, 26, "Black hole", "Blocks/BlackHoleBlock", hardness: 50000f, health: 50000f, minimumDepth: 16000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(11.82f, 11.88f));
            AddPlaceholderBlock(placeholderTexture, 27, "Calcite", hardness: 16, health: 55, minimumDepth: 300, maximumDepth: 14000, occurrenceSpan: new Vector2(11.88f, 12.03f));
            AddPlaceholderBlock(placeholderTexture, 28, "Chest", hardness: 5, health: 25, minimumDepth: 25, maximumDepth: 4000, occurrenceSpan: new Vector2(12.03f, 12.12f));
            AddPlaceholderBlock(placeholderTexture, 29, "Chromium", hardness: 75, health: 320, minimumDepth: 2800, maximumDepth: 280000, occurrenceSpan: new Vector2(12.12f, 12.28f));
            AddPlaceholderBlock(placeholderTexture, 30, "Cinnabar", hardness: 60, health: 260, minimumDepth: 2200, maximumDepth: 220000, occurrenceSpan: new Vector2(12.28f, 12.43f));
            AddPlaceholderBlock(placeholderTexture, 31, "Cobalt", hardness: 85, health: 350, minimumDepth: 3200, maximumDepth: 300000, occurrenceSpan: new Vector2(12.43f, 12.58f));
            AddPlaceholderBlock(placeholderTexture, 32, "Citrine", hardness: 55, health: 220, minimumDepth: 1900, maximumDepth: 180000, occurrenceSpan: new Vector2(12.58f, 12.73f));
            AddTexturedBlock(manager, 33, "Diamond", "Blocks/DiamondBlock", hardness: 400, health: 1200, minimumDepth: 4400, maximumDepth: 900000, occurrenceSpan: new Vector2(12.73f, 12.88f));
            AddPlaceholderBlock(placeholderTexture, 34, "Diode", hardness: 140, health: 420, minimumDepth: 3800, maximumDepth: 350000, occurrenceSpan: new Vector2(12.88f, 12.98f));
            AddTexturedBlock(manager, 35, "Emerald", "Blocks/EmeraldBlock", hardness: 95, health: 360, minimumDepth: 3400, maximumDepth: 320000, occurrenceSpan: new Vector2(12.98f, 13.13f));
            AddPlaceholderBlock(placeholderTexture, 36, "Garnet", hardness: 70, health: 260, minimumDepth: 2400, maximumDepth: 220000, occurrenceSpan: new Vector2(13.13f, 13.28f));
            AddPlaceholderBlock(placeholderTexture, 37, "Lapis Lazuli", hardness: 65, health: 240, minimumDepth: 2100, maximumDepth: 210000, occurrenceSpan: new Vector2(13.28f, 13.43f));
            AddPlaceholderBlock(placeholderTexture, 38, "Lava", hardness: 40, health: 100, minimumDepth: 4500, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(13.43f, 13.53f), ethereal: false);
            AddPlaceholderBlock(placeholderTexture, 39, "Malachite", hardness: 50, health: 210, minimumDepth: 1700, maximumDepth: 170000, occurrenceSpan: new Vector2(13.53f, 13.68f));
            AddPlaceholderBlock(placeholderTexture, 40, "Obsidian", hardness: 650, health: 2600, minimumDepth: 5200, maximumDepth: 1000000, occurrenceSpan: new Vector2(13.68f, 13.83f));
            AddPlaceholderBlock(placeholderTexture, 41, "Opal", hardness: 58, health: 210, minimumDepth: 2100, maximumDepth: 180000, occurrenceSpan: new Vector2(13.83f, 13.98f));
            AddPlaceholderBlock(placeholderTexture, 42, "Osmium", hardness: 160, health: 600, minimumDepth: 4800, maximumDepth: 500000, occurrenceSpan: new Vector2(13.98f, 14.13f));
            AddPlaceholderBlock(placeholderTexture, 43, "Peridot", hardness: 48, health: 180, minimumDepth: 1600, maximumDepth: 140000, occurrenceSpan: new Vector2(14.13f, 14.28f));
            AddPlaceholderBlock(placeholderTexture, 44, "Platinum", hardness: 200, health: 700, minimumDepth: 5000, maximumDepth: 600000, occurrenceSpan: new Vector2(14.28f, 14.43f));
            AddPlaceholderBlock(placeholderTexture, 45, "Pyrite", hardness: 52, health: 200, minimumDepth: 1900, maximumDepth: 160000, occurrenceSpan: new Vector2(14.43f, 14.58f));
            AddPlaceholderBlock(placeholderTexture, 46, "Quartz", hardness: 42, health: 160, minimumDepth: 1200, maximumDepth: 120000, occurrenceSpan: new Vector2(14.58f, 14.73f));
            AddPlaceholderBlock(placeholderTexture, 47, "Rainbow crystal", hardness: 1800, health: 9000, minimumDepth: 14000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(14.73f, 14.83f));
            AddPlaceholderBlock(placeholderTexture, 48, "Rainbow gem", hardness: 2200, health: 12000, minimumDepth: 18000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(14.83f, 14.93f));
            AddPlaceholderBlock(placeholderTexture, 49, "Ring", hardness: 200, health: 500, minimumDepth: 6000, maximumDepth: 700000, occurrenceSpan: new Vector2(14.93f, 15.00f));
            AddTexturedBlock(manager, 50, "Ruby", "Blocks/RubyBlock", hardness: 100, health: 380, minimumDepth: 3600, maximumDepth: 360000, occurrenceSpan: new Vector2(15.00f, 15.15f));
            AddPlaceholderBlock(placeholderTexture, 51, "Sapphire", hardness: 105, health: 390, minimumDepth: 3700, maximumDepth: 370000, occurrenceSpan: new Vector2(15.15f, 15.30f));
            AddPlaceholderBlock(placeholderTexture, 52, "Silicon", hardness: 45, health: 170, minimumDepth: 1400, maximumDepth: 120000, occurrenceSpan: new Vector2(15.30f, 15.45f));
            AddPlaceholderBlock(placeholderTexture, 53, "Skeleton", hardness: 8, health: 25, minimumDepth: 40, maximumDepth: 7000, occurrenceSpan: new Vector2(15.45f, 15.52f));
            AddPlaceholderBlock(placeholderTexture, 54, "Sulphur", hardness: 25, health: 90, minimumDepth: 500, maximumDepth: 30000, occurrenceSpan: new Vector2(15.52f, 15.67f));
            AddPlaceholderBlock(placeholderTexture, 55, "Titanium", hardness: 240, health: 900, minimumDepth: 6200, maximumDepth: 700000, occurrenceSpan: new Vector2(15.67f, 15.82f));
            AddPlaceholderBlock(placeholderTexture, 56, "Topaz", hardness: 82, health: 330, minimumDepth: 3000, maximumDepth: 300000, occurrenceSpan: new Vector2(15.82f, 15.97f));
            AddPlaceholderBlock(placeholderTexture, 57, "Treasure", hardness: 250, health: 700, minimumDepth: 6500, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(15.97f, 16.05f));
            AddPlaceholderBlock(placeholderTexture, 58, "Tungsten", hardness: 300, health: 1100, minimumDepth: 7000, maximumDepth: 800000, occurrenceSpan: new Vector2(16.05f, 16.20f));
            AddTexturedBlock(manager, 59, "Uranium", "Blocks/UraniumBlock", hardness: 380, health: 1400, minimumDepth: 7600, maximumDepth: 900000, occurrenceSpan: new Vector2(16.20f, 16.35f));
            AddPlaceholderBlock(placeholderTexture, 60, "Water", hardness: 0, health: 0, minimumDepth: 8, maximumDepth: 80, occurrenceSpan: new Vector2(16.35f, 16.45f), ethereal: true);
            AddPlaceholderBlock(placeholderTexture, 61, "Wood", hardness: 3, health: 15, minimumDepth: 11, maximumDepth: 40, occurrenceSpan: new Vector2(16.45f, 16.55f));

            AddTexturedBlock(manager, 62, "Rainbow", "Blocks/Rainbow", hardness: 2600, health: 14000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.55f, 16.63f));
            AddTexturedBlock(manager, 63, "Dinosaur bone", "Blocks/DinosaurBoneBlock", hardness: 80, health: 300, minimumDepth: 3200, maximumDepth: 90000, occurrenceSpan: new Vector2(16.63f, 16.71f));
            AddTexturedBlock(manager, 64, "Shard of death", "Blocks/ShardOfDeathBlock", hardness: 3200, health: 18000, minimumDepth: 24000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.71f, 16.79f));
            AddTexturedBlock(manager, 65, "Shard of life", "Blocks/ShardOfLifeBlock", hardness: 3200, health: 18000, minimumDepth: 24000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.79f, 16.87f));
            AddTexturedBlock(manager, 66, "Super nova", "Blocks/SuperNovaBlock", hardness: 75000f, health: 75000f, minimumDepth: 30000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.87f, 16.93f));
            AddTexturedBlock(manager, 67, "Wing of death", "Blocks/WingOfDeathBlock", hardness: 2800, health: 16000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(16.93f, 17.00f));
            AddTexturedBlock(manager, 68, "Wing of life", "Blocks/WingOfLifeBlock", hardness: 2800, health: 16000, minimumDepth: 22000, maximumDepth: long.MaxValue, occurrenceSpan: new Vector2(17.00f, 17.07f));
        }

        private void AddRuntimeBlock(ContentManager manager, short id, string name, string assetPath, bool ethereal, long minimumDepth = long.MinValue, long maximumDepth = long.MaxValue, Vector2? occurrenceSpan = null)
        {
            var info = new BlockInfo(
                MinimumDepth: minimumDepth,
                MaximumDepth: maximumDepth,
                OccurrenceSpan: occurrenceSpan ?? new Vector2(),
                Weight: 0.0f);

            var block = new Block(ID: id, Ethereal: ethereal, Info: info);
            Add(id, (name, manager.Load<Texture2D>(assetPath), block));
        }

        private void AddTexturedBlock(ContentManager manager, short id, string name, string assetPath, float hardness, float health, long minimumDepth, long maximumDepth, Vector2 occurrenceSpan, bool ethereal = false)
        {
            var info = new BlockInfo(
                MinimumDepth: minimumDepth,
                MaximumDepth: maximumDepth,
                OccurrenceSpan: occurrenceSpan,
                Weight: 0.0f);

            var block = new Block(ID: id, Ethereal: ethereal, Hardness: hardness, Health: health, Worth: 0, Info: info);
            Add(id, (name, manager.Load<Texture2D>(assetPath), block));
        }

        private void AddPlaceholderBlock(Texture2D placeholderTexture, short id, string name, float hardness, float health, long minimumDepth, long maximumDepth, Vector2 occurrenceSpan, bool ethereal = false)
        {
            var info = new BlockInfo(
                MinimumDepth: minimumDepth,
                MaximumDepth: maximumDepth,
                OccurrenceSpan: occurrenceSpan,
                Weight: 0.0f);

            var block = new Block(ID: id, Ethereal: ethereal, Hardness: hardness, Health: health, Worth: 0, Info: info);
            Add(id, (name, placeholderTexture, block));
            placeholderLabels[id] = nextPlaceholderLabel.ToString();
            nextPlaceholderLabel++;
        }
    }
}
