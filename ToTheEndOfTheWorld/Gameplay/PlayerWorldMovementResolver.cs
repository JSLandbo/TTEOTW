using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete;
using System;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class PlayerWorldMovementResolver
    {
        private const int MaxIterations = 8;
        private const float CollisionPlacementOffset = 0.0f;

        private readonly float tileSize;
        private readonly float tileTransitionOffset;
        private readonly WorldQueryService worldQueryService;
        private readonly WorldViewportService worldViewportService;

        public PlayerWorldMovementResolver(WorldQueryService worldQueryService, WorldViewportService worldViewportService, int tileSize)
        {
            this.tileSize = tileSize;
            tileTransitionOffset = tileSize * 0.5f;
            this.worldQueryService = worldQueryService;
            this.worldViewportService = worldViewportService;
        }

        public void Resolve(World world, APlayer player)
        {
            for (var i = 0; i < MaxIterations; i++)
            {
                var processedMovement = false;

                if (Math.Abs(player.XOffset) >= Math.Abs(player.YOffset))
                {
                    processedMovement |= TryProcessMovementAxis(world, player, horizontal: true);
                    processedMovement |= TryProcessMovementAxis(world, player, horizontal: false);
                }
                else
                {
                    processedMovement |= TryProcessMovementAxis(world, player, horizontal: false);
                    processedMovement |= TryProcessMovementAxis(world, player, horizontal: true);
                }

                if (!processedMovement)
                {
                    return;
                }
            }
        }

        private bool TryProcessMovementAxis(World world, APlayer player, bool horizontal)
        {
            var offset = horizontal ? player.XOffset : player.YOffset;
            var direction = Math.Sign(offset);

            if (direction == 0)
            {
                return false;
            }

            if (IsAxisObstructed(world, player, horizontal, direction))
            {
                if (horizontal)
                {
                    player.XOffset = direction * CollisionPlacementOffset;
                    player.XVelocity = 0.0f;
                }
                else
                {
                    player.YOffset = direction * CollisionPlacementOffset;
                    player.YVelocity = 0.0f;
                }

                return true;
            }

            if (Math.Abs(offset) < tileTransitionOffset)
            {
                return false;
            }

            worldViewportService.Move(world, horizontal ? direction : 0, horizontal ? 0 : direction);

            if (horizontal)
            {
                player.XOffset -= direction * tileSize;
            }
            else
            {
                player.YOffset -= direction * tileSize;
            }

            return true;
        }

        private bool IsAxisObstructed(World world, APlayer player, bool horizontal, int direction)
        {
            var location = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            var nextBlockVector = new Vector2(
                location.X + (horizontal ? direction : 0),
                location.Y + (horizontal ? 0 : direction)
            );

            return worldQueryService.IsObstructed(world, nextBlockVector);
        }
    }
}
