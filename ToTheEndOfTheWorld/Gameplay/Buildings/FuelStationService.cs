using System;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class FuelStationService(GameEventBus eventBus)
    {
        public float TryRefuelAllAffordable(ModelWorld world)
        {
            float fuelPrice = 0.25f;
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;

            if (missingFuel <= 0.0f || world.Player.Cash <= 0.0)
            {
                return 0.0f;
            }

            float fuelPurchased = MathF.Min(missingFuel, (float)(world.Player.Cash / fuelPrice));
            world.Player.CurrentFuel += fuelPurchased;
            world.Player.Cash -= fuelPurchased * fuelPrice;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));
            return fuelPurchased;
        }
    }
}
