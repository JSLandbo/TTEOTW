using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopInteractionController
    {
        public bool ShouldClose(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            return WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape, Keys.E);
        }

        public void TryHandleBuy(MouseState currentMouseState, MouseState previousMouseState, EquipmentShopLayout layout, ModelWorld world, ABuilding building, EquipmentShopService equipmentShopService)
        {
            if (!WasLeftClicked(currentMouseState, previousMouseState))
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

        private static bool WasLeftClicked(MouseState currentState, MouseState previousState)
        {
            return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
        }

        private static bool WasJustPressed(KeyboardState currentState, KeyboardState previousState, params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentState.IsKeyDown(key) && !previousState.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
