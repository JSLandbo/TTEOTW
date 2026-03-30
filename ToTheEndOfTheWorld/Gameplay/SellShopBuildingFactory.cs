using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class SellShopBuildingFactory
    {
        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: 1,
                Name: "Shop",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 4,
                TilesHigh: 2,
                StorageGrid: null,
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.Shop,
                InteractionPrompt: "Press E to open shop");
        }
    }
}
