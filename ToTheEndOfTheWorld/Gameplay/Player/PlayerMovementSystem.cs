using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerMovementSystem
    {
        private const float GroundHorizontalDragFactor = 5.0f;
        private const float GroundHorizontalStopThreshold = 35.0f;
        private const float AirHorizontalDragFactor = 0.75f;
        private const float UpwardIdleDragFactor = 2.0f;

        public void Update(APlayer player, float deltaTime, bool isGrounded)
        {
            PlayerMovementSettings settings = PlayerMovementSettings.FromPlayer(player);
            float xVelocity = player.XVelocity;
            float yVelocity = player.YVelocity;

            if (player.MovementInput.X == 0)
            {
                xVelocity = MoveTowards(xVelocity, 0.0f, GetHorizontalIdleDrag(xVelocity, isGrounded) * deltaTime);
            }
            else
            {
                float xTarget = player.MovementInput.X * settings.MaximumSpeed;
                float xChangeRate = Math.Sign(xVelocity) != Math.Sign(xTarget) && xVelocity != 0.0f
                    ? settings.Acceleration + settings.Drag
                    : settings.Acceleration;

                xVelocity = MoveTowards(xVelocity, xTarget, xChangeRate * deltaTime);
            }

            if (player.MovementInput.Y != 0)
            {
                float yTarget = player.MovementInput.Y * settings.MaximumSpeed;
                float yChangeRate =
                    player.MovementInput.Y < 0
                        ? settings.Acceleration + settings.Drag
                        : Math.Sign(yVelocity) != Math.Sign(yTarget) && yVelocity != 0.0f
                            ? settings.Acceleration + settings.Drag
                            : settings.Acceleration;

                yVelocity = MoveTowards(yVelocity, yTarget, yChangeRate * deltaTime);
            }
            else if (yVelocity < 0.0f)
            {
                yVelocity = MoveTowards(yVelocity, 0.0f, GetUpwardIdleDrag(yVelocity) * deltaTime);
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

            if (player.MovementInput.X == 0 && isGrounded && Math.Abs(xVelocity) < GroundHorizontalStopThreshold)
            {
                xVelocity = 0.0f;
            }

            if (player.MovementInput.X == 0 && Math.Abs(xVelocity) < PlayerWorldTuning.VelocityStopThreshold)
            {
                xVelocity = 0.0f;
            }

            player.XVelocity = Math.Clamp(xVelocity, -settings.MaximumSpeed, settings.MaximumSpeed);
            player.YVelocity = Math.Clamp(yVelocity, -settings.MaximumSpeed, settings.MaximumFallSpeed);

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

        private static float GetHorizontalIdleDrag(float velocity, bool isGrounded)
        {
            float dragFactor = isGrounded ? GroundHorizontalDragFactor : AirHorizontalDragFactor;
            return Math.Abs(velocity) * dragFactor;
        }

        private static float GetUpwardIdleDrag(float velocity)
        {
            return Math.Abs(velocity) * UpwardIdleDragFactor;
        }
    }
}
