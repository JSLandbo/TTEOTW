using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.World;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryInteractionController
    {
        private enum HeldItemReturnTarget
        {
            Inventory,
            Chest
        }

        private int currentMaxStackSize = InventoryService.DefaultMaxStackSize;
        private HeldItemReturnTarget heldItemReturnTarget = HeldItemReturnTarget.Inventory;
        private bool selectionRequested;
        private bool trashRequested;

        public AType HeldItem { get; private set; }
        public int HeldCount { get; private set; }
        public Point MousePosition { get; private set; }
        public bool HasHeldItem => HeldItem != null && HeldCount > 0;
        public bool PrefersChestReturn => heldItemReturnTarget == HeldItemReturnTarget.Chest;

        public bool ConsumeTrashRequest()
        {
            bool requested = trashRequested;
            trashRequested = false;
            return requested;
        }

        public bool ConsumeSelectionRequest()
        {
            bool requested = selectionRequested;
            selectionRequested = false;
            return requested;
        }

        public bool TryTakeHeldItem(out AType item, out int count, out bool prefersChestReturn)
        {
            if (!HasHeldItem)
            {
                item = null;
                count = 0;
                prefersChestReturn = false;
                return false;
            }

            item = HeldItem;
            count = HeldCount;
            prefersChestReturn = PrefersChestReturn;
            ClearHeldItem();
            return true;
        }

        public void Update(MouseState currentMouseState, MouseState previousMouseState, InventoryInteractionContext ctx)
        {
            MousePosition = currentMouseState.Position;
            currentMaxStackSize = ctx.Inventory.MaxStackSize > 0 ? ctx.Inventory.MaxStackSize : InventoryService.DefaultMaxStackSize;

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (!ctx.BlockCrafting && ctx.Layout.CraftButtonRectangle.Contains(MousePosition))
                {
                    ctx.CraftingService.TryCraft(ctx.CraftingGrid.InternalGrid, ctx.CraftOutputSlot, currentMaxStackSize);

                    return;
                }

                if (HeldItem != null && ctx.Layout.TrashBinRectangle.Contains(MousePosition))
                {
                    trashRequested = true;
                    ClearHeldItem();

                    return;
                }

                if (HeldItem != null && TryGetClickedEquipmentSlot(MousePosition, ctx.Layout, out EPlayerEquipmentSlotType equipmentSlot) && TryEquipHeldItem(ctx.World, ctx.ItemUseService, equipmentSlot))
                {
                    return;
                }

                // Check chest slots first (for drag-and-drop)
                if (ctx.TryGetChestSlotFunc != null)
                {
                    (AGridBox slot, int maxStackSize)? chestResult = ctx.TryGetChestSlot(MousePosition);
                    if (chestResult.HasValue && CanUseClickedSlot(chestResult.Value.slot))
                    {
                        MoveStackWithMaxSize(chestResult.Value.slot, chestResult.Value.maxStackSize, HeldItemReturnTarget.Chest);
                        return;
                    }
                }

                if (TryGetClickedSlot(MousePosition, ctx, out AGridBox clickedSlot) && CanUseClickedSlot(clickedSlot))
                {
                    // CTRL+click to sell when shop is open
                    if (ctx.TrySellSlotFunc != null && clickedSlot.Item != null && ctx.TrySellSlot(clickedSlot))
                    {
                        selectionRequested = true;
                        return;
                    }

                    MoveStack(clickedSlot, HeldItemReturnTarget.Inventory);
                }

                return;
            }

            if (UiInputHelper.WasRightClicked(currentMouseState, previousMouseState))
            {
                // Check chest slots for right-click
                if (ctx.TryGetChestSlotFunc != null)
                {
                    (AGridBox slot, int maxStackSize)? chestResult = ctx.TryGetChestSlot(MousePosition);
                    if (chestResult.HasValue && CanUseClickedSlot(chestResult.Value.slot))
                    {
                        if (HeldItem != null)
                            PlaceSingleHeldItemWithMaxSize(chestResult.Value.slot, chestResult.Value.maxStackSize);
                        else
                            PickupHalfStack(chestResult.Value.slot, HeldItemReturnTarget.Chest);
                        return;
                    }
                }

                if (TryGetClickedSlot(MousePosition, ctx, out AGridBox rightClickedSlot) && CanUseClickedSlot(rightClickedSlot))
                {
                    if (HeldItem != null)
                        PlaceSingleHeldItem(rightClickedSlot);
                    else
                        PickupHalfStack(rightClickedSlot, HeldItemReturnTarget.Inventory);
                }
            }
        }

        public bool IsPointerOverInteractiveElement(Point position, InventoryInteractionContext ctx)
        {
            if (!ctx.BlockCrafting && ctx.Layout.CraftButtonRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem == null && ctx.Layout.SortButtonRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem != null && ctx.Layout.TrashBinRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem != null && TryGetClickedEquipmentSlot(position, ctx.Layout, out EPlayerEquipmentSlotType equipmentSlot) && ctx.ItemUseService.CanEquip(HeldItem, equipmentSlot))
            {
                return true;
            }

            return TryGetClickedSlot(position, ctx, out AGridBox clickedSlot)
                && CanUseClickedSlot(clickedSlot)
                && UiSlotInteractionHelper.CanInteractWithSlot(clickedSlot, HasHeldItem);
        }

        private bool CanUseClickedSlot(AGridBox slot)
        {
            return HeldItem == null
                   || slot.OwnerGrid?.CanPlaceInSlot(slot, HeldItem) != false;
        }

        private void MoveStack(AGridBox slot, HeldItemReturnTarget returnTarget)
        {
            MoveStackWithMaxSize(slot, currentMaxStackSize, returnTarget);
        }

        private void MoveStackWithMaxSize(AGridBox slot, int maxStackSize, HeldItemReturnTarget returnTarget)
        {
            if (HeldItem == null || HeldCount == 0)
            {
                if (slot.Item == null || slot.Count <= 0)
                {
                    return;
                }

                HeldItem = slot.Item;
                HeldCount = slot.Count;
                heldItemReturnTarget = returnTarget;
                slot.Item = null;
                slot.Count = 0;
                selectionRequested = true;

                return;
            }

            if (slot.Item == null || slot.Count <= 0)
            {
                int toPlace = Math.Min(HeldCount, maxStackSize);
                slot.Item = HeldItem;
                slot.Count = toPlace;
                HeldCount -= toPlace;

                if (HeldCount == 0)
                {
                    ClearHeldItem();
                }
                else
                {
                    // Keep holding the remainder
                }

                selectionRequested = true;

                return;
            }

            if (InventoryService.CanStackTogether(slot.Item, HeldItem))
            {
                int availableSpace = maxStackSize - slot.Count;

                if (availableSpace <= 0)
                {
                    return;
                }

                int movedCount = HeldCount > availableSpace ? availableSpace : HeldCount;
                slot.Count += movedCount;
                HeldCount -= movedCount;

                if (HeldCount == 0)
                {
                    ClearHeldItem();
                }

                selectionRequested = true;

                return;
            }

            // Can't swap if held count exceeds max stack size for this slot
            if (HeldCount > maxStackSize)
            {
                return;
            }

            AType swapItem = slot.Item;
            int swapCount = slot.Count;
            slot.Item = HeldItem;
            slot.Count = HeldCount;
            HeldItem = swapItem;
            HeldCount = swapCount;
            heldItemReturnTarget = returnTarget;
            selectionRequested = true;
        }

        private void PlaceSingleHeldItem(AGridBox slot)
        {
            PlaceSingleHeldItemWithMaxSize(slot, currentMaxStackSize);
        }

        private void PickupHalfStack(AGridBox slot, HeldItemReturnTarget returnTarget)
        {
            PickupHalfStackWithMaxSize(slot, returnTarget);
        }

        private void PickupHalfStackWithMaxSize(AGridBox slot, HeldItemReturnTarget returnTarget)
        {
            if (slot.Item == null || slot.Count <= 0) return;

            int halfCount = (slot.Count + 1) / 2;
            HeldItem = slot.Item;
            HeldCount = halfCount;
            heldItemReturnTarget = returnTarget;
            slot.Count -= halfCount;

            if (slot.Count <= 0)
            {
                slot.Item = null;
                slot.Count = 0;
            }

            selectionRequested = true;
        }

        private void PlaceSingleHeldItemWithMaxSize(AGridBox slot, int maxStackSize)
        {
            if (HeldItem == null || HeldCount <= 0)
            {
                return;
            }

            if (slot.Item == null || slot.Count <= 0)
            {
                slot.Item = HeldItem;
                slot.Count = 1;
                HeldCount -= 1;

                if (HeldCount == 0)
                {
                    ClearHeldItem();
                }

                selectionRequested = true;

                return;
            }

            if (!InventoryService.CanStackTogether(HeldItem, slot.Item))
            {
                return;
            }

            if (slot.Count >= maxStackSize)
            {
                return;
            }

            slot.Count += 1;
            HeldCount -= 1;

            if (HeldCount == 0)
            {
                ClearHeldItem();
            }

            selectionRequested = true;
        }

        private void ClearHeldItem()
        {
            HeldItem = null;
            HeldCount = 0;
            heldItemReturnTarget = HeldItemReturnTarget.Inventory;
        }

        private bool TryEquipHeldItem(ModelWorld world, InventoryItemUseService itemUseService, EPlayerEquipmentSlotType equipmentSlot)
        {
            AType heldItem = HeldItem;
            int heldCount = HeldCount;

            if (!itemUseService.TryEquipFromHeld(world, equipmentSlot, ref heldItem, ref heldCount))
            {
                return false;
            }

            HeldItem = heldItem;
            HeldCount = heldCount;
            selectionRequested = true;

            return true;
        }

        private static bool TryGetClickedEquipmentSlot(Point position, InventoryLayout layout, out EPlayerEquipmentSlotType slotType)
        {
            foreach (EPlayerEquipmentSlotType candidate in Enum.GetValues<EPlayerEquipmentSlotType>())
            {
                if (layout.GetEquipmentSlotRectangle(candidate).Contains(position))
                {
                    slotType = candidate;

                    return true;
                }
            }

            slotType = default;

            return false;
        }

        private static bool TryGetClickedSlot(Point position, InventoryInteractionContext ctx, out AGridBox slot)
        {
            if (!ctx.BlockCrafting && TryGetClickedSlot(ctx.CraftingGrid.InternalGrid, ctx.Layout.CraftingStart.X, ctx.Layout.CraftingStart.Y, ctx.Layout.SlotSize, ctx.Layout.SlotSpacing, position, out slot))
            {
                return true;
            }

            if (!ctx.BlockCrafting && ctx.Layout.OutputSlotRectangle.Contains(position))
            {
                slot = ctx.CraftOutputSlot;
                return true;
            }

            if (TryGetClickedInventorySlot(ctx.InventoryGrid, ctx.Layout, position, ctx.InventoryScrollOffset, out slot))
            {
                return true;
            }

            if (ctx.World.Player.HasGadgetBelt)
            {
                for (int x = 0; x < GadgetBarLayout.TotalSlotCount; x++)
                {
                    if (!GadgetBarLayout.GetSlotRectangle(ctx.ViewportWidth, ctx.ViewportHeight, x).Contains(position)) continue;
                    slot = ctx.World.Player.GadgetSlots.Items.InternalGrid[x, 0];
                    return true;
                }
            }

            slot = null;
            return false;
        }

        private static bool TryGetClickedInventorySlot(AGridBox[,] grid, InventoryLayout layout, Point position, int scrollRowOffset, out AGridBox slot)
        {
            slot = null;
            int columns = grid.GetLength(0);
            int totalRows = grid.GetLength(1);
            int slotStep = layout.SlotSize + layout.SlotSpacing;

            // Check if position is within inventory section
            if (!layout.InventorySectionRectangle.Contains(position)) return false;

            // Calculate which slot was clicked based on position relative to inventory start
            int relativeX = position.X - layout.InventoryStart.X;
            int relativeY = position.Y - layout.InventoryStart.Y;

            if (relativeX < 0 || relativeY < 0) return false;

            int slotX = relativeX / slotStep;
            int visibleSlotY = relativeY / slotStep;

            // Check if click is within slot bounds (not in spacing)
            if (relativeX % slotStep >= layout.SlotSize || relativeY % slotStep >= layout.SlotSize) return false;
            if (slotX >= columns) return false;

            int actualY = scrollRowOffset + visibleSlotY;
            if (actualY >= totalRows) return false;

            slot = grid[slotX, actualY];
            return true;
        }

        private static bool TryGetClickedSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot)
        {
            return UiGridHitTestHelper.TryGetSlot(grid, startX, startY, slotSize, slotSpacing, position, out slot);
        }

    }
}
