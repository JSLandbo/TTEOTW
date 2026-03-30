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

            var location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            var belowPlayer = new Vector2(location.X, location.Y + 1);

            return worldBlockDefinitionResolver.IsObstructed(world, belowPlayer);
        }
    }
}
