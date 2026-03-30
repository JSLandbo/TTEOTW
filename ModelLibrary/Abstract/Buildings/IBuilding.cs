using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;

namespace ModelLibrary.Abstract.Buildings
{
    public interface IBuilding
    {
        public short ID { get; set; }
        public string Name { get; set; }
        public long WorldX { get; set; }
        public long WorldY { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int TilesWide { get; set; }
        public int TilesHigh { get; set; }
        public AGrid StorageGrid { get; set; }
        public bool IsBackground { get; set; }
        public bool IsDestructible { get; set; }
        public EBuildingInteraction Interaction { get; set; }
        public string InteractionPrompt { get; set; }
        public bool ContainsTile(long worldX, long worldY);
    }
}
