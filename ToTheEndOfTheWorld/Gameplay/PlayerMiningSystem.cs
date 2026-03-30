using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete;
using System;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class PlayerMiningSystem
    {
        private readonly float miningCenterTolerance;
        private readonly WorldBlockDefinitionResolver worldBlockDefinitionResolver;
        private readonly WorldBlockFactory worldBlockFactory;
        private readonly WorldInteractionsRepository interactions;
        private readonly GameEventBus eventBus;

        public PlayerMiningSystem(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldBlockFactory worldBlockFactory, WorldInteractionsRepository interactions, GameEventBus eventBus, int tileSize)
        {
            miningCenterTolerance = tileSize * PlayerWorldTuning.MiningCenterToleranceRatio;
            this.worldBlockDefinitionResolver = worldBlockDefinitionResolver;
            this.worldBlockFactory = worldBlockFactory;
            this.interactions = interactions;
            this.eventBus = eventBus;
        }

        public void Update(World world, APlayer player)
        {
            player.Mining = false;

            if (!CanContinueMining(player))
            {
                player.DrillExtended = false;
                return;
            }
            
            var location = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            var blockVector = new Vector2(location.X + player.FacingDirection.X, location.Y + player.FacingDirection.Y);
            var worldTile = new WorldTile((long)blockVector.X, (long)blockVector.Y);

            if (!PlayerGroundingService.IsGrounded(world, player, worldBlockDefinitionResolver))
            {
                player.DrillExtended = ShouldKeepDrillExtendedWhileAdvancing(player);
                return;
            }

            if (!IsTouchingMiningSurface(player))
            {
                player.DrillExtended = ShouldKeepDrillExtendedWhileAdvancing(player);
                return;
            }

            if (!IsCenteredForMining(player))
            {
                player.DrillExtended = false;
                return;
            }

            if (IsInsideBuilding(world, worldTile))
            {
                player.DrillExtended = false;
                return;
            }

            if (!worldBlockDefinitionResolver.IsObstructed(world, blockVector))
            {
                player.DrillExtended = ShouldKeepDrillExtendedWhileAdvancing(player);
                return;
            }

            var interaction = GetOrCreateMiningInteraction(world, blockVector);

            if (interaction.Block.Hardness > player.Drill.Hardness)
            {
                player.DrillExtended = false;
                return;
            }

            // Keep the old snap-and-stop behavior for blocks that survive the hit,
            // but let one-shot blocks break before they steal the ship's movement.
            if (!WillBeDestroyedByHit(interaction.Block, player.Drill.Damage))
            {
                SnapPlayerToMiningBlock(player);
            }

            player.Mining = true;
            player.DrillExtended = true;
            DealDamageInArea(world, player, location);
        }

        private static bool ShouldKeepDrillExtendedWhileAdvancing(APlayer player)
        {
            if (!player.DrillExtended || player.FacingDirection == Vector2.Zero) return false;

            var forwardVelocity = (player.XVelocity * player.FacingDirection.X) + (player.YVelocity * player.FacingDirection.Y);
            var forwardOffset = (player.XOffset * player.FacingDirection.X) + (player.YOffset * player.FacingDirection.Y);

            return forwardVelocity > PlayerWorldTuning.VelocityStopThreshold || forwardOffset > PlayerWorldTuning.MiningContactTolerance;
        }

        private void DealDamageInArea(World world, APlayer player, Vector2 playerWorldLocation)
        {
            var halfExtent = Math.Max(0, player.Drill.MiningAreaSize / 2);

            if (player.FacingDirection.X != 0)
            {
                for (var depth = 1; depth <= player.Drill.MiningAreaSize; depth++)
                {
                    for (var lateral = -halfExtent; lateral <= halfExtent; lateral++)
                    {
                        var targetVector = new Vector2(
                            playerWorldLocation.X + (player.FacingDirection.X * depth),
                            playerWorldLocation.Y + lateral);
                        TryDamageBlock(world, player, targetVector);
                    }
                }

                return;
            }

            for (var depth = 1; depth <= player.Drill.MiningAreaSize; depth++)
            {
                for (var lateral = -halfExtent; lateral <= halfExtent; lateral++)
                {
                    var targetVector = new Vector2(
                        playerWorldLocation.X + lateral,
                        playerWorldLocation.Y + (player.FacingDirection.Y * depth));
                    TryDamageBlock(world, player, targetVector);
                }
            }
        }

        private void TryDamageBlock(World world, APlayer player, Vector2 targetVector)
        {
            var targetTile = new WorldTile((long)targetVector.X, (long)targetVector.Y);

            if (IsInsideBuilding(world, targetTile) || !worldBlockDefinitionResolver.IsObstructed(world, targetVector))
            {
                return;
            }

            var interaction = GetOrCreateMiningInteraction(world, targetVector);

            if (interaction.Block.Hardness <= player.Drill.Hardness)
            {
                interaction.Block.TakeDamage(player.Drill.Damage);
            }
        }

        private WorldInteraction GetOrCreateMiningInteraction(World world, Vector2 vector)
        {
            var worldTile = new WorldTile((long)vector.X, (long)vector.Y);

            if (!interactions.TryGet(worldTile, WorldInteractionType.Mining, out var interaction))
            {
                var block = worldBlockFactory.CreateMutableWorldBlock(vector.X, vector.Y);
                interaction = new WorldInteraction(WorldInteractionType.Mining, new WorldTileBounds(worldTile.X, worldTile.Y, 1, 1), block);
                block.OnBlockDestroyed += (sender, e) => OnBlockDestroyed(world, interaction);
                interactions.Add(interaction);
            }

            return interaction;
        }

        private static bool WillBeDestroyedByHit(ModelLibrary.Concrete.Blocks.Block block, float damage) =>
            !block.Ethereal && block.CurrentHealth <= damage;

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

        private void OnBlockDestroyed(World world, WorldInteraction interaction)
        {
            var location = new Vector2(interaction.TileBounds.X, interaction.TileBounds.Y);
            world.WorldTrails.Add(location, true);
            interactions.Remove(interaction);
            eventBus.Publish(new WorldBlockDestroyedEvent(world, interaction.Block.ID, new WorldTile(interaction.TileBounds.X, interaction.TileBounds.Y)));
        }

        private static bool IsInsideBuilding(World world, WorldTile tile)
        {
            if (world.Buildings == null)
            {
                return false;
            }

            foreach (var building in world.Buildings)
            {
                if (building.ContainsTile(tile.X, tile.Y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
