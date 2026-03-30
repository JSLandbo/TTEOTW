using ModelLibrary.Concrete.Blocks;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class CraftingRecipeLibrary
    {
        private readonly WorldElementsRepository blocks;

        public CraftingRecipeLibrary(WorldElementsRepository blocks)
        {
            this.blocks = blocks;
        }

        public CraftingRecipe[] CreateRecipes()
        {
            return new[]
            {
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1),
                        new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1),
                        new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1), new CraftingIngredient(GameIds.Blocks.Dirt, 1)),
                    CreateOutput: () => CreateBlockItem(GameIds.Blocks.Rock),
                    OutputCount: 1),
                new CraftingRecipe(
                    Pattern: CreatePattern(
                        new CraftingIngredient(GameIds.Blocks.Dirt, 64), new CraftingIngredient(GameIds.Blocks.Dirt, 64), new CraftingIngredient(GameIds.Blocks.Dirt, 64),
                        new CraftingIngredient(GameIds.Blocks.Dirt, 64), null, new CraftingIngredient(GameIds.Blocks.Dirt, 64),
                        new CraftingIngredient(GameIds.Blocks.Dirt, 64), new CraftingIngredient(GameIds.Blocks.Dirt, 64), new CraftingIngredient(GameIds.Blocks.Dirt, 64)),
                    CreateOutput: () => CreateBlockItem(GameIds.Blocks.WingOfLife),
                    OutputCount: 64)
            };
        }

        private Block CreateBlockItem(short blockId)
        {
            if (!blocks.TryGetValue(blockId, out var blockDefinition))
            {
                return null;
            }

            return new Block(blockDefinition.block)
            {
                Name = blockDefinition.Name
            };
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
