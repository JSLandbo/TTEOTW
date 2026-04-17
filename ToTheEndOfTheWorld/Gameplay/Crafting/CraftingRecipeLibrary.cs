using ModelLibrary.Abstract.Types;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed class CraftingRecipeLibrary(GameItemsRepository items)
    {
        public CraftingRecipe[] CreateRecipes()
        {
            CraftingRecipe[] oresIngotsGemsBlocks = [

                // Coal to ingots and polished gems
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
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Amethyst, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AmethystPolished),
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

                // Lava to ingots and polished gems
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 1), new CraftingIngredient(GameIds.Blocks.Diamond, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.DiamondPolished),
                    OutputCount: 1
                ),
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
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Lava, 64), new CraftingIngredient(GameIds.Blocks.Bocant, 1
                    )),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BocantIngot),
                    OutputCount: 1
                ),

                // Metals to ingots and cubes
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
                new(Pattern: CreatePattern(
                         new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1),
                         new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1),
                         new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 1)),
                     CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BocantCube),
                     OutputCount: 1
                 ),
                 new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.BocantIngot, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BocantCube), OutputCount: 1),
                 new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BocantIngot), OutputCount: 9),

                // Polished crystals to cubes
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AmethystCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystPolished, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AmethystCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AmethystPolished), OutputCount: 9),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.DiamondCube),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondPolished, 9)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.DiamondCube), OutputCount: 1),
                new(Pattern: CreatePattern(new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1)), CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.DiamondPolished), OutputCount: 9),
            ];

            CraftingRecipe[] playerEquipment = [

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
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Copper),
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
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Iron),
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
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Gold),
                    OutputCount: 1
                ),

                // Gold to Amethyst
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Gold, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Gold, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Gold, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.Containers.Gold, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Gold, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Gold, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Amethyst),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Gold, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AmethystCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Amethyst),
                    OutputCount: 1
                ),

                // Amethyst to diamond
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Amethyst, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Amethyst, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Amethyst, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.Containers.Amethyst, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Amethyst, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Amethyst, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Diamond),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Amethyst, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.DiamondCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Diamond),
                    OutputCount: 1
                ),

                // Diamond to radioactive
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Diamond, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Diamond, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Diamond, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.Containers.Diamond, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Diamond, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Diamond, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Radioactive),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Diamond, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.UraniumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Radioactive),
                    OutputCount: 1
                ),

                // Radioactive to rainbow
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Radioactive, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Radioactive, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Radioactive, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.Containers.Radioactive, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Radioactive, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Radioactive, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Rainbow),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Radioactive, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.RainbowCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Rainbow),
                    OutputCount: 1
                ),

                // Rainbow to mythril
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Rainbow, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Rainbow, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Rainbow, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.Containers.Rainbow, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Rainbow, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Rainbow, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Mythril),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Rainbow, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.MythrilCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Mythril),
                    OutputCount: 1
                ),

                // Mythril to adamantium
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Mythril, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Mythril, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Mythril, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.Containers.Mythril, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Mythril, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Mythril, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Adamant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Mythril, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.AdamantiumCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Adamant),
                    OutputCount: 1
                ),

                // Adamant to Bocant
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        null, new CraftingIngredient(GameIds.Items.ThermalPlatings.Adamant, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.ThermalPlatings.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Engines.Adamant, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Engines.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.FuelTanks.Adamant, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.FuelTanks.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.Containers.Adamant, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Containers.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.Thrusters.Adamant, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Thrusters.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null,
                        null, new CraftingIngredient(GameIds.Items.Hulls.Adamant, 1), null,
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1)
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Hulls.Bocant),
                    OutputCount: 1
                ),
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1),
                        null, new CraftingIngredient(GameIds.Items.Drills.Adamant, 1), null,
                        null, new CraftingIngredient(GameIds.Items.CratingMaterials.BocantCube, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.Drills.Bocant),
                    OutputCount: 1
                ),
            ];

            CraftingRecipe[] cheatCodes = [
                new(Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Grass, 1), null, new CraftingIngredient(GameIds.Blocks.Grass, 1),
                        new CraftingIngredient(GameIds.Blocks.Grass, 1), new CraftingIngredient(GameIds.Blocks.Grass, 1), new CraftingIngredient(GameIds.Blocks.Grass, 1),
                        null, new CraftingIngredient(GameIds.Blocks.Grass, 1), null
                    ),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumCube),
                    OutputCount: 1
                ),
            ];

            return [.. oresIngotsGemsBlocks, .. playerEquipment, .. cheatCodes];
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
