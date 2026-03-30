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
        private readonly PlayerHullSystem playerHullSystem;
        private readonly PlayerFallDamageProtectionService playerFallDamageProtectionService;

        public PlayerWorldMovementResolver(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldViewportService worldViewportService, PlayerHullSystem playerHullSystem, PlayerFallDamageProtectionService playerFallDamageProtectionService, int tileSize)
        {
            this.tileSize = tileSize;
            tileTransitionOffset = tileSize * PlayerWorldTuning.TileTransitionOffsetRatio;
            this.worldBlockDefinitionResolver = worldBlockDefinitionResolver;
            this.worldViewportService = worldViewportService;
            this.playerHullSystem = playerHullSystem;
            this.playerFallDamageProtectionService = playerFallDamageProtectionService;
        }

        public bool ResolveStep(ModelWorld world, APlayer player, float downwardImpactVelocity)
        {
            bool processedMovement = false;

            if (Math.Abs(player.XOffset) >= Math.Abs(player.YOffset))
            {
                processedMovement |= TryProcessMovementAxis(world, player, horizontal: true, downwardImpactVelocity);
                processedMovement |= TryProcessMovementAxis(world, player, horizontal: false, downwardImpactVelocity);
            }
            else
            {
                processedMovement |= TryProcessMovementAxis(world, player, horizontal: false, downwardImpactVelocity);
                processedMovement |= TryProcessMovementAxis(world, player, horizontal: true, downwardImpactVelocity);
            }

            return processedMovement;
        }

        public int EstimateRequiredIterations(APlayer player) => CalculateRequiredIterations(player);

        private int CalculateRequiredIterations(APlayer player)
        {
            float largestOffset = Math.Max(Math.Abs(player.XOffset), Math.Abs(player.YOffset));

            // Offset is consumed one tile transition at a time, so larger offsets may require multiple passes.
            // The extra pass avoids stopping one step early when the offset lands near the transition boundary.
            return Math.Max(1, (int)Math.Ceiling(largestOffset / tileSize) + 1);
        }

        private bool TryProcessMovementAxis(ModelWorld world, APlayer player, bool horizontal, float downwardImpactVelocity)
        {
            float offset = horizontal ? player.XOffset : player.YOffset;
            int direction = Math.Sign(offset);

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
                    if (direction > 0 && !playerFallDamageProtectionService.IsProtectedThisFrame)
                    {
                        playerHullSystem.ApplyFallDamage(player, downwardImpactVelocity);
                    }

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
            Vector2 location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            Vector2 nextBlockVector = new(
                location.X + (horizontal ? direction : 0),
                location.Y + (horizontal ? 0 : direction)
            );

            return worldBlockDefinitionResolver.IsObstructed(world, nextBlockVector);
        }
    }
}
