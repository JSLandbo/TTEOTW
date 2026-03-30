using ModelLibrary.Concrete.Blocks;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class CraftingRecipeLibrary
    {
        private const short DirtBlockId = 2;
        private const short RockBlockId = 4;
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
                        new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1),
                        new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1),
                        new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1), new CraftingIngredient(DirtBlockId, 1)),
                    CreateOutput: () => CreateBlockItem(RockBlockId),
                    OutputCount: 1)
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
