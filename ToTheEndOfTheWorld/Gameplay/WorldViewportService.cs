using Microsoft.Xna.Framework;
using ModelLibrary.Concrete;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldViewportService
    {
        public void EnsurePadding(World world)
        {
            EnsurePadding(world, GetCenterWorldPosition(world));
        }

        public void EnsurePadding(World world, Vector2 centerWorldPosition)
        {
            var playerKey = GetCenterRenderKey(world.BlocksWide, world.BlocksHigh);
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

            world.Player.Coordinates = playerKey;
            world.WorldRender = paddedRender;
        }

        public Vector2 GetCenterWorldPosition(World world)
        {
            return world.WorldRender[GetCenterRenderKey(world.BlocksWide, world.BlocksHigh)];
        }

        public Vector2 GetCenterRenderKey(int blocksWide, int blocksHigh)
        {
            return new Vector2(
                (float)System.Math.Floor(blocksWide / 2.0d),
                (float)System.Math.Floor(blocksHigh / 2.0d));
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
