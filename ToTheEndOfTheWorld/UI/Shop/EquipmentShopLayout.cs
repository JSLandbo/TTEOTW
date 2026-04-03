using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public readonly struct EquipmentShopLayout(Rectangle panelRectangle, Point gridStart)
    {
        public const int MinimumPanelWidth = 500;
        public const int MinimumPanelHeight = 350;
        public const int HeaderHeight = 58;
        public const int GridTop = HeaderHeight + 20;
        public const int GridBottomPadding = 20;
        public const int TitlePaddingLeft = 20;
        public const int TitlePaddingTop = 14;
        public const int MoneyPaddingTop = 68;
        public const int SlotSize = 68;
        public const int SlotSpacing = 12;
        public const int RowSpacing = 18;
        public const int PanelHorizontalPadding = 20;

        public Rectangle PanelRectangle { get; } = panelRectangle;
        public Rectangle HeaderRectangle { get; } = new Rectangle(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
        public Point GridStart { get; } = gridStart;

        public static EquipmentShopLayout Create(int viewportWidth, int viewportHeight, AGridBox[,] grid)
        {
            int gridWidth = grid.GetLength(0) * SlotSize + ((grid.GetLength(0) - 1) * SlotSpacing);
            int rowHeight = SlotSize;
            int gridHeight = grid.GetLength(1) * rowHeight + ((grid.GetLength(1) - 1) * RowSpacing);
            int panelWidth = System.Math.Max(MinimumPanelWidth, gridWidth + (PanelHorizontalPadding * 2));
            int panelHeight = System.Math.Max(MinimumPanelHeight, GridTop + gridHeight + GridBottomPadding);
            Rectangle panelRectangle = new((viewportWidth - panelWidth) / 2, (viewportHeight - panelHeight) / 2, panelWidth, panelHeight);
            Point gridStart = new(
                panelRectangle.X + ((panelRectangle.Width - gridWidth) / 2),
                panelRectangle.Y + GridTop + ((panelRectangle.Height - GridTop - GridBottomPadding - gridHeight) / 2));

            return new EquipmentShopLayout(panelRectangle, gridStart);
        }

        public Rectangle GetSlotRectangle(int x, int y)
        {
            int rowHeight = SlotSize;
            return new Rectangle(
                GridStart.X + x * (SlotSize + SlotSpacing),
                GridStart.Y + y * (rowHeight + RowSpacing),
                SlotSize,
                SlotSize);
        }
    }
}
