using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiGridHitTestHelper
    {
        public static bool TryGetCoordinates(int columns, int rows, Point position, Func<int, int, Rectangle> getSlotRectangle, out int x, out int y)
        {
            for (y = 0; y < rows; y++)
            {
                for (x = 0; x < columns; x++)
                {
                    if (getSlotRectangle(x, y).Contains(position))
                    {
                        return true;
                    }
                }
            }

            x = -1;
            y = -1;

            return false;
        }

        public static bool TryGetSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot, int scrollOffset = 0, int visibleRows = -1)
        {
            int columns = grid.GetLength(0);
            int totalRows = grid.GetLength(1);
            int rowsToCheck = visibleRows > 0 ? Math.Min(visibleRows, totalRows - scrollOffset) : totalRows;

            bool found = TryGetCoordinates(
                columns,
                rowsToCheck,
                position,
                (x, y) => new Rectangle(
                    startX + (x * (slotSize + slotSpacing)),
                    startY + (y * (slotSize + slotSpacing)),
                    slotSize,
                    slotSize),
                out int slotX,
                out int visibleY);

            if (found)
            {
                int actualY = scrollOffset + visibleY;
                if (actualY < totalRows)
                {
                    slot = grid[slotX, actualY];
                    return true;
                }
            }

            slot = null;
            return false;
        }
    }
}
