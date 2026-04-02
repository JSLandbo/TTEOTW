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
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Iron, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.IronIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Copper, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.CopperIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Tin, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.TinIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Silver, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.SilverIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Gold, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.GoldIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Lead, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.LeadIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Aluminium, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.AluminiumIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Zinc, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.ZincIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Nickel, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.NickelIngot),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Coal, 1), new CraftingIngredient(GameIds.Blocks.Bismuth, 1), null,
                        null, null, null,
                        null, null, null),
                    CreateOutput: () => CreateItem(GameIds.Items.CratingMaterials.BismuthIngot),
                    OutputCount: 1),
            ];
        }

        private ModelLibrary.Abstract.Types.AType CreateItem(short itemId)
        {
            return items.Create((short)itemId);
        }

        private static CraftingIngredient[,] CreatePattern(
            CraftingIngredient topLeft,
            CraftingIngredient topMiddle,
            CraftingIngredient topRight,
            CraftingIngredient middleLeft,
            CraftingIngredient middleMiddle,
            CraftingIngredient middleRight,
            CraftingIngredient bottomLeft,
            CraftingIngredient bottomMiddle,
            CraftingIngredient bottomRight)
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
