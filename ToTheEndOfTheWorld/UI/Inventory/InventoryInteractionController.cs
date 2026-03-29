using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ToTheEndOfTheWorld.Gameplay;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryInteractionController
    {
        private AGridBox heldSourceSlot;

        public AType HeldItem { get; private set; }
        public int HeldCount { get; private set; }
        public Point MousePosition { get; private set; }

        public void Update(
            MouseState currentMouseState,
            MouseState previousMouseState,
            InventoryLayout layout,
            AGridBox[,] inventoryGrid,
            Grid craftingGrid,
            GridBox craftOutputSlot,
            CraftingService craftingService)
        {
            MousePosition = currentMouseState.Position;

            if (WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (layout.CraftButtonRectangle.Contains(MousePosition))
                {
                    craftingService.TryCraft(craftingGrid.InternalGrid, craftOutputSlot);
                    return;
                }

                if (TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, out var clickedSlot))
                {
                    MoveStack(clickedSlot);
                }

                return;
            }

            if (WasRightClicked(currentMouseState, previousMouseState)
                && TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, out var rightClickedSlot))
            {
                TakeSingleItem(rightClickedSlot);
            }
        }

        public void ReleaseHeldItem(InventoryService inventoryService, ModelLibrary.Abstract.PlayerShipComponents.AInventory inventory)
        {
            if (HeldItem == null || HeldCount <= 0)
            {
                return;
            }

            if (inventoryService.TryAdd(inventory, HeldItem, HeldCount))
            {
                ClearHeldItem();
                return;
            }

            if (heldSourceSlot == null)
            {
                return;
            }

            if (heldSourceSlot.Item == null || heldSourceSlot.Count <= 0)
            {
                heldSourceSlot.Item = HeldItem;
                heldSourceSlot.Count = HeldCount;
                ClearHeldItem();
                return;
            }

            if (heldSourceSlot.Item.ID == HeldItem.ID && heldSourceSlot.Count + HeldCount <= InventoryService.MaxStackSize)
            {
                heldSourceSlot.Count += HeldCount;
                ClearHeldItem();
            }
        }

        public void ReturnCraftingGridToInventory(InventoryService inventoryService, ModelLibrary.Abstract.PlayerShipComponents.AInventory inventory, Grid craftingGrid)
        {
            var grid = craftingGrid.InternalGrid;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    if (!inventoryService.TryAdd(inventory, slot.Item, slot.Count))
                    {
                        continue;
                    }

                    slot.Item = null;
                    slot.Count = 0;
                }
            }
        }

        private void MoveStack(AGridBox slot)
        {
            if (HeldItem == null || HeldCount == 0)
            {
                if (slot.Item == null || slot.Count <= 0)
                {
                    return;
                }

                HeldItem = slot.Item;
                HeldCount = slot.Count;
                heldSourceSlot = slot;
                slot.Item = null;
                slot.Count = 0;
                return;
            }

            if (slot.Item == null || slot.Count <= 0)
            {
                slot.Item = HeldItem;
                slot.Count = HeldCount;
                ClearHeldItem();
                return;
            }

            if (slot.Item.ID == HeldItem.ID)
            {
                var availableSpace = InventoryService.MaxStackSize - slot.Count;

                if (availableSpace <= 0)
                {
                    return;
                }

                var movedCount = HeldCount > availableSpace ? availableSpace : HeldCount;
                slot.Count += movedCount;
                HeldCount -= movedCount;

                if (HeldCount == 0)
                {
                    ClearHeldItem();
                }

                return;
            }

            var swapItem = slot.Item;
            var swapCount = slot.Count;
            slot.Item = HeldItem;
            slot.Count = HeldCount;
            HeldItem = swapItem;
            HeldCount = swapCount;
            heldSourceSlot = slot;
        }

        private void TakeSingleItem(AGridBox slot)
        {
            if (slot.Item == null || slot.Count <= 0)
            {
                return;
            }

            if (HeldItem != null && HeldItem.ID != slot.Item.ID)
            {
                return;
            }

            if (HeldCount >= InventoryService.MaxStackSize)
            {
                return;
            }

            HeldItem ??= slot.Item;
            HeldCount += 1;
            heldSourceSlot ??= slot;
            slot.Count -= 1;

            if (slot.Count <= 0)
            {
                slot.Item = null;
                slot.Count = 0;
            }
        }

        private void ClearHeldItem()
        {
            HeldItem = null;
            HeldCount = 0;
            heldSourceSlot = null;
        }

        private static bool TryGetClickedSlot(Point position, AGridBox[,] inventoryGrid, InventoryLayout layout, Grid craftingGrid, GridBox craftOutputSlot, out AGridBox slot)
        {
            if (TryGetClickedSlot(craftingGrid.InternalGrid, layout.CraftingStart.X, layout.CraftingStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                return true;
            }

            if (layout.OutputSlotRectangle.Contains(position))
            {
                slot = craftOutputSlot;
                return true;
            }

            if (TryGetClickedSlot(inventoryGrid, layout.InventoryStart.X, layout.InventoryStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                return true;
            }

            slot = null;
            return false;
        }

        private static bool TryGetClickedSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slotRectangle = new Rectangle(
                        startX + (x * (slotSize + slotSpacing)),
                        startY + (y * (slotSize + slotSpacing)),
                        slotSize,
                        slotSize
                    );

                    if (slotRectangle.Contains(position))
                    {
                        slot = grid[x, y];
                        return true;
                    }
                }
            }

            slot = null;
            return false;
        }

        private static bool WasLeftClicked(MouseState currentMouseState, MouseState previousMouseState)
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        private static bool WasRightClicked(MouseState currentMouseState, MouseState previousMouseState)
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }
    }
}
