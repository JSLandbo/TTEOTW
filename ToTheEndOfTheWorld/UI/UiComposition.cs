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
            uiManager.Register(new Inventory.InventoryOverlay(inventoryService, craftingService, itemUseService, blocks, items, () => uiManager.HasOpenInteractionOverlay, (world, slot) => uiManager.TryShopSellSlot(world, slot)));
            uiManager.Register(new Shop.ShopOverlay(shopService, blocks, items));
            uiManager.Register(new Shop.EquipmentShopOverlay(equipmentShopService, blocks, items, () => uiManager.InventoryHasHeldItem));
            uiManager.Register(new Shop.FuelStationOverlay(fuelStationService));
            uiManager.Register(new Shop.GadgetShopOverlay(gadgetShopService, blocks, items, () => uiManager.InventoryHasHeldItem));
            return uiManager;
        }
    }
}
