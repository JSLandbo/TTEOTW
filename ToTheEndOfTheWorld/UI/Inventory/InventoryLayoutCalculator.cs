using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public static class InventoryLayoutCalculator
    {
        public const int SlotSize = 64;
        public const int SlotSpacing = 8;
        public const int VisibleInventoryRows = 6;
        private const int PanelPadding = 18;
        private const int TitleHeight = 56;
        private const int HeaderBottomGap = 14;
        private const int SectionGap = 18;
        private const int OutputGap = 18;
        private const int EquipmentGap = 24;
        private const int SectionPadding = 16;
        private const int DividerHeight = 1;

        public static InventoryLayout Create(int viewportWidth, int viewportHeight, AGridBox[,] inventoryGrid, int panelXOffset = 0)
        {
            int craftingColumns = 3;
            int craftingRows = 3;
            int inventoryColumns = inventoryGrid.GetLength(0);
            int totalInventoryRows = inventoryGrid.GetLength(1);
            int visibleRows = Math.Min(totalInventoryRows, VisibleInventoryRows);

            int craftingSectionWidth = (craftingColumns * SlotSize) + ((craftingColumns - 1) * SlotSpacing);
            int inventorySectionWidth = (inventoryColumns * SlotSize) + ((inventoryColumns - 1) * SlotSpacing);
            int craftButtonWidth = SlotSize + 28;
            int outputColumnWidth = SlotSize + 12;
            int equipmentSectionWidth = (SlotSize * 3) + (SlotSpacing * 2);
            int topSectionWidth = craftingSectionWidth + OutputGap + outputColumnWidth + EquipmentGap + equipmentSectionWidth;
            int contentWidth = Math.Max(topSectionWidth, inventorySectionWidth);

            int craftingHeight = (craftingRows * SlotSize) + ((craftingRows - 1) * SlotSpacing);
            int inventoryHeight = (visibleRows * SlotSize) + ((visibleRows - 1) * SlotSpacing);
            int equipmentSectionHeight = (SlotSize * 3) + (SlotSpacing * 2);
            int topContentHeight = Math.Max(craftingHeight, equipmentSectionHeight);

            int craftingSectionHeight = (SectionPadding * 2) + topContentHeight;
            int inventorySectionHeight = (SectionPadding * 2) + inventoryHeight;
            int contentAreaWidth = contentWidth + (SectionPadding * 2);
            int inventoryAreaWidth = inventorySectionWidth + (SectionPadding * 2);
            int panelWidth = (PanelPadding * 2) + Math.Max(contentAreaWidth, inventoryAreaWidth);
            int panelHeight = TitleHeight + (PanelPadding * 2) + HeaderBottomGap + craftingSectionHeight + SectionGap + DividerHeight + SectionGap + inventorySectionHeight;

            int panelX = ((viewportWidth - panelWidth) / 2) + panelXOffset;
            int panelY = (viewportHeight - panelHeight) / 2;

            Rectangle headerRectangle = new(panelX, panelY, panelWidth, TitleHeight);
            Rectangle craftingSectionRectangle = new(panelX + PanelPadding, panelY + TitleHeight + HeaderBottomGap, panelWidth - (PanelPadding * 2), craftingSectionHeight);

            int craftingStartX = craftingSectionRectangle.X + ((craftingSectionRectangle.Width - contentWidth) / 2);
            int craftingStartY = craftingSectionRectangle.Y + SectionPadding + ((topContentHeight - craftingHeight) / 2);
            int outputSlotY = craftingStartY + SlotSize + SlotSpacing;

            Rectangle craftButtonRectangle = new(craftingStartX + craftingSectionWidth + OutputGap + 11 - ((craftButtonWidth - SlotSize) / 2), outputSlotY + SlotSize + 23, craftButtonWidth + 8, 40);
            int outputSlotX = craftButtonRectangle.Center.X - (SlotSize / 2);
            int equipmentStartX = outputSlotX + outputColumnWidth + EquipmentGap;
            int equipmentStartY = craftingSectionRectangle.Y + SectionPadding + ((topContentHeight - equipmentSectionHeight) / 2);

            Rectangle equipmentSectionRectangle = new(equipmentStartX, craftingSectionRectangle.Y + SectionPadding, equipmentSectionWidth, equipmentSectionHeight);
            int dividerY = craftingSectionRectangle.Bottom + SectionGap;
            Rectangle dividerRectangle = new(panelX + PanelPadding + 8, dividerY, panelWidth - (PanelPadding * 2) - 16, DividerHeight);
            Rectangle inventorySectionRectangle = new(panelX + PanelPadding, dividerRectangle.Bottom + SectionGap, panelWidth - (PanelPadding * 2), inventorySectionHeight);
            Rectangle selfDestructButtonRectangle = new(16, viewportHeight - SlotSize - 16, SlotSize, SlotSize);

            int inventoryStartX = inventorySectionRectangle.X + ((inventorySectionRectangle.Width - inventorySectionWidth) / 2);
            int inventoryStartY = inventorySectionRectangle.Y + SectionPadding;

            Rectangle trashBinRectangle = new(panelX + panelWidth - SlotSize + 86, panelY + panelHeight - SlotSize, SlotSize, SlotSize);
            Rectangle sortButtonRectangle = new(trashBinRectangle.X, trashBinRectangle.Y - SlotSize - 10, SlotSize, SlotSize);

            return new InventoryLayout(
                new Rectangle(panelX, panelY, panelWidth, panelHeight),
                headerRectangle,
                craftingSectionRectangle,
                equipmentSectionRectangle,
                Rectangle.Empty,
                inventorySectionRectangle,
                dividerRectangle,
                selfDestructButtonRectangle,
                new Point(craftingStartX, craftingStartY),
                new Point(inventoryStartX, inventoryStartY),
                sortButtonRectangle,
                trashBinRectangle,
                new Rectangle(outputSlotX, outputSlotY, SlotSize, SlotSize),
                craftButtonRectangle,
                new Rectangle(equipmentStartX + SlotSize + SlotSpacing, equipmentStartY, SlotSize, SlotSize),
                new Rectangle(equipmentStartX + SlotSize + SlotSpacing, equipmentStartY + SlotSize + SlotSpacing, SlotSize, SlotSize),
                new Rectangle(equipmentStartX, equipmentStartY + SlotSize + SlotSpacing, SlotSize, SlotSize),
                new Rectangle(equipmentStartX + (SlotSize + SlotSpacing) * 2, equipmentStartY + SlotSize + SlotSpacing, SlotSize, SlotSize),
                new Rectangle(equipmentStartX + (SlotSize + SlotSpacing) * 2, equipmentStartY + (SlotSize + SlotSpacing) * 2, SlotSize, SlotSize),
                new Rectangle(equipmentStartX, equipmentStartY + (SlotSize + SlotSpacing) * 2, SlotSize, SlotSize),
                new Rectangle(equipmentStartX + SlotSize + SlotSpacing, equipmentStartY + (SlotSize + SlotSpacing) * 2, SlotSize, SlotSize),
                SlotSize,
                SlotSpacing
            );
        }
    }
}
