using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI
{
    public static class UiOverlayLayout
    {
        public const int PanelGap = 40;

        public static int CalculateShopOffset(int shopPanelWidth, int inventoryPanelWidth)
        {
            return -((shopPanelWidth + inventoryPanelWidth) / 2 + PanelGap) / 2;
        }

        public static int CalculateInventoryOffset(int shopPanelWidth, int inventoryPanelWidth)
        {
            return ((shopPanelWidth + inventoryPanelWidth) / 2 + PanelGap) / 2;
        }

        public static Rectangle GetCenteredPanelRectangle(int panelWidth, int panelHeight, int viewportWidth, int viewportHeight, int panelOffsetX = 0)
        {
            return new Rectangle(
                ((viewportWidth - panelWidth) / 2) + panelOffsetX,
                (viewportHeight - panelHeight) / 2,
                panelWidth,
                panelHeight);
        }
    }
}
