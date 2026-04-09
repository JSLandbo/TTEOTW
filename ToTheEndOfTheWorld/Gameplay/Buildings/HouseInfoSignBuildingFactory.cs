using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class HouseInfoSignBuildingFactory
    {
        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.HouseInfoSign,
                Name: "Welcome Sign",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 12,
                TilesWide: 2,
                TilesHigh: 2,
                StorageInventory: null,
                IsBackground: true,
                Interaction: EBuildingInteraction.HouseInfoSign);
        }
    }
}
