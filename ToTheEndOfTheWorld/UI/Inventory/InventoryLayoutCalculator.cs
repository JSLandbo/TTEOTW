using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using System;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public static class InventoryLayoutCalculator
    {
        public static InventoryLayout Create(int viewportWidth, int viewportHeight, AGridBox[,] inventoryGrid)
        {
            const int panelPadding = 18;
            const int slotSpacing = 8;
            const int titleHeight = 56;
            const int headerBottomGap = 14;
            const int sectionGap = 18;
            const int outputGap = 18;
            const int equipmentGap = 24;
            const int equipmentInfoGap = 20;
            const int equipmentInfoWidth = 500;
            const int sectionPadding = 16;
            const int dividerHeight = 1;
            var craftingColumns = 3;
            var craftingRows = 3;
            var inventoryColumns = inventoryGrid.GetLength(0);
            var inventoryRows = inventoryGrid.GetLength(1);
            var topColumns = craftingColumns + 7;
            var maxColumns = topColumns > inventoryColumns ? topColumns : inventoryColumns;
            var totalRows = craftingRows + inventoryRows;
            var availableWidth = viewportWidth - 128 - (panelPadding * 2) - ((maxColumns - 1) * slotSpacing);
            var availableHeight = viewportHeight - 170 - titleHeight - (sectionGap * 2) - (panelPadding * 2) - ((totalRows - 2) * slotSpacing);
            var slotSize = Math.Max(42, Math.Min(72, Math.Min(availableWidth / maxColumns, availableHeight / totalRows)));
            var craftingSectionWidth = (craftingColumns * slotSize) + ((craftingColumns - 1) * slotSpacing);
            var inventorySectionWidth = (inventoryColumns * slotSize) + ((inventoryColumns - 1) * slotSpacing);
            var craftButtonWidth = slotSize + 20;
            var outputColumnWidth = slotSize + 12;
            var equipmentSectionWidth = (slotSize * 3) + (slotSpacing * 2);
            var topSectionWidth = craftingSectionWidth + outputGap + outputColumnWidth + equipmentGap + equipmentSectionWidth + equipmentInfoGap + equipmentInfoWidth;
            var contentWidth = topSectionWidth > inventorySectionWidth ? topSectionWidth : inventorySectionWidth;
            var craftingHeight = (craftingRows * slotSize) + ((craftingRows - 1) * slotSpacing);
            var inventoryHeight = (inventoryRows * slotSize) + ((inventoryRows - 1) * slotSpacing);
            var equipmentSectionHeight = (slotSize * 3) + (slotSpacing * 2);
            var topContentHeight = craftingHeight > equipmentSectionHeight ? craftingHeight : equipmentSectionHeight;
            var craftingSectionHeight = (sectionPadding * 2) + topContentHeight;
            var inventorySectionHeight = (sectionPadding * 2) + inventoryHeight;
            var contentAreaWidth = contentWidth + (sectionPadding * 2);
            var inventoryAreaWidth = inventorySectionWidth + (sectionPadding * 2);
            var panelWidth = (panelPadding * 2) + (contentAreaWidth > inventoryAreaWidth ? contentAreaWidth : inventoryAreaWidth);
            var panelHeight = titleHeight
                              + (panelPadding * 2)
                              + headerBottomGap
                              + craftingSectionHeight
                              + sectionGap
                              + dividerHeight
                              + sectionGap
                              + inventorySectionHeight;
            var panelX = (viewportWidth - panelWidth) / 2;
            var panelY = (viewportHeight - panelHeight) / 2;
            var headerRectangle = new Rectangle(panelX, panelY, panelWidth, titleHeight);
            var craftingSectionRectangle = new Rectangle(
                panelX + panelPadding,
                panelY + titleHeight + headerBottomGap,
                panelWidth - (panelPadding * 2),
                craftingSectionHeight);
            var craftingStartX = craftingSectionRectangle.X + ((craftingSectionRectangle.Width - contentWidth) / 2);
            var craftingStartY = craftingSectionRectangle.Y + sectionPadding + ((topContentHeight - craftingHeight) / 2);
            var outputSlotY = craftingStartY + slotSize + slotSpacing;
            var craftButtonRectangle = new Rectangle(
                craftingStartX + craftingSectionWidth + outputGap + 11 - ((craftButtonWidth - slotSize) / 2),
                outputSlotY + slotSize + 23,
                craftButtonWidth + 8,
                40);
            var outputSlotX = craftButtonRectangle.Center.X - (slotSize / 2);
            var equipmentStartX = outputSlotX + outputColumnWidth + equipmentGap;
            var equipmentStartY = craftingSectionRectangle.Y + sectionPadding + ((topContentHeight - equipmentSectionHeight) / 2);
            var equipmentInfoRectangle = new Rectangle(equipmentStartX + equipmentSectionWidth + equipmentInfoGap, craftingSectionRectangle.Y + sectionPadding, equipmentInfoWidth, topContentHeight);
            var equipmentSectionRectangle = new Rectangle(equipmentStartX, craftingSectionRectangle.Y + sectionPadding, equipmentInfoRectangle.Right - equipmentStartX, topContentHeight);
            var dividerY = craftingSectionRectangle.Bottom + sectionGap;
            var dividerRectangle = new Rectangle(panelX + panelPadding + 8, dividerY, panelWidth - (panelPadding * 2) - 16, dividerHeight);
            var inventorySectionRectangle = new Rectangle(
                panelX + panelPadding,
                dividerRectangle.Bottom + sectionGap,
                panelWidth - (panelPadding * 2),
                inventorySectionHeight);
            var inventoryStartX = inventorySectionRectangle.X + ((inventorySectionRectangle.Width - inventorySectionWidth) / 2);
            var inventoryStartY = inventorySectionRectangle.Y + sectionPadding;
            var trashBinRectangle = new Rectangle(
                inventorySectionRectangle.Right - sectionPadding - slotSize,
                inventorySectionRectangle.Bottom - sectionPadding - slotSize,
                slotSize,
                slotSize);

            return new InventoryLayout(
                new Rectangle(panelX, panelY, panelWidth, panelHeight),
                headerRectangle,
                craftingSectionRectangle,
                equipmentSectionRectangle,
                equipmentInfoRectangle,
                inventorySectionRectangle,
                dividerRectangle,
                new Point(craftingStartX, craftingStartY),
                new Point(inventoryStartX, inventoryStartY),
                trashBinRectangle,
                new Rectangle(outputSlotX, outputSlotY, slotSize, slotSize),
                craftButtonRectangle,
                new Rectangle(equipmentStartX + slotSize + slotSpacing, equipmentStartY, slotSize, slotSize),
                new Rectangle(equipmentStartX + slotSize + slotSpacing, equipmentStartY + slotSize + slotSpacing, slotSize, slotSize),
                new Rectangle(equipmentStartX, equipmentStartY + slotSize + slotSpacing, slotSize, slotSize),
                new Rectangle(equipmentStartX + (slotSize + slotSpacing) * 2, equipmentStartY + slotSize + slotSpacing, slotSize, slotSize),
                new Rectangle(equipmentStartX + (slotSize + slotSpacing) * 2, equipmentStartY + (slotSize + slotSpacing) * 2, slotSize, slotSize),
                new Rectangle(equipmentStartX, equipmentStartY + (slotSize + slotSpacing) * 2, slotSize, slotSize),
                new Rectangle(equipmentStartX + slotSize + slotSpacing, equipmentStartY + (slotSize + slotSpacing) * 2, slotSize, slotSize),
                slotSize,
                slotSpacing
            );
        }
    }
}
