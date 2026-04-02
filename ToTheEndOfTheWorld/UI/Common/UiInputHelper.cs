using Microsoft.Xna.Framework.Input;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiInputHelper
    {
        public static bool WasLeftClicked(MouseState currentState, MouseState previousState)
        {
            return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
        }

        public static bool WasRightClicked(MouseState currentState, MouseState previousState)
        {
            return currentState.RightButton == ButtonState.Pressed && previousState.RightButton == ButtonState.Released;
        }

        public static bool WasJustPressed(KeyboardState currentState, KeyboardState previousState, params Keys[] keys)
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

        public static bool WasCloseRequested(KeyboardState currentState, KeyboardState previousState)
        {
            return WasJustPressed(currentState, previousState, Keys.Escape, Keys.E);
        }
    }
}
