using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public static class PlayerWorldPositionService
    {
        public static Vector2 GetPlayerWorldPosition(ModelWorld world)
        {
            var player = world.Player;
            return world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
        }

        public static WorldTile GetPlayerWorldTile(ModelWorld world)
        {
            var worldPosition = GetPlayerWorldPosition(world);
            return new WorldTile((long)worldPosition.X, (long)worldPosition.Y);
        }
    }
}
