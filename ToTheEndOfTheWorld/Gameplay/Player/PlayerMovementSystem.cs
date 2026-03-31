using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerMovementSystem
    {
        private const float GroundHorizontalDragFactor = 10.0f;
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
                float maximumHorizontalSpeed = isGrounded ? settings.GroundMaximumSpeed : settings.AirMaximumSpeed;
                float horizontalAcceleration = isGrounded ? settings.GroundAcceleration : settings.AirAcceleration;
                float xTarget = player.MovementInput.X * maximumHorizontalSpeed;
                bool isReversingDirection = Math.Sign(xVelocity) != Math.Sign(xTarget) && xVelocity != 0.0f;
                float xChangeRate = horizontalAcceleration;

                if (isReversingDirection)
                {
                    xChangeRate += isGrounded
                        ? GetHorizontalIdleDrag(xVelocity, true)
                        : settings.AirDrag;
                }

                xVelocity = MoveTowards(xVelocity, xTarget, xChangeRate * deltaTime);
            }

            bool isTryingToMoveUpWithoutLift = player.MovementInput.Y < 0 && !settings.CanMoveUpward;

            if (player.MovementInput.Y != 0 && !isTryingToMoveUpWithoutLift)
            {
                float yTarget = player.MovementInput.Y * settings.AirMaximumSpeed;
                float yChangeRate =
                    player.MovementInput.Y < 0
                        ? GetUpwardAcceleration(player)
                        : Math.Sign(yVelocity) != Math.Sign(yTarget) && yVelocity != 0.0f
                            ? settings.AirAcceleration + settings.AirDrag
                            : settings.AirAcceleration;

                yVelocity = MoveTowards(yVelocity, yTarget, yChangeRate * deltaTime);
            }
            else if (yVelocity < 0.0f)
            {
                yVelocity = MoveTowards(yVelocity, 0.0f, GetUpwardIdleDrag(yVelocity) * deltaTime);
            }

            if (player.MovementInput.Y >= 0 || isTryingToMoveUpWithoutLift)
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

            player.XVelocity = xVelocity;
            player.YVelocity = Math.Clamp(yVelocity, -settings.AirMaximumSpeed, settings.MaximumFallSpeed);

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

        private static float GetUpwardAcceleration(APlayer player)
        {
            if (player.Thruster.Power <= 0.0f)
            {
                return 0.0f;
            }

            float liftRatio = Math.Clamp((player.Thruster.Power - player.Weight) / player.Thruster.Power, 0.0f, 1.0f);

            return player.Thruster.Acceleration * liftRatio;
        }
    }
}
