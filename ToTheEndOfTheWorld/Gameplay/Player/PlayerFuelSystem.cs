using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerFuelSystem
    {
        public bool CanAffordMovement(APlayer player, float deltaTime, bool isGrounded)
        {
            return CanAfford(player, deltaTime, isGrounded, includeMovement: true, includeMining: false);
        }

        public bool CanAffordMining(APlayer player, float deltaTime, bool isGrounded)
        {
            return CanAfford(player, deltaTime, isGrounded, includeMovement: true, includeMining: true);
        }

        public void Update(APlayer player, float deltaTime, bool isGrounded)
        {
            float fuelUsage = GetFuelUsagePerSecond(player,
                isGrounded,
                includeMovement: player.MovementInput != Vector2.Zero,
                includeMining: player.Mining
            );

            player.FuelTank.Fuel = Math.Clamp(player.FuelTank.Fuel - fuelUsage * deltaTime, 0.0f, player.FuelTank.Capacity);
        }

        private static bool CanAfford(APlayer player, float deltaTime, bool isGrounded, bool includeMovement, bool includeMining)
        {
            float requiredFuel = GetFuelUsagePerSecond(player, isGrounded, includeMovement, includeMining) * deltaTime;
            return player.FuelTank.Fuel >= requiredFuel;
        }

        private static float GetFuelUsagePerSecond(APlayer player, bool isGrounded, bool includeMovement, bool includeMining)
        {
            float fuelUsage = player.Engine.StandbyFuelConsumption;

            if (includeMovement && player.MovementInput != Vector2.Zero)
            {
                fuelUsage += UsesThrustersForMovement(player, isGrounded)
                    ? player.Thruster.ActiveFuelConsumption
                    : player.Engine.ActiveFuelConsumption;
            }

            if (includeMining)
            {
                fuelUsage += player.Engine.ActiveFuelConsumption;
            }

            return fuelUsage;
        }

        private static bool UsesThrustersForMovement(APlayer player, bool isGrounded)
        {
            return player.MovementInput.Y != 0 || (!isGrounded && player.MovementInput != Vector2.Zero);
        }
    }
}
