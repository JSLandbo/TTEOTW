using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerInputMapper
    {
        public PlayerIntent ReadPlayerIntent(KeyboardState currentState, KeyboardState previousState)
        {
            Vector2 movementInput = ReadMovementInput(currentState);
            Vector2 facingDirection = ReadFacingDirection(currentState, previousState);

            return new PlayerIntent(movementInput, facingDirection);
        }

        private static Vector2 ReadMovementInput(KeyboardState state)
        {
            bool up = IsPressed(state, Keys.Up, Keys.W);
            bool down = IsPressed(state, Keys.Down, Keys.S);
            bool left = IsPressed(state, Keys.Left, Keys.A);
            bool right = IsPressed(state, Keys.Right, Keys.D);

            Vector2 input = new(0, 0);

            if (left)
            {
                input.X -= 1;
            }

            if (right)
            {
                input.X += 1;
            }

            if (up)
            {
                input.Y -= 1;
            }

            if (down)
            {
                input.Y += 1;
            }

            if (input != Vector2.Zero)
            {
                input.Normalize();
            }

            return input;
        }

        private static Vector2 ReadFacingDirection(KeyboardState currentState, KeyboardState previousState)
        {
            if (WasJustPressed(currentState, previousState, Keys.Right, Keys.D))
            {
                return new Vector2(1, 0);
            }

            if (WasJustPressed(currentState, previousState, Keys.Left, Keys.A))
            {
                return new Vector2(-1, 0);
            }

            if (WasJustPressed(currentState, previousState, Keys.Down, Keys.S))
            {
                return new Vector2(0, 1);
            }

            if (WasJustPressed(currentState, previousState, Keys.Up, Keys.W))
            {
                return new Vector2(0, -1);
            }

            return Vector2.Zero;
        }

        private static bool IsPressed(KeyboardState state, params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (state.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
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
