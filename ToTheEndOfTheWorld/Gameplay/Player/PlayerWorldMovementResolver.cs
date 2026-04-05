using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerWorldMovementResolver(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldViewportService worldViewportService, PlayerVerticalImpactService playerVerticalImpactService, int tileSize)
    {
        private readonly float tileSize = tileSize;
        private readonly float tileTransitionOffset = tileSize * PlayerWorldTuning.TileTransitionOffsetRatio;

        public bool ResolveStep(ModelWorld world, APlayer player)
        {
            bool processedMovement = false;

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
            float largestOffset = Math.Max(Math.Abs(player.XOffset), Math.Abs(player.YOffset));

            // Offset is consumed one tile transition at a time, so larger offsets may require multiple passes
            return Math.Max(1, (int)Math.Ceiling(largestOffset / tileSize) + 1);
        }

        private bool TryProcessMovementAxis(ModelWorld world, APlayer player, bool horizontal)
        {
            float offset = horizontal ? player.XOffset : player.YOffset;
            int direction = Math.Sign(offset);

            if (direction == 0) return false;

            if (IsAxisObstructed(world, horizontal, direction))
            {
                if (horizontal)
                {
                    player.XOffset = 0.0f;
                    player.XVelocity = 0.0f;
                }
                else
                {
                    if (direction > 0)
                    {
                        playerVerticalImpactService.ApplyFallDamageIfNeeded(player);
                    }

                    player.YOffset = 0.0f;
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

        private bool IsAxisObstructed(ModelWorld world, bool horizontal, int direction)
        {
            Vector2 location = PlayerWorldPositionService.GetPlayerWorldPosition(world);

            Vector2 nextBlockVector = new(
                location.X + (horizontal ? direction : 0),
                location.Y + (horizontal ? 0 : direction)
            );

            return worldBlockDefinitionResolver.IsObstructed(world, nextBlockVector);
        }
    }
}
