using Microsoft.Xna.Framework;
using ModelLibrary.Concrete;
using ToTheEndOfTheWorld.Context.StaticRepositories;

namespace ToTheEndOfTheWorld.Gameplay
{
    public static class PlayerWorldPositionService
    {
        public static Vector2 GetPlayerWorldPosition(World world)
        {
            var player = world.Player;
            return world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
        }

        public static WorldTile GetPlayerWorldTile(World world)
        {
            var worldPosition = GetPlayerWorldPosition(world);
            return new WorldTile((long)worldPosition.X, (long)worldPosition.Y);
        }
    }
}
