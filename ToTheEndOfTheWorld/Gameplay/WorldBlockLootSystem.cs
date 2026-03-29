using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldBlockLootSystem
    {
        private readonly BlockLootResolver blockLootResolver;
        private readonly InventoryService inventoryService;

        public WorldBlockLootSystem(GameEventBus eventBus, BlockLootResolver blockLootResolver, InventoryService inventoryService)
        {
            this.blockLootResolver = blockLootResolver;
            this.inventoryService = inventoryService;
            eventBus.Subscribe<WorldBlockDestroyedEvent>(OnWorldBlockDestroyed);
        }

        private void OnWorldBlockDestroyed(WorldBlockDestroyedEvent gameEvent)
        {
            if (!blockLootResolver.TryResolve(gameEvent.BlockId, out var loot, out var count))
            {
                return;
            }

            inventoryService.TryAdd(gameEvent.World.Player.Inventory, loot, count);
        }
    }
}
