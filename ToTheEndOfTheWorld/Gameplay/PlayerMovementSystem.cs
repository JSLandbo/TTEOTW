using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class PlayerMovementSystem
    {
        public void Update(APlayer player, float deltaTime)
        {
            var settings = PlayerMovementSettings.FromThruster(player.Thruster);
            var xVelocity = player.XVelocity;
            var yVelocity = player.YVelocity;

            if (player.MovementInput.X == 0)
            {
                xVelocity = MoveTowards(xVelocity, 0.0f, settings.Drag * deltaTime);
            }
            else
            {
                var xTarget = player.MovementInput.X * settings.MaximumSpeed;
                var xChangeRate = Math.Sign(xVelocity) != Math.Sign(xTarget) && xVelocity != 0.0f
                    ? settings.Acceleration + settings.Drag
                    : settings.Acceleration;

                xVelocity = MoveTowards(xVelocity, xTarget, xChangeRate * deltaTime);
            }

            if (player.MovementInput.Y != 0)
            {
                var yTarget = player.MovementInput.Y * settings.MaximumSpeed;
                var yChangeRate = Math.Sign(yVelocity) != Math.Sign(yTarget) && yVelocity != 0.0f
                    ? settings.Acceleration + settings.Drag
                    : settings.Acceleration;

                yVelocity = MoveTowards(yVelocity, yTarget, yChangeRate * deltaTime);
            }

            if (player.MovementInput.X != 0 && Math.Abs(xVelocity) < settings.MinimumSpeed)
            {
                xVelocity = Math.Sign(player.MovementInput.X) * settings.MinimumSpeed;
            }

            if (player.MovementInput.Y != 0 && Math.Abs(yVelocity) < settings.MinimumSpeed)
            {
                yVelocity = Math.Sign(player.MovementInput.Y) * settings.MinimumSpeed;
            }

            if (player.MovementInput.Y >= 0)
            {
                yVelocity += settings.Gravity * deltaTime;
            }

            player.XVelocity = Math.Clamp(xVelocity, -settings.MaximumSpeed, settings.MaximumSpeed);
            player.YVelocity = Math.Clamp(yVelocity, -settings.MaximumSpeed, settings.MaximumFallSpeed);

            if (player.MovementInput.X == 0 && Math.Abs(player.XVelocity) < 1.0f)
            {
                player.XVelocity = 0.0f;
            }

            player.XOffset += player.XVelocity * deltaTime;
            player.YOffset += player.YVelocity * deltaTime;
        }

        private static float MoveTowards(float current, float target, float maxDelta)
        {
            if (Math.Abs(target - current) <= maxDelta)
            {
                return target;
            }

            return current + Math.Sign(target - current) * maxDelta;
        }
    }
}
