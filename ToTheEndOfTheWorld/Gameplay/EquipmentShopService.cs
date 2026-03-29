using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class EquipmentShopService
    {
        private readonly InventoryService inventoryService;
        private readonly GameItemsRepository items;

        public EquipmentShopService(InventoryService inventoryService, GameItemsRepository items)
        {
            this.inventoryService = inventoryService;
            this.items = items;
        }

        public bool TryBuy(World world, ABuilding building, int slotX, int slotY)
        {
            if (building.StorageGrid == null)
            {
                return false;
            }

            var grid = building.StorageGrid.InternalGrid;

            if (slotX < 0 || slotX >= grid.GetLength(0) || slotY < 0 || slotY >= grid.GetLength(1))
            {
                return false;
            }

            var slot = grid[slotX, slotY];

            if (slot.Item == null)
            {
                return false;
            }

            if (world.Player.Cash < slot.Item.Worth)
            {
                return false;
            }

            // Shop slots hold item definitions; buying creates a fresh concrete item.
            var purchasedItem = items.Create(slot.Item.ID);

            if (purchasedItem == null || !inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1))
            {
                return false;
            }

            world.Player.Cash -= slot.Item.Worth;
            return true;
        }
    }
}
