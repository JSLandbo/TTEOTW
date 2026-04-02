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

        public static bool TryGetSlot(AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing, Point position, out AGridBox slot)
        {
            bool found = TryGetCoordinates(
                grid.GetLength(0),
                grid.GetLength(1),
                position,
                (x, y) => new Rectangle(
                    startX + (x * (slotSize + slotSpacing)),
                    startY + (y * (slotSize + slotSpacing)),
                    slotSize,
                    slotSize),
                out int slotX,
                out int slotY);

            slot = found ? grid[slotX, slotY] : null;
            return found;
        }
    }
}
