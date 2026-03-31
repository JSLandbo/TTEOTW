using ModelLibrary.Abstract;

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
        private const float GravityPerSecond = 1000.0f;
        private const float BaseMaximumFallSpeed = 50000.0f;

        public static PlayerMovementSettings FromPlayer(APlayer player)
        {
            ModelLibrary.Abstract.PlayerShipComponents.AThruster thruster = player.Thruster;
            ModelLibrary.Abstract.PlayerShipComponents.AEngine engine = player.Engine;
            float airMaximumSpeed = thruster.Speed;
            float groundMaximumSpeed = engine.Speed;
            float airAcceleration = thruster.Acceleration;
            float airDrag = airAcceleration * DragMultiplier;
            float groundAcceleration = engine.Acceleration;
            bool canMoveUpward = thruster.Power >= player.Weight;
            float gravity = GravityPerSecond;
            float maximumFallSpeed = BaseMaximumFallSpeed + airMaximumSpeed;

            return new PlayerMovementSettings(airMaximumSpeed, groundMaximumSpeed, airAcceleration, airDrag, groundAcceleration, canMoveUpward, gravity, maximumFallSpeed);
        }
    }
}
