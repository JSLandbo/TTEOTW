using ModelLibrary.Abstract.Types;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed class CraftingRecipeLibrary(GameItemsRepository items)
    {
        public CraftingRecipe[] CreateRecipes()
        {
            var oresIngotsBlocks = new CraftingRecipe[] {
                // Coal to ingot
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Iron, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Copper, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Tin, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Silver, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Gold, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Lead, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Aluminium, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Zinc, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Nickel, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Bismuth, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Titanium, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TitaniumIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Tungsten, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TungstenIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Osmium, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.OsmiumIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Chromium, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ChromiumIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Platinum, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.PlatinumIngot),
                    OutputCount: 1
                ),

                // Lava to ingot
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Cobalt, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CobaltIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Uranium, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.UraniumIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Rainbow, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.RainbowIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Mythril, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.MythrilIngot),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Adamantium, 1
                    )),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumIngot),
                    OutputCount: 1
                ),

                // Ingot to cube
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.IronIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.CopperIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TinIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TinCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.SilverIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.SilverCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.GoldIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.LeadIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.LeadCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AluminiumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.ZincIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.ZincCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.NickelIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.NickelCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.BismuthCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TitaniumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TitaniumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TitaniumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TitaniumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TungstenCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TungstenCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.TungstenCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TungstenIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.UraniumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.UraniumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.UraniumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.OsmiumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.OsmiumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.OsmiumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.OsmiumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CobaltCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CobaltCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.CobaltCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CobaltIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ChromiumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ChromiumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.ChromiumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ChromiumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.PlatinumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.PlatinumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.PlatinumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.PlatinumIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.RainbowCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.RainbowCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.RainbowIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.MythrilCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.MythrilCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.MythrilIngot), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumIngot), OutputCount: 9),
            };

            var playerEquipment = new CraftingRecipe[] {
                // Scrap to Copper
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Scrap, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Scrap, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Scrap, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.Containers.Scrap, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Scrap, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Scrap, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Copper),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Scrap, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.CopperCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Copper),
                    OutputCount: 1
                ),
                // Copper to Iron
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Copper, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Copper, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Copper, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.Containers.Copper, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Copper, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Copper, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Iron),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Copper, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.IronCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Iron),
                    OutputCount: 1
                ),
                // Iron to Gold
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Iron, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Iron, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Iron, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.Containers.Iron, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Iron, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Iron, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Gold),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Iron, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.GoldCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Gold),
                    OutputCount: 1
                ),
                // Gold to Amethyst blocks ??, NOT CURRENTLY DOABLE
                // Crystal to Diamond ??, NOT CURRENTLY DOABLE
                // Diamond to Radioactive (OK, doable)
                // Radioactive to rainbow (OK, doable)
                // Rainbow to mythril (OK, doable)
                // Mythril to adamantinum (OK, doable)
            };

            return [.. oresIngotsBlocks, .. playerEquipment];
        }

        private AType CreateItem(short itemId)
        {
            return items.Create(itemId);
        }

        private static CraftingIngredient[,] CreatePattern(
            CraftingIngredient topLeft = null,
            CraftingIngredient topMiddle = null,
            CraftingIngredient topRight = null,
            CraftingIngredient middleLeft = null,
            CraftingIngredient middleMiddle = null,
            CraftingIngredient middleRight = null,
            CraftingIngredient bottomLeft = null,
            CraftingIngredient bottomMiddle = null,
            CraftingIngredient bottomRight = null)
        {
            return new[,]
            {
                { topLeft, middleLeft, bottomLeft },
                { topMiddle, middleMiddle, bottomMiddle },
                { topRight, middleRight, bottomRight }
            };
        }
    }
}
