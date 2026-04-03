using System;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class FuelCapsuleConsumeableService(GameEventBus eventBus)
    {
        public bool TryUse(ModelWorld world, AFuelCapsule fuelCapsule)
        {
            world.Player.CurrentFuel = Math.Min(world.Player.FuelTank.Capacity, world.Player.CurrentFuel + fuelCapsule.FuelAmount);
            eventBus.Publish(new ConsumeableUsedEvent(fuelCapsule));
            return true;
        }
    }
}
