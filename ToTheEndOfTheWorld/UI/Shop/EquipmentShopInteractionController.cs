using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ToTheEndOfTheWorld.UI.Common;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopInteractionController
    {
        public bool ShouldClose(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            return UiInputHelper.WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape, Keys.E);
        }

        public void TryHandleBuy(MouseState currentMouseState, MouseState previousMouseState, EquipmentShopLayout layout, ModelWorld world, ABuilding building, EquipmentShopService equipmentShopService)
        {
            if (!UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                return;
            }

            if (building?.StorageGrid?.InternalGrid == null)
            {
                return;
            }

            if (!TryGetClickedSlot(currentMouseState.Position, layout, building.StorageGrid.InternalGrid, out int slotX, out int slotY))
            {
                return;
            }

            equipmentShopService.TryBuy(world, building, slotX, slotY);
        }

        private static bool TryGetClickedSlot(Point mousePosition, EquipmentShopLayout layout, AGridBox[,] grid, out int slotX, out int slotY)
        {
            for (slotY = 0; slotY < grid.GetLength(1); slotY++)
            {
                for (slotX = 0; slotX < grid.GetLength(0); slotX++)
                {
                    if (!layout.GetSlotRectangle(slotX, slotY).Contains(mousePosition))
                    {
                        continue;
                    }

                    return true;
                }
            }

            slotX = -1;
            slotY = -1;

            return false;
        }

    }
}
