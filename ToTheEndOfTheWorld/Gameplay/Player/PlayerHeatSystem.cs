using System;
using ModelLibrary.Abstract;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerHeatSystem
    {
        public bool CanUseThrusters(APlayer player)
        {
            return !IsCapped(player);
        }

        public float AddThrusterHeat(APlayer player, float deltaTime)
        {
            return AddHeat(player, player.Thruster.ActiveHeatGeneration * deltaTime);
        }

        public void Update(APlayer player, float deltaTime)
        {
            float cooledThermals = player.CurrentHeat - (player.ThermalPlating.ThermalDissipation * deltaTime);
            player.CurrentHeat = Math.Clamp(cooledThermals, 0.0f, player.ThermalPlating.MaxThermals);
        }

        public float AddHeat(APlayer player, float heatAmount)
        {
            if (heatAmount <= 0.0f)
            {
                return 0.0f;
            }

            float nextHeat = player.CurrentHeat + heatAmount;
            float overflowHeat = Math.Max(0.0f, nextHeat - player.ThermalPlating.MaxThermals);
            player.CurrentHeat = Math.Clamp(nextHeat, 0.0f, player.ThermalPlating.MaxThermals);

            return overflowHeat;
        }

        public bool IsCapped(APlayer player)
        {
            return player.CurrentHeat >= player.ThermalPlating.MaxThermals;
        }
    }
}
