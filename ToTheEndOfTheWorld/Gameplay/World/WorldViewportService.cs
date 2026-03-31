using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldViewportService
    {
        public void EnsurePadding(ModelWorld world)
        {
            if (world.WorldRender == null || world.WorldRender.Count == 0)
            {
                EnsurePadding(world, world.Player.Coordinates);
                return;
            }

            EnsurePadding(world, GetCenterWorldPosition(world));
        }

        public void EnsurePadding(ModelWorld world, Vector2 centerWorldPosition)
        {
            Vector2 playerKey = GetCenterRenderKey(world.BlocksWide, world.BlocksHigh);
            Dictionary<Vector2, Vector2> paddedRender = [];

            for (int x = -1; x <= world.BlocksWide + 1; x++)
            {
                for (int y = -1; y <= world.BlocksHigh + 1; y++)
                {
                    Vector2 renderKey = new(x, y);
                    Vector2 worldLocation = new(
                        centerWorldPosition.X + (x - playerKey.X),
                        centerWorldPosition.Y + (y - playerKey.Y)
                    );

                    paddedRender[renderKey] = worldLocation;
                }
            }

            world.Player.Coordinates = playerKey;
            world.WorldRender = paddedRender;
        }

        public Vector2 GetCenterWorldPosition(ModelWorld world)
        {
            return world.WorldRender[GetCenterRenderKey(world.BlocksWide, world.BlocksHigh)];
        }

        public Vector2 GetCenterRenderKey(int blocksWide, int blocksHigh)
        {
            return new Vector2(
                (float)System.Math.Floor(blocksWide / 2.0d),
                (float)System.Math.Floor(blocksHigh / 2.0d));
        }

        public void Move(ModelWorld world, float x, float y)
        {
            Dictionary<Vector2, Vector2> updated = [];

            foreach (KeyValuePair<Vector2, Vector2> block in world.WorldRender)
            {
                updated.Add(new Vector2(block.Key.X, block.Key.Y), new Vector2(block.Value.X + x, block.Value.Y + y));
            }

            world.WorldRender = updated;
        }
    }
}
