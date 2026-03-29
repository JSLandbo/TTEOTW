using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete;
using System;
using ToTheEndOfTheWorld.Context.StaticRepositories;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class PlayerMiningSystem
    {
        private readonly float miningCenterTolerance;
        private readonly WorldQueryService worldQueryService;
        private readonly WorldInteractionsRepository interactions;

        public PlayerMiningSystem(WorldQueryService worldQueryService, WorldInteractionsRepository interactions, int tileSize)
        {
            miningCenterTolerance = tileSize * PlayerWorldTuning.MiningCenterToleranceRatio;
            this.worldQueryService = worldQueryService;
            this.interactions = interactions;
        }

        public void Update(World world, APlayer player)
        {
            player.Mining = false;

            if (!CanContinueMining(player))
            {
                player.DrillExtended = false;
                return;
            }

            if (!IsGrounded(world, player))
            {
                player.DrillExtended = false;
                return;
            }

            if (!IsTouchingMiningSurface(player))
            {
                return;
            }

            if (!IsCenteredForMining(player))
            {
                return;
            }

            var location = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            var blockVector = new Vector2(location.X + player.FacingDirection.X, location.Y + player.FacingDirection.Y);

            if (!worldQueryService.IsObstructed(world, blockVector))
            {
                return;
            }

            SnapPlayerToMiningBlock(player);
            player.Mining = true;
            player.DrillExtended = true;
            DealDamageToBlock(world, player, blockVector);
        }

        private void DealDamageToBlock(World world, APlayer player, Vector2 vector)
        {
            if (world.WorldTrails.ContainsKey(vector))
            {
                return;
            }

            var block = worldQueryService.CreateMutableWorldBlock(vector.X, vector.Y);

            if (!interactions.ContainsKey(vector))
            {
                block.OnBlockDestroyed += (sender, e) => OnBlockDestroyed(world, vector);
                interactions.Add(vector, block);
            }

            if (interactions[vector].Hardness <= player.Drill.Hardness)
            {
                interactions[vector].TakeDamage(player.Drill.Damage);
            }
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

        private bool IsGrounded(World world, APlayer player)
        {
            var location = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            var belowPlayer = new Vector2(location.X, location.Y + 1);

            return worldQueryService.IsObstructed(world, belowPlayer);
        }

        private static bool IsTouchingMiningSurface(APlayer player)
        {
            if (player.FacingDirection.X != 0)
            {
                return Math.Abs(player.XOffset) <= PlayerWorldTuning.MiningContactTolerance;
            }

            if (player.FacingDirection.Y != 0)
            {
                return Math.Abs(player.YOffset) <= PlayerWorldTuning.MiningContactTolerance;
            }

            return false;
        }

        private void OnBlockDestroyed(World world, Vector2 location)
        {
            world.WorldTrails.Add(location, true);
            interactions.Remove(location);
        }
    }
}
