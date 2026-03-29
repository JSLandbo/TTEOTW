using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.UI
{
    public static class UiComposition
    {
        public static UiManager Create(
            Gameplay.InventoryService inventoryService,
            Gameplay.CraftingService craftingService,
            Gameplay.InventoryItemUseService itemUseService,
            Gameplay.ShopService shopService,
            Gameplay.EquipmentShopService equipmentShopService,
            WorldElementsRepository blocks,
            GameItemsRepository items)
        {
            var uiManager = new UiManager();
            uiManager.Register(new Inventory.InventoryOverlay(inventoryService, craftingService, itemUseService, blocks, items));
            uiManager.Register(new Shop.ShopOverlay(shopService, blocks, items));
            uiManager.Register(new Shop.EquipmentShopOverlay(equipmentShopService, blocks, items));
            return uiManager;
        }
    }
}
