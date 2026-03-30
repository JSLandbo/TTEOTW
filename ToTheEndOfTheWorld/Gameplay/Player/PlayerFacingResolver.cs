using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerFacingResolver
    {
        public Vector2 Resolve(APlayer player, PlayerIntent intent)
        {
            if (intent.FacingDirection != Vector2.Zero)
            {
                return intent.FacingDirection;
            }

            if (intent.MovementInput == Vector2.Zero)
            {
                if (Math.Abs(player.XVelocity) > Math.Abs(player.YVelocity))
                {
                    return new Vector2(Math.Sign(player.XVelocity), 0);
                }

                if (Math.Abs(player.YVelocity) > 0)
                {
                    return new Vector2(0, Math.Sign(player.YVelocity));
                }

                return player.FacingDirection;
            }

            if (intent.MovementInput.X == 0 || intent.MovementInput.Y == 0)
            {
                return intent.MovementInput;
            }

            if (FacingMatchesInput(player.FacingDirection, intent.MovementInput))
            {
                return player.FacingDirection;
            }

            if (intent.MovementInput.X != 0)
            {
                return new Vector2(Math.Sign(intent.MovementInput.X), 0);
            }

            return new Vector2(0, Math.Sign(intent.MovementInput.Y));
        }

        private static bool FacingMatchesInput(Vector2 facingDirection, Vector2 input)
        {
            if (facingDirection == Vector2.Zero)
            {
                return false;
            }

            if (facingDirection.X != 0)
            {
                return Math.Sign(facingDirection.X) == Math.Sign(input.X) && input.X != 0;
            }

            return Math.Sign(facingDirection.Y) == Math.Sign(input.Y) && input.Y != 0;
        }
    }
}
