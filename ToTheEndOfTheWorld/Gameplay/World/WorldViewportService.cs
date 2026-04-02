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
            for (int renderX = -1; renderX <= world.BlocksWide + 1; renderX++)
            {
                for (int renderY = -1; renderY <= world.BlocksHigh + 1; renderY++)
                {
                    Vector2 renderKey = new(renderX, renderY);
                    Vector2 worldLocation = world.WorldRender[renderKey];
                    world.WorldRender[renderKey] = new Vector2(worldLocation.X + x, worldLocation.Y + y);
                }
            }
        }
    }
}
