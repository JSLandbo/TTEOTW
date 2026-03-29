using Microsoft.Xna.Framework;
using ModelLibrary.Concrete;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldViewportService
    {
        public void EnsurePadding(World world)
        {
            var playerKey = new Vector2(world.Player.Coordinates.X, world.Player.Coordinates.Y);
            var centerWorldPosition = world.WorldRender[playerKey];
            var paddedRender = new Dictionary<Vector2, Vector2>();

            for (var x = -1; x <= world.BlocksWide + 1; x++)
            {
                for (var y = -1; y <= world.BlocksHigh + 1; y++)
                {
                    var renderKey = new Vector2(x, y);
                    var worldLocation = new Vector2(
                        centerWorldPosition.X + (x - playerKey.X),
                        centerWorldPosition.Y + (y - playerKey.Y)
                    );

                    paddedRender[renderKey] = worldLocation;
                }
            }

            world.WorldRender = paddedRender;
        }

        public void Move(World world, float x, float y)
        {
            var updated = new Dictionary<Vector2, Vector2>();

            foreach (var block in world.WorldRender)
            {
                updated.Add(new Vector2(block.Key.X, block.Key.Y), new Vector2(block.Value.X + x, block.Value.Y + y));
            }

            world.WorldRender = updated;
        }
    }
}
