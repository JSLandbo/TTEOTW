using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Buildings
{
    public sealed class Building : ABuilding
    {
        [JsonConstructor]
        public Building(
            short ID,
            string Name,
            long WorldX,
            long WorldY,
            int XOffset,
            int YOffset,
            int TilesWide,
            int TilesHigh,
            AGrid StorageGrid,
            bool IsBackground = true,
            EBuildingInteraction Interaction = EBuildingInteraction.None,
            bool ShowPlayerInventoryWhenOpen = false
        ) : base(ID, Name, WorldX, WorldY, XOffset, YOffset, TilesWide, TilesHigh, StorageGrid, IsBackground, Interaction, ShowPlayerInventoryWhenOpen)
        {
        }
    }
}
