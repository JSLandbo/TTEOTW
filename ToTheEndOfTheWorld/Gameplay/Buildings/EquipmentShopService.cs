using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class EquipmentShopService(InventoryService inventoryService, GameItemsRepository items)
    {
        public bool TryBuy(ModelWorld world, ABuilding building, int slotX, int slotY)
        {
            AGridBox[,] grid = building.StorageGrid.InternalGrid;

            if (slotX < 0 || slotX >= grid.GetLength(0) || slotY < 0 || slotY >= grid.GetLength(1))
            {
                return false;
            }

            AGridBox slot = grid[slotX, slotY];

            if (slot.Item == null)
            {
                return false;
            }

            if (world.Player.Cash < slot.Item.Worth)
            {
                return false;
            }

            // Shop slots hold item definitions; buying creates a fresh concrete item.
            AType purchasedItem = items.Create(slot.Item.ID);

            if (!inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1))
            {
                return false;
            }

            world.Player.Cash -= slot.Item.Worth;

            return true;
        }
    }
}
