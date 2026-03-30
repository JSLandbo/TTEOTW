using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerFuelSystem
    {
        public bool CanAffordMovement(APlayer player, float deltaTime)
        {
            return CanAfford(player, deltaTime, includeMovement: true, includeMining: false);
        }

        public bool CanAffordMining(APlayer player, float deltaTime)
        {
            return CanAfford(player, deltaTime, includeMovement: true, includeMining: true);
        }

        public void Update(APlayer player, float deltaTime)
        {
            var fuelUsage = GetFuelUsagePerSecond(player,
                includeMovement: player.MovementInput != Vector2.Zero,
                includeMining: player.Mining
            );

            player.FuelTank.Fuel = Math.Clamp(player.FuelTank.Fuel - fuelUsage * deltaTime, 0.0f, player.FuelTank.Capacity);
        }

        private static bool CanAfford(APlayer player, float deltaTime, bool includeMovement, bool includeMining)
        {
            var requiredFuel = GetFuelUsagePerSecond(player, includeMovement, includeMining) * deltaTime;
            return player.FuelTank.Fuel >= requiredFuel;
        }

        private static float GetFuelUsagePerSecond(APlayer player, bool includeMovement, bool includeMining)
        {
            var fuelUsage = player.Engine.StandbyFuelConsumption;

            if (includeMovement && player.MovementInput != Vector2.Zero)
            {
                fuelUsage += player.Thruster.ActiveFuelConsumption;
            }

            if (includeMining)
            {
                fuelUsage += player.Engine.ActiveFuelConsumption;
            }

            return fuelUsage;
        }
    }
}
