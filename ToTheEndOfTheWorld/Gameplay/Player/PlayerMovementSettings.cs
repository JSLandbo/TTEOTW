using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public readonly record struct PlayerMovementSettings(
        float AirMaximumSpeed,
        float GroundMaximumSpeed,
        float AirAcceleration,
        float AirDrag,
        float GroundAcceleration,
        bool CanMoveUpward,
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
            ModelLibrary.Abstract.PlayerShipComponents.AEngine engine = player.Engine;
            float effectiveWeight = Math.Max(player.Weight, MinimumEffectiveWeight);
            float airMaximumSpeed = thruster.Speed;
            float groundMaximumSpeed = engine.Speed;
            float airAcceleration = thruster.Acceleration;
            float airDrag = airAcceleration * DragMultiplier;
            float groundAcceleration = engine.Acceleration;
            bool canMoveUpward = thruster.Power >= player.Weight;
            float gravity = effectiveWeight * GravityPerWeightUnit;
            float maximumFallSpeed = (effectiveWeight * MaximumFallSpeedPerWeightUnit) + (airMaximumSpeed * ThrusterFallSpeedContributionMultiplier);

            return new PlayerMovementSettings(airMaximumSpeed, groundMaximumSpeed, airAcceleration, airDrag, groundAcceleration, canMoveUpward, gravity, maximumFallSpeed);
        }
    }
}
