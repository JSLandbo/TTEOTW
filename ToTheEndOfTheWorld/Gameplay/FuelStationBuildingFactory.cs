using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class FuelStationBuildingFactory
    {
        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: 3,
                Name: "Fuel Station",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 2,
                TilesHigh: 2,
                StorageGrid: null,
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.FuelStation,
                InteractionPrompt: string.Empty);
        }
    }
}
