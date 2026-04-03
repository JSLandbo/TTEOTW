using System;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class CoolantPatchConsumeableService(GameEventBus eventBus)
    {
        public bool TryUse(ModelWorld world, ACoolantPatch coolantPatch)
        {
            world.Player.CurrentHeat = Math.Max(0.0f, world.Player.CurrentHeat - coolantPatch.CoolingAmount);
            eventBus.Publish(new ConsumeableUsedEvent(coolantPatch));
            return true;
        }
    }
}
