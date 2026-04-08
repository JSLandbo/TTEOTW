using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI
{
    public static class UiComposition
    {
        public static UiManager Create(
            InventoryService inventoryService,
            CraftingService craftingService,
            InventoryItemUseService itemUseService,
            ShopService shopService,
            EquipmentShopService equipmentShopService,
            FuelStationService fuelStationService,
            GadgetShopService gadgetShopService,
            WorldElementsRepository blocks,
            GameItemsRepository items)
        {
            UiManager uiManager = new();
            Shop.StorageChestOverlay chestOverlay = new(inventoryService, blocks, items, () => uiManager.InventoryHasHeldItem);

            uiManager.Register(new Inventory.InventoryOverlay(
                inventoryService,
                craftingService,
                itemUseService,
                blocks,
                items,
                () => uiManager.HasOpenShopOverlay,
                (world, slot) => TrySellOrTransferSlot(uiManager, chestOverlay, world, slot),
                (pos, vw, vh) => TryGetChestSlot(chestOverlay, pos, vw, vh),
                chestOverlay.GetOpenInventory));

            uiManager.Register(new Shop.ShopOverlay(shopService, blocks, items));
            uiManager.Register(new Shop.EquipmentShopOverlay(equipmentShopService, blocks, items, () => uiManager.InventoryHasHeldItem));
            uiManager.Register(new Shop.FuelStationOverlay(fuelStationService));
            uiManager.Register(new Shop.GadgetShopOverlay(gadgetShopService, blocks, items, () => uiManager.InventoryHasHeldItem));
            uiManager.Register(chestOverlay);

            return uiManager;
        }

        private static bool TrySellOrTransferSlot(UiManager uiManager, Shop.StorageChestOverlay chestOverlay, ModelWorld world, AGridBox slot)
        {
            return uiManager.TryShopSellSlot(world, slot) || chestOverlay.TryTransferToChest(slot);
        }

        private static (AGridBox slot, int maxStackSize)? TryGetChestSlot(Shop.StorageChestOverlay chestOverlay, Point pos, int vw, int vh)
        {
            // Don't allow drag-and-drop interaction when CTRL is held (transfer mode)
            if (!chestOverlay.IsOpen || chestOverlay.IsTransferModeActive)
            {
                return null;
            }

            if (chestOverlay.TryGetClickedChestSlot(pos, vw, vh, out AGridBox slot))
            {
                return (slot, chestOverlay.GetMaxStackSize());
            }

            return null;
        }
    }
}
