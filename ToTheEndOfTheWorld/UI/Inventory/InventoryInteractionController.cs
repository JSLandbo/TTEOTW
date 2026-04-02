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

        public AType HeldItem { get; private set; }
        public int HeldCount { get; private set; }
        public Point MousePosition { get; private set; }
        public bool HasHeldItem => HeldItem != null && HeldCount > 0;

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
            int viewportHeight)
        {
            MousePosition = currentMouseState.Position;
            currentMaxStackSize = inventory.MaxStackSize > 0 ? inventory.MaxStackSize : InventoryService.DefaultMaxStackSize;

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (layout.CraftButtonRectangle.Contains(MousePosition))
                {
                    craftingService.TryCraft(craftingGrid.InternalGrid, craftOutputSlot, currentMaxStackSize);

                    return;
                }

                if (HeldItem != null && layout.TrashBinRectangle.Contains(MousePosition))
                {
                    ClearHeldItem();

                    return;
                }

                if (HeldItem != null && TryGetClickedEquipmentSlot(MousePosition, layout, out EPlayerEquipmentSlotType equipmentSlot) && TryEquipHeldItem(world, itemUseService, equipmentSlot))
                {
                    return;
                }

                if (TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, world.Player, viewportWidth, viewportHeight, out AGridBox clickedSlot, out int gadgetSlotIndex)
                    && (HeldItem == null || gadgetSlotIndex < 0 || world.Player.GadgetSlots.CanPlaceInSlot(gadgetSlotIndex, HeldItem)))
                {
                    MoveStack(clickedSlot);
                }

                return;
            }

            if (HeldItem != null
                && UiInputHelper.WasRightClicked(currentMouseState, previousMouseState)
                && TryGetClickedSlot(MousePosition, inventoryGrid, layout, craftingGrid, craftOutputSlot, world.Player, viewportWidth, viewportHeight, out AGridBox rightClickedSlot, out int rightClickGadgetSlotIndex)
                && (rightClickGadgetSlotIndex < 0 || world.Player.GadgetSlots.CanPlaceInSlot(rightClickGadgetSlotIndex, HeldItem)))
            {
                PlaceSingleHeldItem(rightClickedSlot);
            }
        }

        public void ReleaseHeldItem(InventoryService inventoryService, AInventory inventory)
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

            if (InventoryService.CanStackTogether(heldSourceSlot.Item, HeldItem) && heldSourceSlot.Count + HeldCount <= currentMaxStackSize)
            {
                heldSourceSlot.Count += HeldCount;
                ClearHeldItem();
            }
        }

        public void ReturnCraftingGridToInventory(InventoryService inventoryService, AInventory inventory, Grid craftingGrid)
        {
            AGridBox[,] grid = craftingGrid.InternalGrid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

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

        public void ClearHeldItemState()
        {
            ClearHeldItem();
        }

        public bool IsPointerOverInteractiveElement(Point position, InventoryLayout layout, AGridBox[,] inventoryGrid, Grid craftingGrid, GridBox craftOutputSlot, APlayer player, InventoryItemUseService itemUseService, int viewportWidth, int viewportHeight)
        {
            if (layout.CraftButtonRectangle.Contains(position))
            {
                return true;
            }

            if (layout.SelfDestructButtonRectangle.Contains(position))
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

            return TryGetClickedSlot(position, inventoryGrid, layout, craftingGrid, craftOutputSlot, player, viewportWidth, viewportHeight, out AGridBox clickedSlot, out int gadgetSlotIndex)
                && UiSlotInteractionHelper.CanInteractWithSlot(clickedSlot, HasHeldItem)
                && (!HasHeldItem || gadgetSlotIndex < 0 || player.GadgetSlots.CanPlaceInSlot(gadgetSlotIndex, HeldItem));
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

                return;
            }

            AType swapItem = slot.Item;
            int swapCount = slot.Count;
            slot.Item = HeldItem;
            slot.Count = HeldCount;
            HeldItem = swapItem;
            HeldCount = swapCount;
            heldSourceSlot = slot;
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

            return true;
        }

        private static bool TryGetClickedEquipmentSlot(Point position, InventoryLayout layout, out EPlayerEquipmentSlotType slotType)
        {
            foreach (EPlayerEquipmentSlotType candidate in new[]
            {
                EPlayerEquipmentSlotType.ThermalPlating,
                EPlayerEquipmentSlotType.Inventory,
                EPlayerEquipmentSlotType.FuelTank,
                EPlayerEquipmentSlotType.Drill,
                EPlayerEquipmentSlotType.Hull,
                EPlayerEquipmentSlotType.Engine,
                EPlayerEquipmentSlotType.Thruster
            })
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

        private static bool TryGetClickedSlot(Point position, AGridBox[,] inventoryGrid, InventoryLayout layout, Grid craftingGrid, GridBox craftOutputSlot, APlayer player, int viewportWidth, int viewportHeight, out AGridBox slot, out int gadgetSlotIndex)
        {
            if (TryGetClickedSlot(craftingGrid.InternalGrid, layout.CraftingStart.X, layout.CraftingStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                gadgetSlotIndex = -1;
                return true;
            }

            if (layout.OutputSlotRectangle.Contains(position))
            {
                slot = craftOutputSlot;
                gadgetSlotIndex = -1;

                return true;
            }

            if (TryGetClickedSlot(inventoryGrid, layout.InventoryStart.X, layout.InventoryStart.Y, layout.SlotSize, layout.SlotSpacing, position, out slot))
            {
                gadgetSlotIndex = -1;
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
                    gadgetSlotIndex = x;

                    return true;
                }
            }

            slot = null;
            gadgetSlotIndex = -1;

            return false;
        }

        private static bool TryGetClickedSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Rectangle slotRectangle = new(
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

    }
}
