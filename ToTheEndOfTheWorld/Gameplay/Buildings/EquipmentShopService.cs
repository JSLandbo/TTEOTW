using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.PlayerShipComponents;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class EquipmentShopService(InventoryItemUseService itemUseService, InventoryService inventoryService, GameItemsRepository items, GameEventBus eventBus)
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

            bool wasEquipped = itemUseService.TryEquip(world, purchasedItem);

            if (!wasEquipped && !inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1))
            {
                return false;
            }

            if (wasEquipped)
            {
                ApplyFreshPurchasedState(world, purchasedItem);
            }

            world.Player.Cash -= slot.Item.Worth;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));

            return true;
        }

        private static void ApplyFreshPurchasedState(ModelWorld world, AType purchasedItem)
        {
            switch (purchasedItem)
            {
                case ThermalPlating:
                    world.Player.CurrentHeat = 0.0f;
                    break;
                case Hull:
                    world.Player.CurrentHull = world.Player.Hull.Health;
                    break;
                case FuelTank:
                    world.Player.CurrentFuel = world.Player.FuelTank.Capacity;
                    break;
            }
        }
    }
}
