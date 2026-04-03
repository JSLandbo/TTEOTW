using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class GadgetShopService(InventoryService inventoryService, GameItemsRepository items, GameEventBus eventBus)
    {
        private const double GadgetBeltPrice = 10000.0;

        public double GadgetBeltPriceValue => GadgetBeltPrice;

        public bool TryBuyGadgetBelt(ModelWorld world)
        {
            if (world.Player.HasGadgetBelt || world.Player.Cash < GadgetBeltPrice)
            {
                return false;
            }

            world.Player.HasGadgetBelt = true;
            world.Player.Cash -= GadgetBeltPrice;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));
            return true;
        }

        public bool TryBuyGadget(ModelWorld world, ABuilding building, int slotX, int slotY)
        {
            AGridBox[,] grid = building.StorageGrid.InternalGrid;

            if (slotX < 0 || slotX >= grid.GetLength(0) || slotY < 0 || slotY >= grid.GetLength(1))
            {
                return false;
            }

            AGridBox slot = grid[slotX, slotY];

            if (slot.Item == null || world.Player.Cash < slot.Item.Worth)
            {
                return false;
            }

            AType purchasedItem = items.Create(slot.Item.ID);

            if (!inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1))
            {
                return false;
            }

            world.Player.Cash -= slot.Item.Worth;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));
            return true;
        }
    }
}
