namespace ToTheEndOfTheWorld.UI
{
    public static class UiComposition
    {
        public static UiManager Create(
            Gameplay.InventoryService inventoryService,
            Gameplay.CraftingService craftingService,
            Context.StaticRepositories.WorldElementsRepository blocks,
            Context.StaticRepositories.GameItemsRepository items)
        {
            var uiManager = new UiManager();
            uiManager.Register(new Inventory.InventoryOverlay(inventoryService, craftingService, blocks, items));
            return uiManager;
        }
    }
}
