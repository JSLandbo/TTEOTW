using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.World;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryHoverLabelResolver
    {
        public string Resolve(
            ModelWorld world,
            Point mousePosition,
            InventoryLayout layout,
            Grid craftingGrid,
            GridBox craftOutputSlot,
            InventoryItemUseService itemUseService,
            int viewportWidth,
            int viewportHeight)
        {
            if (layout.SelfDestructButtonRectangle.Contains(mousePosition))
            {
                return "Self Destruct";
            }

            if (layout.TrashBinRectangle.Contains(mousePosition))
            {
                return "Trash";
            }

            if (layout.SortButtonRectangle.Contains(mousePosition))
            {
                return "Sort";
            }

            if (TryGetEquipmentHoverLabel(world, itemUseService, layout, mousePosition, out string equipmentHoverLabel))
            {
                return equipmentHoverLabel;
            }

            if (TryGetGridHoverLabel(craftingGrid.InternalGrid, layout.CraftingStart.X, layout.CraftingStart.Y, layout.SlotSize, layout.SlotSpacing, mousePosition, out string craftingHoverLabel))
            {
                return craftingHoverLabel;
            }

            if (layout.OutputSlotRectangle.Contains(mousePosition))
            {
                return craftOutputSlot.Item?.Name;
            }

            if (TryGetGridHoverLabel(world.Player.Inventory.Items.InternalGrid, layout.InventoryStart.X, layout.InventoryStart.Y, layout.SlotSize, layout.SlotSpacing, mousePosition, out string inventoryHoverLabel))
            {
                return inventoryHoverLabel;
            }

            if (world.Player.HasGadgetBelt)
            {
                for (int x = 0; x < GadgetBarLayout.TotalSlotCount; x++)
                {
                    if (!GadgetBarLayout.GetSlotRectangle(viewportWidth, viewportHeight, x).Contains(mousePosition))
                    {
                        continue;
                    }

                    AGridBox slot = world.Player.GadgetSlots.Items.InternalGrid[x, 0];
                    return slot.Item?.Name ?? $"Gadget Slot {x + 1}";
                }
            }

            return null;
        }

        private static bool TryGetEquipmentHoverLabel(ModelWorld world, InventoryItemUseService itemUseService, InventoryLayout layout, Point mousePosition, out string hoverLabel)
        {
            foreach (EPlayerEquipmentSlotType slotType in Enum.GetValues<EPlayerEquipmentSlotType>())
            {
                if (!layout.GetEquipmentSlotRectangle(slotType).Contains(mousePosition))
                {
                    continue;
                }

                hoverLabel = itemUseService.GetEquippedItem(world, slotType)?.Name;
                return true;
            }

            hoverLabel = null;
            return false;
        }

        private static bool TryGetGridHoverLabel(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point mousePosition, out string hoverLabel)
        {
            bool found = UiGridHitTestHelper.TryGetSlot(grid, startX, startY, slotSize, slotSpacing, mousePosition, out AGridBox slot);
            hoverLabel = found ? slot.Item?.Name : null;
            return found;
        }
    }
}
