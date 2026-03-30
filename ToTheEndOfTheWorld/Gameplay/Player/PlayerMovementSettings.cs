using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public readonly record struct PlayerMovementSettings(
        float MaximumSpeed,
        float MinimumSpeed,
        float Acceleration,
        float Drag,
        float Gravity,
        float MaximumFallSpeed)
    {
        private const float DragMultiplier = 1.5f;
        private const float MinimumEffectiveWeight = 20.0f;
        private const float GravityPerWeightUnit = 25.0f;
        private const float MaximumFallSpeedPerWeightUnit = 650.0f;
        private const float ThrusterFallSpeedContributionMultiplier = 1.0f;

        public static PlayerMovementSettings FromPlayer(APlayer player)
        {
            ModelLibrary.Abstract.PlayerShipComponents.AThruster thruster = player.Thruster;
            float effectiveWeight = Math.Max(player.Weight, MinimumEffectiveWeight);
            float maximumSpeed = thruster.Speed;
            float minimumSpeed = thruster.MinimumVelocity;
            float acceleration = thruster.Acceleration;
            float drag = acceleration * DragMultiplier;
            float gravity = effectiveWeight * GravityPerWeightUnit;
            float maximumFallSpeed = (effectiveWeight * MaximumFallSpeedPerWeightUnit) + (maximumSpeed * ThrusterFallSpeedContributionMultiplier);

            return new PlayerMovementSettings(maximumSpeed, minimumSpeed, acceleration, drag, gravity, maximumFallSpeed);
        }
    }
}
