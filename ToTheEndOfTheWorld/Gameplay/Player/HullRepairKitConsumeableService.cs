using System;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class HullRepairKitConsumeableService(GameEventBus eventBus)
    {
        public bool TryUse(ModelWorld world, AHullRepairKit hullRepairKit)
        {
            world.Player.CurrentHull = Math.Min(world.Player.Hull.Health, world.Player.CurrentHull + hullRepairKit.RepairAmount);
            eventBus.Publish(new ConsumeableUsedEvent(hullRepairKit));
            return true;
        }
    }
}
