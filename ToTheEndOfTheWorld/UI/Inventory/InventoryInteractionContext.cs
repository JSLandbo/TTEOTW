using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Concrete.Grids;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed record InventoryInteractionContext(
        InventoryLayout Layout,
        AGridBox[,] InventoryGrid,
        Grid CraftingGrid,
        GridBox CraftOutputSlot,
        CraftingService CraftingService,
        ModelWorld World,
        InventoryItemUseService ItemUseService,
        AInventory Inventory,
        int ViewportWidth,
        int ViewportHeight,
        bool BlockCrafting = false,
        Func<AGridBox, bool> TrySellSlot = null,
        Func<Point, (AGridBox slot, int maxStackSize)?> TryGetChestSlot = null);
}
