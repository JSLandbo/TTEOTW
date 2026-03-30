using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerWorldMovementResolver
    {
        private readonly float tileSize;
        private readonly float tileTransitionOffset;
        private readonly WorldBlockDefinitionResolver worldBlockDefinitionResolver;
        private readonly WorldViewportService worldViewportService;

        public PlayerWorldMovementResolver(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldViewportService worldViewportService, int tileSize)
        {
            this.tileSize = tileSize;
            tileTransitionOffset = tileSize * PlayerWorldTuning.TileTransitionOffsetRatio;
            this.worldBlockDefinitionResolver = worldBlockDefinitionResolver;
            this.worldViewportService = worldViewportService;
        }

        public bool ResolveStep(ModelWorld world, APlayer player)
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

            return processedMovement;
        }

        public int EstimateRequiredIterations(APlayer player) => CalculateRequiredIterations(player);

        private int CalculateRequiredIterations(APlayer player)
        {
            var largestOffset = Math.Max(Math.Abs(player.XOffset), Math.Abs(player.YOffset));

            // Offset is consumed one tile transition at a time, so larger offsets may require multiple passes.
            // The extra pass avoids stopping one step early when the offset lands near the transition boundary.
            return Math.Max(1, (int)Math.Ceiling(largestOffset / tileSize) + 1);
        }

        private bool TryProcessMovementAxis(ModelWorld world, APlayer player, bool horizontal)
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
                    player.XOffset = direction * PlayerWorldTuning.CollisionPlacementOffset;
                    player.XVelocity = 0.0f;
                }
                else
                {
                    player.YOffset = direction * PlayerWorldTuning.CollisionPlacementOffset;
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

        private bool IsAxisObstructed(ModelWorld world, APlayer player, bool horizontal, int direction)
        {
            var location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            var nextBlockVector = new Vector2(
                location.X + (horizontal ? direction : 0),
                location.Y + (horizontal ? 0 : direction)
            );

            return worldBlockDefinitionResolver.IsObstructed(world, nextBlockVector);
        }
    }
}
