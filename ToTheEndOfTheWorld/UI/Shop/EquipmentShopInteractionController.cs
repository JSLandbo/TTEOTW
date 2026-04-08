using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ToTheEndOfTheWorld.UI.Common;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopInteractionController
    {
        public void TryHandleBuy(MouseState currentMouseState, MouseState previousMouseState, EquipmentShopLayout layout, ModelWorld world, ABuilding building, EquipmentShopService equipmentShopService)
        {
            if (!UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                return;
            }

            if (building?.StorageInventory?.Items?.InternalGrid == null)
            {
                return;
            }

            if (!TryGetClickedSlot(currentMouseState.Position, layout, building.StorageInventory.Items.InternalGrid, out int slotX, out int slotY))
            {
                return;
            }

            equipmentShopService.TryBuy(world, building, slotX, slotY);
        }

        private static bool TryGetClickedSlot(Point mousePosition, EquipmentShopLayout layout, AGridBox[,] grid, out int slotX, out int slotY)
        {
            return UiGridHitTestHelper.TryGetCoordinates(grid.GetLength(0), grid.GetLength(1), mousePosition, layout.GetSlotRectangle, out slotX, out slotY);
        }

    }
}
