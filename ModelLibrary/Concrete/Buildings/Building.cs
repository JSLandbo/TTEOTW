using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Enums;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Buildings
{
    [method: JsonConstructor]
    public sealed class Building(
        short ID,
        string Name,
        long WorldX,
        long WorldY,
        int XOffset,
        int YOffset,
        int TilesWide,
        int TilesHigh,
        AInventory StorageInventory,
        bool IsBackground = true,
        EBuildingInteraction Interaction = EBuildingInteraction.None,
        bool ShowPlayerInventoryWhenOpen = false
        ) : ABuilding(ID, Name, WorldX, WorldY, XOffset, YOffset, TilesWide, TilesHigh, StorageInventory, IsBackground, Interaction, ShowPlayerInventoryWhenOpen)
    {
    }
}
