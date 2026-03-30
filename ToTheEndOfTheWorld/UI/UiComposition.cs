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
