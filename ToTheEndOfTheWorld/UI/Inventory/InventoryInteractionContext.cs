using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Concrete.Grids;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public readonly record struct InventoryInteractionContext(
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
        bool BlockCrafting,
        int InventoryScrollOffset,
        Func<ModelWorld, AGridBox, bool> TrySellSlotFunc,
        Func<Point, int, int, (AGridBox slot, int maxStackSize)?> TryGetChestSlotFunc)
    {
        public bool TrySellSlot(AGridBox slot) => TrySellSlotFunc?.Invoke(World, slot) ?? false;
        public (AGridBox slot, int maxStackSize)? TryGetChestSlot(Point pos) => TryGetChestSlotFunc?.Invoke(pos, ViewportWidth, ViewportHeight);
    }
}
