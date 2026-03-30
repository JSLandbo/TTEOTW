using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using System;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed class CraftingService
    {
        private readonly IReadOnlyList<CraftingRecipe> recipes;

        public CraftingService(IReadOnlyList<CraftingRecipe> recipes)
        {
            this.recipes = recipes;
        }

        public bool TryCraft(AGridBox[,] craftingGrid, AGridBox outputSlot, int maxStackSize)
        {
            if (!TryResolveRecipe(craftingGrid, outputSlot, maxStackSize, out var recipe, out var craftedItem, out var craftedCount, out var craftCount))
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
            return true;
        }

        private bool TryResolveRecipe(AGridBox[,] craftingGrid, AGridBox outputSlot, int maxStackSize, out CraftingRecipe recipe, out AType craftedItem, out int craftedCount, out int craftCount)
        {
            foreach (var candidate in recipes)
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

                    var availableOutputSpace = maxStackSize - outputSlot.Count;
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
            for (var y = 0; y < craftingGrid.GetLength(1); y++)
            {
                for (var x = 0; x < craftingGrid.GetLength(0); x++)
                {
                    var ingredient = recipe.Pattern[x, y];
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
            var hasRequiredIngredient = false;

            for (var y = 0; y < craftingGrid.GetLength(1); y++)
            {
                for (var x = 0; x < craftingGrid.GetLength(0); x++)
                {
                    var slot = craftingGrid[x, y];
                    var ingredient = recipe.Pattern[x, y];

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
