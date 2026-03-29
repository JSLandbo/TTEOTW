using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class CraftingService
    {
        public bool TryCraft(AGridBox[,] craftingGrid, AGridBox outputSlot, int maxStackSize)
        {
            if (!TryResolveRecipe(craftingGrid, out var craftedItem, out var craftedCount))
            {
                return false;
            }

            if (outputSlot.Item != null)
            {
                if (!InventoryService.CanStackTogether(outputSlot.Item, craftedItem) || outputSlot.Count + craftedCount > maxStackSize)
                {
                    return false;
                }

                outputSlot.Count += craftedCount;
            }
            else
            {
                outputSlot.Item = craftedItem;
                outputSlot.Count = craftedCount;
            }

            ConsumeRecipeIngredients(craftingGrid);
            return true;
        }

        private static bool TryResolveRecipe(AGridBox[,] craftingGrid, out AType craftedItem, out int craftedCount)
        {
            craftedItem = null;
            craftedCount = 0;
            return false;
        }

        private static void ConsumeRecipeIngredients(AGridBox[,] craftingGrid)
        {
        }
    }
}
