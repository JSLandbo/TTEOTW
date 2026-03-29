using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using System;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public static class InventoryLayoutCalculator
    {
        public static InventoryLayout Create(int viewportWidth, int viewportHeight, AGridBox[,] inventoryGrid)
        {
            const int panelPadding = 24;
            const int slotSpacing = 8;
            const int titleHeight = 60;
            const int sectionGap = 28;
            const int outputGap = 24;
            const int bottomInfoHeight = 16;
            var craftingColumns = 3;
            var craftingRows = 3;
            var inventoryColumns = inventoryGrid.GetLength(0);
            var inventoryRows = inventoryGrid.GetLength(1);
            var topColumns = craftingColumns + 2;
            var maxColumns = topColumns > inventoryColumns ? topColumns : inventoryColumns;
            var totalRows = craftingRows + inventoryRows;
            var availableWidth = viewportWidth - 160 - (panelPadding * 2) - ((maxColumns - 1) * slotSpacing);
            var availableHeight = viewportHeight - 220 - titleHeight - sectionGap - bottomInfoHeight - (panelPadding * 2) - ((totalRows - 2) * slotSpacing);
            var slotSize = Math.Max(42, Math.Min(72, Math.Min(availableWidth / maxColumns, availableHeight / totalRows)));
            var craftingSectionWidth = (craftingColumns * slotSize) + ((craftingColumns - 1) * slotSpacing);
            var inventorySectionWidth = (inventoryColumns * slotSize) + ((inventoryColumns - 1) * slotSpacing);
            var craftButtonWidth = slotSize + 28;
            var topSectionWidth = craftingSectionWidth + outputGap + craftButtonWidth;
            var contentWidth = topSectionWidth > inventorySectionWidth ? topSectionWidth : inventorySectionWidth;
            var craftingHeight = (craftingRows * slotSize) + ((craftingRows - 1) * slotSpacing);
            var inventoryHeight = (inventoryRows * slotSize) + ((inventoryRows - 1) * slotSpacing);
            var panelWidth = (panelPadding * 2) + contentWidth;
            var panelHeight = titleHeight + (panelPadding * 2) + craftingHeight + sectionGap + inventoryHeight + bottomInfoHeight;
            var panelX = (viewportWidth - panelWidth) / 2;
            var panelY = (viewportHeight - panelHeight) / 2;
            var craftingStartX = panelX + panelPadding;
            var craftingStartY = panelY + titleHeight + panelPadding;
            var outputSlotX = craftingStartX + craftingSectionWidth + outputGap;
            var outputSlotY = craftingStartY + slotSize + (slotSpacing / 2);
            var craftButtonRectangle = new Rectangle(outputSlotX - 14, outputSlotY + slotSize + 12, craftButtonWidth, 34);
            var inventoryStartX = panelX + panelPadding + ((contentWidth - inventorySectionWidth) / 2);
            var inventoryStartY = craftingStartY + craftingHeight + sectionGap;

            return new InventoryLayout(
                new Rectangle(panelX, panelY, panelWidth, panelHeight),
                new Point(craftingStartX, craftingStartY),
                new Point(inventoryStartX, inventoryStartY),
                new Rectangle(outputSlotX, outputSlotY, slotSize, slotSize),
                new Vector2(craftingStartX + craftingSectionWidth + 8, outputSlotY + (slotSize / 4.0f)),
                craftButtonRectangle,
                slotSize,
                slotSpacing
            );
        }
    }
}
