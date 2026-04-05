using System;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class FuelStationService(GameEventBus eventBus)
    {
        private const float FuelPrice = 0.10f;

        public float FuelPricePerUnit => FuelPrice;

        public float TryRefuelAllAffordable(ModelWorld world)
        {
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;

            if (missingFuel <= 0.0f || world.Player.Cash <= 0.0)
            {
                return 0.0f;
            }

            float fuelPurchased = MathF.Min(missingFuel, (float)(world.Player.Cash / FuelPrice));
            world.Player.CurrentFuel = Math.Min(world.Player.CurrentFuel + fuelPurchased, world.Player.FuelTank.Capacity);
            world.Player.Cash = Math.Max(0.0, world.Player.Cash - (fuelPurchased * FuelPrice));
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));
            return fuelPurchased;
        }
    }
}
