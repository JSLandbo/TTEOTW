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

        public bool CanBuyGadget(ModelWorld world, AType item)
        {
            if (item == null) return false;

            // Check stackable space in inventory first, then gadget slots
            int inventorySpace = inventoryService.GetStackableSpace(world.Player.Inventory, item);
            int gadgetSpace = world.Player.HasGadgetBelt ? inventoryService.GetStackableSpace(world.Player.GadgetSlots, item) : 0;

            if (inventorySpace > 0 || gadgetSpace > 0) return true;

            // Check for empty slots that can accept this item
            return inventoryService.HasEmptySlotFor(world.Player.Inventory, item)
                || (world.Player.HasGadgetBelt && inventoryService.HasEmptySlotFor(world.Player.GadgetSlots, item));
        }

        public bool TryBuyGadget(ModelWorld world, ABuilding building, int slotX, int slotY)
        {
            if (building?.StorageGrid?.InternalGrid == null)
            {
                return false;
            }

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

            // Try inventory first, then gadget slots (if owned)
            if (!inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1)
                && (!world.Player.HasGadgetBelt || !inventoryService.TryAdd(world.Player.GadgetSlots, purchasedItem, 1)))
            {
                return false;
            }

            world.Player.Cash -= slot.Item.Worth;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Bought));
            return true;
        }
    }
}
