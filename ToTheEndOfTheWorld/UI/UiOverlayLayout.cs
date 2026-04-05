using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;

namespace ToTheEndOfTheWorld.UI
{
    public static class UiOverlayLayout // TODO: Make this dynamic by setting GetWidth()-methods on buildings and inventory and calculate even margins left, middle and center of screen.
    {
        public const int ShopWithInventoryPanelOffsetX = -460;
        public const int InventoryWithShopPanelOffsetX = 460;

        public static Rectangle GetCenteredPanelRectangle(int panelWidth, int panelHeight, int viewportWidth, int viewportHeight, ABuilding building)
        {
            int panelOffsetX = building?.ShowPlayerInventoryWhenOpen == true ? ShopWithInventoryPanelOffsetX : 0;
            return new Rectangle(
                ((viewportWidth - panelWidth) / 2) + panelOffsetX,
                (viewportHeight - panelHeight) / 2,
                panelWidth,
                panelHeight);
        }
    }
}
