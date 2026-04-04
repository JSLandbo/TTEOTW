using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract;
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

        public void Update(
            MouseState currentMouseState,
            MouseState previousMouseState,
            InventoryLayout layout,
            AGridBox[,] inventoryGrid,
            Grid craftingGrid,
            GridBox craftOutputSlot,
            CraftingService craftingService,
            ModelWorld world,
            InventoryItemUseService itemUseService,
            AInventory inventory,
            int viewportWidth,
            int viewportHeight,
            bool blockCrafting = false)
        {
            MousePosition = currentMouseState.Position;
            currentMaxStackSize = inventory.MaxStackSize > 0 ? inventory.MaxStackSize : InventoryService.DefaultMaxStackSize;

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (!blockCrafting && layout.CraftButtonRectangle.Contains(MousePosition))
                {
                    craftingService.TryCraft(craftingGrid.InternalGrid, craftOutputSlot, currentMaxStackSize);

                    return;
                }

                if (HeldItem != null && layout.TrashBinRectangle.Contains(MousePosition))
                {
                    trashRequested = true;
                    ClearHeldItem();

                    return;
                }

                if (HeldItem != null && TryGetClickedEquipmentSlot(MousePosition, layout, out EPlayerEquipmentSlotType equipmentSlot) && TryEquipHeldItem(world, itemUseService, equipmentSlot))
                {
                    return;
                }

                if (TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, world.Player, viewportWidth, viewportHeight, blockCrafting, out AGridBox clickedSlot)
                    && CanUseClickedSlot(clickedSlot))
                {
                    MoveStack(clickedSlot);
                }

                return;
            }

            if (HeldItem != null
                && UiInputHelper.WasRightClicked(currentMouseState, previousMouseState)
                && TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, world.Player, viewportWidth, viewportHeight, blockCrafting, out AGridBox rightClickedSlot)
                && CanUseClickedSlot(rightClickedSlot))
            {
                PlaceSingleHeldItem(rightClickedSlot);
            }
        }

        public void ReleaseHeldItem(InventoryService inventoryService, AInventory inventory, AGadgetInventory gadgetSlots)
        {
            if (HeldItem == null || HeldCount <= 0)
            {
                return;
            }

            if (TryPlaceItem(inventoryService, inventory, gadgetSlots, HeldItem, HeldCount))
            {
                ClearHeldItem();
                return;
            }

            // Fallback: try to return to source slot
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

            if (InventoryService.CanStackTogether(heldSourceSlot.Item, HeldItem) && heldSourceSlot.Count + HeldCount <= currentMaxStackSize)
            {
                heldSourceSlot.Count += HeldCount;
                ClearHeldItem();
            }
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

        public bool IsPointerOverInteractiveElement(Point position, InventoryLayout layout, AGridBox[,] inventoryGrid, Grid craftingGrid, GridBox craftOutputSlot, APlayer player, InventoryItemUseService itemUseService, int viewportWidth, int viewportHeight, bool blockCrafting = false)
        {
            if (!blockCrafting && layout.CraftButtonRectangle.Contains(position))
            {
                return true;
            }

            if (layout.SelfDestructButtonRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem == null && layout.SortButtonRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem != null && layout.TrashBinRectangle.Contains(position))
            {
                return true;
            }

            if (HeldItem != null && TryGetClickedEquipmentSlot(position, layout, out EPlayerEquipmentSlotType equipmentSlot) && itemUseService.CanEquip(HeldItem, equipmentSlot))
            {
                return true;
            }

            return TryGetClickedSlot(position, inventoryGrid, layout, craftingGrid, craftOutputSlot, player, viewportWidth, viewportHeight, blockCrafting, out AGridBox clickedSlot)
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
                slot.Item = HeldItem;
                slot.Count = HeldCount;

                ClearHeldItem();
                selectionRequested = true;

                return;
            }

            if (InventoryService.CanStackTogether(slot.Item, HeldItem))
            {
                int availableSpace = currentMaxStackSize - slot.Count;

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

            if (slot.Count >= currentMaxStackSize)
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

        private static bool TryGetClickedSlot(Point position, AGridBox[,] inventoryGrid, InventoryLayout layout, Grid craftingGrid, GridBox craftOutputSlot, APlayer player, int viewportWidth, int viewportHeight, bool blockCrafting, out AGridBox slot)
        {
            if (!blockCrafting && TryGetClickedSlot(craftingGrid.InternalGrid, layout.CraftingStart.X, layout.CraftingStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                return true;
            }

            if (!blockCrafting && layout.OutputSlotRectangle.Contains(position))
            {
                slot = craftOutputSlot;

                return true;
            }

            if (TryGetClickedSlot(inventoryGrid, layout.InventoryStart.X, layout.InventoryStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                return true;
            }

            if (player.HasGadgetBelt)
            {
                for (int x = 0; x < GadgetBarLayout.TotalSlotCount; x++)
                {
                    if (!GadgetBarLayout.GetSlotRectangle(viewportWidth, viewportHeight, x).Contains(position))
                    {
                        continue;
                    }

                    slot = player.GadgetSlots.Items.InternalGrid[x, 0];

                    return true;
                }
            }

            slot = null;

            return false;
        }

        private static bool TryGetClickedSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot)
        {
            return UiGridHitTestHelper.TryGetSlot(grid, startX, startY, slotSize, slotSpacing, position, out slot);
        }

    }
}
