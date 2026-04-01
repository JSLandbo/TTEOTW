using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Ids;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldBlockLootSystem
    {
        private const int UtilitySlotStartIndex = 4;
        private const int UtilitySlotCount = 2;
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
            if (!blockLootResolver.TryResolve(gameEvent.BlockId, out Block loot, out int count))
            {
                return;
            }

            if (ShouldDiscardFilteredLoot(gameEvent.World.Player, loot))
            {
                return;
            }

            inventoryService.TryAdd(gameEvent.World.Player.Inventory, loot, count);
        }

        private static bool ShouldDiscardFilteredLoot(APlayer player, Block loot)
        {
            if (loot == null) return false;

            if (loot.ID == GameIds.Blocks.Dirt)
            {
                return HasUtilityItem(player, GameIds.Items.Gadgets.DirtFilter);
            }

            if (loot.ID == GameIds.Blocks.Rock)
            {
                return HasUtilityItem(player, GameIds.Items.Gadgets.RockFilter);
            }

            return false;
        }

        private static bool HasUtilityItem(APlayer player, short itemId)
        {
            for (int x = UtilitySlotStartIndex; x < UtilitySlotStartIndex + UtilitySlotCount; x++)
            {
                AGridBox slot = player.GadgetSlots.Items.InternalGrid[x, 0];

                if (slot?.Item?.ID == itemId && slot.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
