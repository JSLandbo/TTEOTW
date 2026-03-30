using ModelLibrary.Abstract;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerHeatSystem
    {
        public bool CanAffordThrusterHeat(APlayer player, float deltaTime)
        {
            return !player.ThermalPlating.WouldOverheat(player.Thruster.ActiveHeatGeneration * deltaTime);
        }

        public void AddThrusterHeat(APlayer player, float deltaTime)
        {
            AddHeat(player, player.Thruster.ActiveHeatGeneration * deltaTime);
        }

        public void Update(APlayer player, float deltaTime)
        {
            float cooledThermals = player.ThermalPlating.Thermals - (player.ThermalPlating.ThermalDissipation * deltaTime);
            player.ThermalPlating.Thermals = Math.Clamp(cooledThermals, 0.0f, player.ThermalPlating.MaxThermals);
        }

        public void AddHeat(APlayer player, float heatAmount)
        {
            if (heatAmount <= 0.0f)
            {
                return;
            }

            player.ThermalPlating.Thermals = Math.Clamp(
                player.ThermalPlating.Thermals + heatAmount,
                0.0f,
                player.ThermalPlating.MaxThermals);
        }

        public bool WouldOverheat(APlayer player, float heatAmount)
        {
            return player.ThermalPlating.WouldOverheat(heatAmount);
        }
    }
}
