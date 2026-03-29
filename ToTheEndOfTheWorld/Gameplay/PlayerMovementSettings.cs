using ModelLibrary.Abstract.PlayerShipComponents;

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
        private const float GravityMultiplier = 2.5f;
        private const float MaximumFallSpeedMultiplier = 3.0f;

        public static PlayerMovementSettings FromThruster(AThruster thruster)
        {
            var maximumSpeed = thruster.Speed * LegacyFramesPerSecond;
            var minimumSpeed = thruster.MinimumVelocity * LegacyFramesPerSecond;
            var acceleration = thruster.Acceleration * LegacyFramesPerSecond * LegacyFramesPerSecond;
            var drag = acceleration * DragMultiplier;
            var gravity = acceleration * GravityMultiplier;
            var maximumFallSpeed = maximumSpeed * MaximumFallSpeedMultiplier;

            return new PlayerMovementSettings(maximumSpeed, minimumSpeed, acceleration, drag, gravity, maximumFallSpeed);
        }
    }
}
