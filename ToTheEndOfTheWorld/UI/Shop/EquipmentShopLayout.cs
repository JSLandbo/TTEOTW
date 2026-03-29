using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public readonly struct EquipmentShopLayout
    {
        public const int MinimumPanelWidth = 500;
        public const int MinimumPanelHeight = 350;
        public const int HeaderHeight = 58;
        public const int GridTop = 102;
        public const int GridBottomPadding = 66;
        public const int FooterTextBottomPadding = 42;
        public const int TitlePaddingLeft = 20;
        public const int TitlePaddingTop = 14;
        public const int MoneyPaddingTop = 68;
        public const int SlotSize = 68;
        public const int SlotSpacing = 12;
        public const int PanelHorizontalPadding = 32;

        public EquipmentShopLayout(Rectangle panelRectangle, Point gridStart)
        {
            PanelRectangle = panelRectangle;
            HeaderRectangle = new Rectangle(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            GridStart = gridStart;
        }

        public Rectangle PanelRectangle { get; }
        public Rectangle HeaderRectangle { get; }
        public Point GridStart { get; }

        public static EquipmentShopLayout Create(int viewportWidth, int viewportHeight, AGridBox[,] grid)
        {
            var gridWidth = grid.GetLength(0) * SlotSize + ((grid.GetLength(0) - 1) * SlotSpacing);
            var gridHeight = grid.GetLength(1) * SlotSize + ((grid.GetLength(1) - 1) * SlotSpacing);
            var panelWidth = System.Math.Max(MinimumPanelWidth, gridWidth + (PanelHorizontalPadding * 2));
            var panelHeight = System.Math.Max(MinimumPanelHeight, GridTop + gridHeight + GridBottomPadding);
            var panelRectangle = new Rectangle((viewportWidth - panelWidth) / 2, (viewportHeight - panelHeight) / 2, panelWidth, panelHeight);
            var gridStart = new Point(
                panelRectangle.X + ((panelRectangle.Width - gridWidth) / 2),
                panelRectangle.Y + GridTop + ((panelRectangle.Height - GridTop - GridBottomPadding - gridHeight) / 2));

            return new EquipmentShopLayout(panelRectangle, gridStart);
        }

        public Rectangle GetSlotRectangle(int x, int y)
        {
            return new Rectangle(
                GridStart.X + x * (SlotSize + SlotSpacing),
                GridStart.Y + y * (SlotSize + SlotSpacing),
                SlotSize,
                SlotSize);
        }
    }
}
