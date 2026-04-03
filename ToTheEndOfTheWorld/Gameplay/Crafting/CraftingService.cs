using System;
using System.Collections.Generic;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed class CraftingService(GameEventBus eventBus, IReadOnlyList<CraftingRecipe> recipes)
    {
        public bool TryCraft(AGridBox[,] craftingGrid, AGridBox outputSlot, int maxStackSize)
        {
            if (!TryResolveRecipe(craftingGrid, outputSlot, maxStackSize, out CraftingRecipe recipe, out AType craftedItem, out int craftedCount, out int craftCount))
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

            ConsumeRecipeIngredients(craftingGrid, recipe, craftCount);
            eventBus.Publish(new PlayerCraftedItemEvent(craftedItem, craftedCount));

            return true;
        }

        private bool TryResolveRecipe(AGridBox[,] craftingGrid, AGridBox outputSlot, int maxStackSize, out CraftingRecipe recipe, out AType craftedItem, out int craftedCount, out int craftCount)
        {
            foreach (CraftingRecipe candidate in recipes)
            {
                if (!TryGetCraftCount(craftingGrid, candidate, out craftCount))
                {
                    continue;
                }

                recipe = candidate;
                craftedItem = candidate.CreateOutput();

                if (craftedItem == null || candidate.OutputCount <= 0)
                {
                    continue;
                }

                if (outputSlot.Item != null)
                {
                    if (!InventoryService.CanStackTogether(outputSlot.Item, craftedItem))
                    {
                        continue;
                    }

                    int availableOutputSpace = maxStackSize - outputSlot.Count;
                    craftCount = Math.Min(craftCount, availableOutputSpace / candidate.OutputCount);
                }
                else
                {
                    craftCount = Math.Min(craftCount, maxStackSize / candidate.OutputCount);
                }

                if (craftCount <= 0)
                {
                    continue;
                }

                craftedCount = candidate.OutputCount * craftCount;

                return true;
            }

            recipe = null;
            craftedItem = null;
            craftedCount = 0;
            craftCount = 0;
            return false;
        }

        private static void ConsumeRecipeIngredients(AGridBox[,] craftingGrid, CraftingRecipe recipe, int craftCount)
        {
            for (int y = 0; y < craftingGrid.GetLength(1); y++)
            {
                for (int x = 0; x < craftingGrid.GetLength(0); x++)
                {
                    CraftingIngredient ingredient = recipe.Pattern[x, y];

                    if (ingredient == null)
                    {
                        continue;
                    }

                    craftingGrid[x, y].Count -= ingredient.RequiredCount * craftCount;

                    if (craftingGrid[x, y].Count <= 0)
                    {
                        craftingGrid[x, y].Item = null;
                        craftingGrid[x, y].Count = 0;
                    }
                }
            }
        }

        private static bool TryGetCraftCount(AGridBox[,] craftingGrid, CraftingRecipe recipe, out int craftCount)
        {
            craftCount = int.MaxValue;
            bool hasRequiredIngredient = false;

            for (int y = 0; y < craftingGrid.GetLength(1); y++)
            {
                for (int x = 0; x < craftingGrid.GetLength(0); x++)
                {
                    AGridBox slot = craftingGrid[x, y];
                    CraftingIngredient ingredient = recipe.Pattern[x, y];

                    if (ingredient == null)
                    {
                        if (slot.Item != null && slot.Count > 0)
                        {
                            craftCount = 0;

                            return false;
                        }

                        continue;
                    }

                    if (slot.Item == null || slot.Count < ingredient.RequiredCount || slot.Item.ID != ingredient.ItemId)
                    {
                        craftCount = 0;

                        return false;
                    }

                    hasRequiredIngredient = true;
                    craftCount = Math.Min(craftCount, slot.Count / ingredient.RequiredCount);
                }
            }

            if (!hasRequiredIngredient || craftCount <= 0)
            {
                craftCount = 0;

                return false;
            }

            return true;
        }
    }
}
