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
            if (!blockLootResolver.TryResolve(gameEvent.BlockId, out ModelLibrary.Concrete.Blocks.Block loot, out int count))
            {
                return;
            }

            inventoryService.TryAdd(gameEvent.World.Player.Inventory, loot, count);
        }
    }
}
