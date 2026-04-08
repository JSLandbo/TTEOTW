using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class SellShopBuildingFactory
    {
        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.SellShop,
                Name: "Shop",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 4,
                TilesHigh: 2,
                StorageInventory: null,
                IsBackground: true,
                Interaction: EBuildingInteraction.Shop,
                ShowPlayerInventoryWhenOpen: true);
        }
    }
}
