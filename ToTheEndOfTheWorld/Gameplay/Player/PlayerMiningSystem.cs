using System;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete.Blocks;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerMiningSystem(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldBlockDamageService worldBlockDamageService, PlayerHeatSystem playerHeatSystem, PlayerHullSystem playerHullSystem, PlayerFuelSystem playerFuelSystem, PlayerVerticalImpactService playerVerticalImpactService, int tileSize)
    {
        private readonly float miningCenterTolerance = tileSize * PlayerWorldTuning.MiningCenterToleranceRatio;

        public bool Update(ModelWorld world, APlayer player, float deltaTime)
        {
            bool isGrounded = PlayerGroundingService.IsGrounded(world, player, worldBlockDefinitionResolver);

            if (!CanContinueMining(player))
            {
                player.DrillExtended = false;

                return false;
            }

            if (!playerFuelSystem.CanAffordMining(player, deltaTime, isGrounded))
            {
                player.DrillExtended = false;

                return false;
            }

            Vector2 location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            Vector2 blockVector = new(location.X + player.FacingDirection.X, location.Y + player.FacingDirection.Y);

            if (!isGrounded)
            {
                player.DrillExtended = ShouldKeepDrillExtendedWhileAdvancing(player);

                return false;
            }

            if (!IsTouchingMiningSurface(player))
            {
                player.DrillExtended = ShouldKeepDrillExtendedWhileAdvancing(player);

                return false;
            }

            if (!IsCenteredForMining(player))
            {
                player.DrillExtended = false;

                return false;
            }

            if (!worldBlockDamageService.TryGetBlockInteraction(world, blockVector, out MiningInteraction interaction, out bool isBlockedByBuilding))
            {
                player.DrillExtended = isBlockedByBuilding
                    ? false
                    : ShouldKeepDrillExtendedWhileAdvancing(player);

                return false;
            }

            if (interaction.Block.Hardness > player.Drill.Hardness)
            {
                player.DrillExtended = false;

                return false;
            }

            if (player.FacingDirection.Y > 0)
            {
                if (!playerVerticalImpactService.CanStartDownwardMining(player))
                {
                    player.DrillExtended = false;

                    return false;
                }

                StopHorizontalMovementForVerticalMining(player);
            }

            // TODO: Consider this, mining at 100.000f is ridiculously fast lmao.
            // Keep the old snap-and-stop behavior for blocks that survive the hit,
            // but let one-shot blocks break before they steal the ship's movement.
            if (!WillBeDestroyedByHit(interaction.Block, player.Drill.Damage))
            {
                SnapPlayerToMiningBlock(player);
            }

            player.DrillExtended = true;

            return DealDamageInArea(world, player, location, deltaTime);
        }

        private static bool ShouldKeepDrillExtendedWhileAdvancing(APlayer player)
        {
            if (!player.DrillExtended || player.FacingDirection == Vector2.Zero)
            {
                return false;
            }

            float forwardVelocity = (player.XVelocity * player.FacingDirection.X) + (player.YVelocity * player.FacingDirection.Y);
            float forwardOffset = (player.XOffset * player.FacingDirection.X) + (player.YOffset * player.FacingDirection.Y);

            return forwardVelocity > PlayerWorldTuning.VelocityStopThreshold || forwardOffset > PlayerWorldTuning.MiningContactTolerance;
        }

        private bool DealDamageInArea(ModelWorld world, APlayer player, Vector2 playerWorldLocation, float deltaTime)
        {
            int halfExtent = Math.Max(0, player.Drill.MiningAreaSize / 2);
            bool damagedAnyBlock = false;

            if (player.FacingDirection.X != 0)
            {
                for (int depth = 1; depth <= player.Drill.MiningAreaSize; depth++)
                {
                    for (int lateral = -halfExtent; lateral <= halfExtent; lateral++)
                    {
                        if (!playerFuelSystem.CanAffordMining(player, deltaTime, true))
                        {
                            player.DrillExtended = false;

                            return damagedAnyBlock;
                        }

                        Vector2 targetVector = new(
                            playerWorldLocation.X + (player.FacingDirection.X * depth),
                            playerWorldLocation.Y + lateral
                        );

                        if (!TryDamageBlock(world, player, targetVector, out bool damagedBlock))
                        {
                            player.DrillExtended = false;

                            return damagedAnyBlock;
                        }

                        damagedAnyBlock |= damagedBlock;
                    }
                }

                return damagedAnyBlock;
            }

            for (int depth = 1; depth <= player.Drill.MiningAreaSize; depth++)
            {
                for (int lateral = -halfExtent; lateral <= halfExtent; lateral++)
                {
                    if (!playerFuelSystem.CanAffordMining(player, deltaTime, true))
                    {
                        player.DrillExtended = false;

                        return damagedAnyBlock;
                    }

                    Vector2 targetVector = new(
                        playerWorldLocation.X + lateral,
                        playerWorldLocation.Y + (player.FacingDirection.Y * depth)
                    );

                    if (!TryDamageBlock(world, player, targetVector, out bool damagedBlock))
                    {
                        player.DrillExtended = false;

                        return damagedAnyBlock;
                    }

                    damagedAnyBlock |= damagedBlock;
                }
            }

            return damagedAnyBlock;
        }

        private bool TryDamageBlock(ModelWorld world, APlayer player, Vector2 targetVector, out bool damagedBlock)
        {
            if (!worldBlockDamageService.TryDamageBlock(world, targetVector, player.Drill.Damage, player.Drill.Hardness, WorldBlockDestroyMethod.Mined, out Block block))
            {
                damagedBlock = false;
                return true;
            }

            float heatGeneration = block.Info?.MiningHeatGeneration ?? 0.0f;
            damagedBlock = true;

            if (player.FacingDirection.Y > 0)
            {
                playerVerticalImpactService.RefreshAfterDownwardMining(player);
            }

            float overflowHeat = playerHeatSystem.AddHeat(player, heatGeneration);

            if (overflowHeat > 0.0f)
            {
                playerHullSystem.ApplyHeatOverflowDamage(player, overflowHeat);
            }

            return true;
        }

        private static bool WillBeDestroyedByHit(Block block, float damage) => !block.Ethereal && block.CurrentHealth <= damage;

        private static void StopHorizontalMovementForVerticalMining(APlayer player)
        {
            player.XVelocity = 0.0f;
            player.XOffset = 0.0f;
        }

        private static void SnapPlayerToMiningBlock(APlayer player)
        {
            player.ResetVelocity();

            if (player.FacingDirection.X != 0)
            {
                player.XOffset = player.FacingDirection.X * PlayerWorldTuning.CollisionPlacementOffset;
                player.YOffset = 0.0f;

                return;
            }

            if (player.FacingDirection.Y != 0)
            {
                player.XOffset = 0.0f;
                player.YOffset = player.FacingDirection.Y * PlayerWorldTuning.CollisionPlacementOffset;
            }
        }

        private bool IsCenteredForMining(APlayer player)
        {
            if (player.FacingDirection.X != 0)
            {
                return Math.Abs(player.YOffset) <= miningCenterTolerance;
            }

            if (player.FacingDirection.Y != 0)
            {
                return Math.Abs(player.XOffset) <= miningCenterTolerance;
            }

            return false;
        }

        private static bool CanContinueMining(APlayer player)
        {
            if (player.FacingDirection == Vector2.Zero)
            {
                return false;
            }

            if (player.FacingDirection.Y < 0)
            {
                return false;
            }

            return Vector2.Dot(player.MovementInput, player.FacingDirection) > 0.0f;
        }

        private static bool IsTouchingMiningSurface(APlayer player)
        {
            if (player.FacingDirection.X != 0)
            {
                return player.XOffset * player.FacingDirection.X >= -PlayerWorldTuning.MiningContactTolerance;
            }

            if (player.FacingDirection.Y != 0)
            {
                return player.YOffset * player.FacingDirection.Y >= -PlayerWorldTuning.MiningContactTolerance;
            }

            return false;
        }

    }
}
