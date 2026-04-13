using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Items;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class TeleporterConsumeableService(WorldViewportService worldViewportService, GameEventBus eventBus)
    {
        public bool TryUse(ModelWorld world, ATeleporter teleporter)
        {
            if (teleporter is WorldSpawnTeleporter)
            {
                worldViewportService.EnsurePadding(world, world.SpawnWorldPosition);
                eventBus.Publish(new ConsumeableUsedEvent(teleporter));
                return true;
            }

            return false;
        }
    }
}
