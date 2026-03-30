using System;
using ModelLibrary.Abstract;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerVerticalImpactService
    {
        private const int MinimumProtectionFramesAfterDownwardMining = 4;
        private readonly PlayerHullSystem playerHullSystem;
        private int framesRemaining;
        private bool isProtectedThisFrame;
        private float downwardImpactVelocityThisStep;

        public PlayerVerticalImpactService(PlayerHullSystem playerHullSystem)
        {
            this.playerHullSystem = playerHullSystem;
        }

        public void BeginFrame()
        {
            isProtectedThisFrame = framesRemaining > 0;
            downwardImpactVelocityThisStep = 0.0f;

            if (framesRemaining > 0)
            {
                framesRemaining--;
            }
        }

        public void BeginResolveStep(APlayer player)
        {
            downwardImpactVelocityThisStep = Math.Max(0.0f, player.YVelocity);
        }

        public bool CanStartDownwardMining(APlayer player)
        {
            if (player.DrillExtended)
            {
                return true;
            }

            return !playerHullSystem.WouldTakeFallDamage(downwardImpactVelocityThisStep);
        }

        public void ApplyFallDamageIfNeeded(APlayer player)
        {
            if (isProtectedThisFrame)
            {
                return;
            }

            playerHullSystem.ApplyFallDamage(player, downwardImpactVelocityThisStep);
        }

        public void RefreshAfterDownwardMining(APlayer player)
        {
            framesRemaining = Math.Max(
                framesRemaining,
                Math.Max(MinimumProtectionFramesAfterDownwardMining, player.Drill.MiningAreaSize));
        }

        public void Clear()
        {
            framesRemaining = 0;
            isProtectedThisFrame = false;
        }
    }
}
