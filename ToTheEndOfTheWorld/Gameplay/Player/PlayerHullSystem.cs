using System;
using ModelLibrary.Abstract;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerHullSystem(GameEventBus eventBus)
    {
        private const float FallDamageVelocityThreshold = 600.0f;
        private const float FallDamagePerExcessVelocity = 0.25f;
        private const float ExplosionChancePerSecondAtMaxHeat = 0.25f;
        private const float MinimumExplosionDamage = 05.0f;
        private const float MaximumExplosionDamage = 10.0f;

        public void Update(APlayer player, float deltaTime)
        {
            float heatRatio = GetHeatRatio(player);

            if (heatRatio >= 0.95f)
            {
                float explosionChance = ExplosionChancePerSecondAtMaxHeat * deltaTime;

                if (Random.Shared.NextSingle() < explosionChance)
                {
                    ApplyExplosionDamage(player, Lerp(MinimumExplosionDamage, MaximumExplosionDamage, Random.Shared.NextSingle()));
                    eventBus.Publish(new ScreenEffectRequestedEvent(ScreenEffectType.Explosion));
                }
            }
        }

        public void ApplyFallDamage(APlayer player, float impactVelocity)
        {
            float excessVelocity = impactVelocity - FallDamageVelocityThreshold;

            if (excessVelocity <= 0.0f)
            {
                return;
            }

            float damage = excessVelocity * FallDamagePerExcessVelocity;
            ApplyDamage(player, damage);
            eventBus.Publish(new PlayerFallDamageEvent(damage));
        }

        public bool WouldTakeFallDamage(float impactVelocity) => impactVelocity > FallDamageVelocityThreshold;

        public void ApplyExplosionDamage(APlayer player, float damage)
        {
            ApplyDamage(player, damage);
        }

        public void ApplyHeatOverflowDamage(APlayer player, float overflowHeat)
        {
            ApplyDamage(player, overflowHeat);
        }

        private static float GetHeatRatio(APlayer player)
        {
            if (player.ThermalPlating.MaxThermals <= 0.0f)
            {
                return 0.0f;
            }

            return Math.Clamp(player.CurrentHeat / player.ThermalPlating.MaxThermals, 0.0f, 1.0f);
        }

        private static void ApplyDamage(APlayer player, float damage)
        {
            if (damage <= 0.0f)
            {
                return;
            }

            player.CurrentHull = Math.Max(0.0f, player.CurrentHull - damage);
        }

        private static float Lerp(float start, float end, float amount) => start + ((end - start) * amount);
    }
}
