using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay
{
    public readonly record struct PlayerMovementSettings(
        float MaximumSpeed,
        float MinimumSpeed,
        float Acceleration,
        float Drag,
        float Gravity,
        float MaximumFallSpeed)
    {
        private const float LegacyFramesPerSecond = 60.0f;
        private const float DragMultiplier = 1.5f;
        private const float MinimumEffectiveWeight = 20.0f;
        private const float GravityPerWeightUnit = 25.0f;
        private const float MaximumFallSpeedPerWeightUnit = 400.0f;

        public static PlayerMovementSettings FromPlayer(APlayer player)
        {
            var thruster = player.Thruster;
            var effectiveWeight = Math.Max(player.Weight, MinimumEffectiveWeight);
            var maximumSpeed = thruster.Speed * LegacyFramesPerSecond;
            var minimumSpeed = thruster.MinimumVelocity * LegacyFramesPerSecond;
            var acceleration = thruster.Acceleration * LegacyFramesPerSecond * LegacyFramesPerSecond;
            var drag = acceleration * DragMultiplier;
            var gravity = effectiveWeight * GravityPerWeightUnit;
            var maximumFallSpeed = effectiveWeight * MaximumFallSpeedPerWeightUnit;

            return new PlayerMovementSettings(maximumSpeed, minimumSpeed, acceleration, drag, gravity, maximumFallSpeed);
        }
    }
}
