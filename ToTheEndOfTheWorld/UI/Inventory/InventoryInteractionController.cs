using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.World;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryInteractionController
    {
        private AGridBox heldSourceSlot;
        private int currentMaxStackSize = InventoryService.DefaultMaxStackSize;
        private bool selectionRequested;
        private bool trashRequested;

        public AType HeldItem { get; private set; }
        public int HeldCount { get; private set; }
        public Point MousePosition { get; private set; }
        public bool HasHeldItem => HeldItem != null && HeldCount > 0;

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
                    var chestResult = ctx.TryGetChestSlot(MousePosition);
                    if (chestResult.HasValue && CanUseClickedSlot(chestResult.Value.slot))
                    {
                        MoveStackWithMaxSize(chestResult.Value.slot, chestResult.Value.maxStackSize);
                        return;
                    }
                }

                if (TryGetClickedSlot(MousePosition, ctx, out AGridBox clickedSlot)
                    && CanUseClickedSlot(clickedSlot))
                {
                    // CTRL+click to sell when shop is open
                    if (ctx.TrySellSlotFunc != null && clickedSlot.Item != null && ctx.TrySellSlot(clickedSlot))
                    {
                        selectionRequested = true;
                        return;
                    }

                    MoveStack(clickedSlot);
                }

                return;
            }

            if (UiInputHelper.WasRightClicked(currentMouseState, previousMouseState))
            {
                // Check chest slots for right-click
                if (ctx.TryGetChestSlotFunc != null)
                {
                    var chestResult = ctx.TryGetChestSlot(MousePosition);
                    if (chestResult.HasValue && CanUseClickedSlot(chestResult.Value.slot))
                    {
                        if (HeldItem != null)
                            PlaceSingleHeldItemWithMaxSize(chestResult.Value.slot, chestResult.Value.maxStackSize);
                        else
                            PickupHalfStackWithMaxSize(chestResult.Value.slot);
                        return;
                    }
                }

                if (TryGetClickedSlot(MousePosition, ctx, out AGridBox rightClickedSlot) && CanUseClickedSlot(rightClickedSlot))
                {
                    if (HeldItem != null)
                        PlaceSingleHeldItem(rightClickedSlot);
                    else
                        PickupHalfStack(rightClickedSlot);
                }
            }
        }

        public void ReleaseHeldItem(InventoryService inventoryService, AInventory inventory, AGadgetInventory gadgetSlots)
        {
            if (HeldItem == null || HeldCount <= 0)
            {
                return;
            }

            // 1) Try inventory + gadget slots
            if (TryPlaceItem(inventoryService, inventory, gadgetSlots, HeldItem, HeldCount))
            {
                ClearHeldItem();
                return;
            }

            // 2) Try to return to source slot
            if (heldSourceSlot != null)
            {
                if (heldSourceSlot.Item == null || heldSourceSlot.Count <= 0)
                {
                    heldSourceSlot.Item = HeldItem;
                    heldSourceSlot.Count = HeldCount;
                    ClearHeldItem();
                    return;
                }

                if (InventoryService.CanStackTogether(heldSourceSlot.Item, HeldItem) && heldSourceSlot.Count + HeldCount <= currentMaxStackSize)
                {
                    heldSourceSlot.Count += HeldCount;
                    ClearHeldItem();
                    return;
                }
            }

            // 3) Give up - caller handles remaining item
        }

        public (AType Item, int Count) GetAndClearHeldItem()
        {
            var result = (HeldItem, HeldCount);
            ClearHeldItem();
            return result;
        }

        public void ReturnCraftingGridToInventory(InventoryService inventoryService, AInventory inventory, Grid craftingGrid, AGadgetInventory gadgetSlots)
        {
            foreach (AGridBox slot in InventoryService.EnumerateSlots(craftingGrid.InternalGrid))
            {
                if (slot.Item == null || slot.Count <= 0) continue;

                if (TryPlaceItem(inventoryService, inventory, gadgetSlots, slot.Item, slot.Count))
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
                // Items that couldn't be placed stay in crafting grid for caller to handle
            }
        }

        public static void ClearCraftingGrid(Grid craftingGrid, Func<AType, int, int> addToChest)
        {
            foreach (AGridBox slot in InventoryService.EnumerateSlots(craftingGrid.InternalGrid))
            {
                if (slot.Item == null || slot.Count <= 0) continue;

                addToChest?.Invoke(slot.Item, slot.Count);
                slot.Item = null;
                slot.Count = 0;
            }
        }

        private static bool TryPlaceItem(InventoryService svc, AInventory inv, AGadgetInventory gadget, AType item, int count)
        {
            // 1) Try inventory
            if (svc.TryAdd(inv, item, count)) return true;

            // 2) Try gadget slots
            if (gadget != null && svc.TryAdd(gadget, item, count)) return true;

            // 3) Sort inventory, try again
            svc.SortByName(inv);
            if (svc.TryAdd(inv, item, count)) return true;

            // 4) Sort gadget slots (only consumeable slots 0-3), try again
            if (gadget != null)
            {
                SortConsumeableSlots(svc, gadget);
                if (svc.TryAdd(gadget, item, count)) return true;
            }

            // 5) Move items from inventory to gadget slots to free space
            if (gadget != null)
            {
                MoveInventoryToGadgetSlots(svc, inv, gadget);
                if (svc.TryAdd(inv, item, count)) return true;
            }

            // 6) Give up
            return false;
        }

        private static void SortConsumeableSlots(InventoryService svc, AGadgetInventory gadget)
        {
            // Consolidate consumeable stacks in slots 0-3
            AGridBox[,] grid = gadget.Items.InternalGrid;
            int maxStack = svc.GetMaxStackSize(gadget);
            int consumeableSlots = Math.Min(4, grid.GetLength(0));

            for (int i = 0; i < consumeableSlots; i++)
            {
                AGridBox slot = grid[i, 0];
                if (slot.Item == null || slot.Count >= maxStack) continue;

                // Find matching items in later slots to merge
                for (int j = i + 1; j < consumeableSlots; j++)
                {
                    AGridBox other = grid[j, 0];
                    if (!InventoryService.CanStackTogether(slot.Item, other.Item)) continue;

                    int space = maxStack - slot.Count;
                    int toMove = Math.Min(space, other.Count);
                    slot.Count += toMove;
                    other.Count -= toMove;
                    if (other.Count <= 0) other.Item = null;
                    if (slot.Count >= maxStack) break;
                }
            }
        }

        private static void MoveInventoryToGadgetSlots(InventoryService svc, AInventory inv, AGadgetInventory gadget)
        {
            // Try to move consumeables from inventory to gadget slots (only consumeables can go in slots 0-3)
            foreach (AGridBox slot in InventoryService.EnumerateSlots(inv.Items.InternalGrid))
            {
                if (slot.Item is not AConsumeable || slot.Count <= 0) continue;
                if (svc.TryAdd(gadget, slot.Item, slot.Count))
                {
                    slot.Item = null;
                    slot.Count = 0;
                }
            }
        }

        public void ClearHeldItemState()
        {
            ClearHeldItem();
        }

        public bool IsPointerOverInteractiveElement(Point position, InventoryInteractionContext ctx)
        {
            if (!ctx.BlockCrafting && ctx.Layout.CraftButtonRectangle.Contains(position))
            {
                return true;
            }

            if (ctx.Layout.SelfDestructButtonRectangle.Contains(position))
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

        private void MoveStack(AGridBox slot)
        {
            MoveStackWithMaxSize(slot, currentMaxStackSize);
        }

        private void MoveStackWithMaxSize(AGridBox slot, int maxStackSize)
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
            heldSourceSlot = slot;
            selectionRequested = true;
        }

        private void PlaceSingleHeldItem(AGridBox slot)
        {
            PlaceSingleHeldItemWithMaxSize(slot, currentMaxStackSize);
        }

        private void PickupHalfStack(AGridBox slot)
        {
            PickupHalfStackWithMaxSize(slot);
        }

        private void PickupHalfStackWithMaxSize(AGridBox slot)
        {
            if (slot.Item == null || slot.Count <= 0) return;

            int halfCount = (slot.Count + 1) / 2;
            HeldItem = slot.Item;
            HeldCount = halfCount;
            heldSourceSlot = slot;
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
            heldSourceSlot = null;
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
