using ModelLibrary.Abstract.Types;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed class CraftingRecipeLibrary(GameItemsRepository items)
    {
        public CraftingRecipe[] CreateRecipes()
        {
            return
            [
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Iron, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Copper, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Tin, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Silver, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Gold, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Lead, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Aluminium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Zinc, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Nickel, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Bismuth, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Titanium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TitaniumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Tungsten, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TungstenIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Uranium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.UraniumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Osmium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.OsmiumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Cobalt, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CobaltIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Chromium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ChromiumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Platinum, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.PlatinumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Rainbow, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.RainbowIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Mythril, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.MythrilIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Adamantium, 1)),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AdamantiumIngot),
                    OutputCount: 1),
            ];
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
