using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;

namespace ModelLibrary.Concrete.Buildings
{
    public sealed class Building(
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
        bool IsDestructible = false,
        EBuildingInteraction Interaction = EBuildingInteraction.None
    ) : ABuilding(ID, Name, WorldX, WorldY, XOffset, YOffset, TilesWide, TilesHigh, StorageGrid, IsBackground, IsDestructible, Interaction);
}
