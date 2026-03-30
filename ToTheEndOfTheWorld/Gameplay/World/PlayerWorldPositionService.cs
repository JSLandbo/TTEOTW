using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public static class PlayerWorldPositionService
    {
        public static Vector2 GetPlayerWorldPosition(ModelWorld world)
        {
            ModelLibrary.Abstract.APlayer player = world.Player;
            return world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
        }

        public static WorldTile GetPlayerWorldTile(ModelWorld world)
        {
            Vector2 worldPosition = GetPlayerWorldPosition(world);
            return new WorldTile((long)worldPosition.X, (long)worldPosition.Y);
        }
    }
}
