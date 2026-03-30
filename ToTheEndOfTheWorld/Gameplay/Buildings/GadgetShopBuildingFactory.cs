using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class GadgetShopBuildingFactory
    {
        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.GadgetShop,
                Name: "Gadget Shop",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 3,
                TilesHigh: 3,
                StorageGrid: null,
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.GadgetShop,
                InteractionPrompt: "Press E to open shop");
        }
    }
}
