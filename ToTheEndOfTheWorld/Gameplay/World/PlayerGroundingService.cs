using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public static class PlayerGroundingService
    {
        public static bool IsGrounded(ModelWorld world, APlayer player, WorldBlockDefinitionResolver worldBlockDefinitionResolver)
        {
            if (player.YVelocity < -PlayerWorldTuning.VelocityStopThreshold)
            {
                return false;
            }

            if (player.YOffset < -PlayerWorldTuning.MiningContactTolerance)
            {
                return false;
            }

            Vector2 location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            Vector2 belowPlayer = new(location.X, location.Y + 1);

            return worldBlockDefinitionResolver.IsObstructed(world, belowPlayer);
        }
    }
}
