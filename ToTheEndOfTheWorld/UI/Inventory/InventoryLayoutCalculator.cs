using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

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
            int craftingColumns = 3;
            int craftingRows = 3;
            int inventoryColumns = inventoryGrid.GetLength(0);
            int inventoryRows = inventoryGrid.GetLength(1);
            int topColumns = craftingColumns + 7;
            int maxColumns = topColumns > inventoryColumns ? topColumns : inventoryColumns;
            int totalRows = craftingRows + inventoryRows;
            int availableWidth = viewportWidth - 128 - (panelPadding * 2) - ((maxColumns - 1) * slotSpacing);
            int availableHeight = viewportHeight - 170 - titleHeight - (sectionGap * 2) - (panelPadding * 2) - ((totalRows - 2) * slotSpacing);
            int slotSize = Math.Max(42, Math.Min(72, Math.Min(availableWidth / maxColumns, availableHeight / totalRows)));
            int craftingSectionWidth = (craftingColumns * slotSize) + ((craftingColumns - 1) * slotSpacing);
            int inventorySectionWidth = (inventoryColumns * slotSize) + ((inventoryColumns - 1) * slotSpacing);
            int craftButtonWidth = slotSize + 20;
            int outputColumnWidth = slotSize + 12;
            int equipmentSectionWidth = (slotSize * 3) + (slotSpacing * 2);
            int topSectionWidth = craftingSectionWidth + outputGap + outputColumnWidth + equipmentGap + equipmentSectionWidth + equipmentInfoGap + equipmentInfoWidth;
            int contentWidth = topSectionWidth > inventorySectionWidth ? topSectionWidth : inventorySectionWidth;
            int craftingHeight = (craftingRows * slotSize) + ((craftingRows - 1) * slotSpacing);
            int inventoryHeight = (inventoryRows * slotSize) + ((inventoryRows - 1) * slotSpacing);
            int equipmentSectionHeight = (slotSize * 3) + (slotSpacing * 2);
            int topContentHeight = craftingHeight > equipmentSectionHeight ? craftingHeight : equipmentSectionHeight;
            int craftingSectionHeight = (sectionPadding * 2) + topContentHeight;
            int inventorySectionHeight = (sectionPadding * 2) + inventoryHeight;
            int contentAreaWidth = contentWidth + (sectionPadding * 2);
            int inventoryAreaWidth = inventorySectionWidth + (sectionPadding * 2);
            int panelWidth = (panelPadding * 2) + (contentAreaWidth > inventoryAreaWidth ? contentAreaWidth : inventoryAreaWidth);
            int panelHeight = titleHeight
                              + (panelPadding * 2)
                              + headerBottomGap
                              + craftingSectionHeight
                              + sectionGap
                              + dividerHeight
                              + sectionGap
                              + inventorySectionHeight;
            int panelX = (viewportWidth - panelWidth) / 2;
            int panelY = (viewportHeight - panelHeight) / 2;
            Rectangle headerRectangle = new(panelX, panelY, panelWidth, titleHeight);
            Rectangle craftingSectionRectangle = new(
                panelX + panelPadding,
                panelY + titleHeight + headerBottomGap,
                panelWidth - (panelPadding * 2),
                craftingSectionHeight);
            int craftingStartX = craftingSectionRectangle.X + ((craftingSectionRectangle.Width - contentWidth) / 2);
            int craftingStartY = craftingSectionRectangle.Y + sectionPadding + ((topContentHeight - craftingHeight) / 2);
            int outputSlotY = craftingStartY + slotSize + slotSpacing;
            Rectangle craftButtonRectangle = new(
                craftingStartX + craftingSectionWidth + outputGap + 11 - ((craftButtonWidth - slotSize) / 2),
                outputSlotY + slotSize + 23,
                craftButtonWidth + 8,
                40);
            int outputSlotX = craftButtonRectangle.Center.X - (slotSize / 2);
            int equipmentStartX = outputSlotX + outputColumnWidth + equipmentGap;
            int equipmentStartY = craftingSectionRectangle.Y + sectionPadding + ((topContentHeight - equipmentSectionHeight) / 2);
            Rectangle equipmentInfoRectangle = new(equipmentStartX + equipmentSectionWidth + equipmentInfoGap, craftingSectionRectangle.Y + sectionPadding, equipmentInfoWidth, topContentHeight);
            Rectangle equipmentSectionRectangle = new(equipmentStartX, craftingSectionRectangle.Y + sectionPadding, equipmentInfoRectangle.Right - equipmentStartX, topContentHeight);
            int dividerY = craftingSectionRectangle.Bottom + sectionGap;
            Rectangle dividerRectangle = new(panelX + panelPadding + 8, dividerY, panelWidth - (panelPadding * 2) - 16, dividerHeight);
            Rectangle inventorySectionRectangle = new(
                panelX + panelPadding,
                dividerRectangle.Bottom + sectionGap,
                panelWidth - (panelPadding * 2),
                inventorySectionHeight);
            Rectangle selfDestructButtonRectangle = new(
                16,
                viewportHeight - slotSize - 16,
                slotSize,
                slotSize);
            int inventoryStartX = inventorySectionRectangle.X + ((inventorySectionRectangle.Width - inventorySectionWidth) / 2);
            int inventoryStartY = inventorySectionRectangle.Y + sectionPadding;
            Rectangle trashBinRectangle = new(
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
                selfDestructButtonRectangle,
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
