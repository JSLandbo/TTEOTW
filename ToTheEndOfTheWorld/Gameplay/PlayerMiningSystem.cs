using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete;
using System;
using ToTheEndOfTheWorld.Context.StaticRepositories;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class PlayerMiningSystem
    {
        private const float CollisionPlacementOffset = 0.0f;
        private const float MiningContactTolerance = 0.5f;

        private readonly float miningCenterTolerance;
        private readonly WorldQueryService worldQueryService;
        private readonly WorldInteractionsRepository interactions;

        public PlayerMiningSystem(WorldQueryService worldQueryService, WorldInteractionsRepository interactions, int tileSize)
        {
            miningCenterTolerance = tileSize * 0.08f;
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

            var block = worldQueryService.GetWorldBlock(vector.X, vector.Y).Value.Block;

            if (!interactions.ContainsKey(vector))
            {
                block.OnBlockDestroyed += (sender, e) => OnBlockDestroyed(world, vector);
                interactions.Add(vector, block);
            }

            if (interactions[vector].Hardness <= player.Drill.Hardness)
            {
                interactions[vector].TakeDamage(player.Drill.Damage * 1000);
            }
        }

        private static void SnapPlayerToMiningBlock(APlayer player)
        {
            player.ResetVelocity();

            if (player.FacingDirection.X != 0)
            {
                player.XOffset = player.FacingDirection.X * CollisionPlacementOffset;
                player.YOffset = 0.0f;
                return;
            }

            if (player.FacingDirection.Y != 0)
            {
                player.XOffset = 0.0f;
                player.YOffset = player.FacingDirection.Y * CollisionPlacementOffset;
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
                return Math.Abs(player.XOffset) <= MiningContactTolerance;
            }

            if (player.FacingDirection.Y != 0)
            {
                return Math.Abs(player.YOffset) <= MiningContactTolerance;
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
