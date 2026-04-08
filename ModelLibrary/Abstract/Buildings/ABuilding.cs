using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Enums;

namespace ModelLibrary.Abstract.Buildings
{
    public abstract class ABuilding(
        short ID,
        string Name,
        long WorldX,
        long WorldY,
        int XOffset,
        int YOffset,
        int TilesWide,
        int TilesHigh,
        AInventory StorageInventory,
        bool IsBackground,
        EBuildingInteraction Interaction,
        bool ShowPlayerInventoryWhenOpen = false) : IBuilding
    {
        public short ID { get; set; } = ID;
        public string Name { get; set; } = Name;
        public long WorldX { get; set; } = WorldX;
        public long WorldY { get; set; } = WorldY;
        public int XOffset { get; set; } = XOffset;
        public int YOffset { get; set; } = YOffset;
        public int TilesWide { get; set; } = TilesWide;
        public int TilesHigh { get; set; } = TilesHigh;
        public AInventory StorageInventory { get; set; } = StorageInventory;
        public bool IsBackground { get; set; } = IsBackground;
        public EBuildingInteraction Interaction { get; set; } = Interaction;
        public bool ShowPlayerInventoryWhenOpen { get; set; } = ShowPlayerInventoryWhenOpen;

        public bool ContainsTile(long worldX, long worldY)
        {
            return worldX >= WorldX &&
                   worldX < WorldX + TilesWide &&
                   worldY >= WorldY &&
                   worldY < WorldY + TilesHigh;
        }
    }
}
