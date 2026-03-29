using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;

namespace ModelLibrary.Abstract.Buildings
{
    public abstract class ABuilding : IBuilding
    {
        protected ABuilding(
            short ID,
            string Name,
            long WorldX,
            long WorldY,
            int TilesWide,
            int TilesHigh,
            AGrid StorageGrid,
            bool IsBackground,
            bool IsDestructible,
            EBuildingInteraction Interaction,
            string InteractionPrompt)
        {
            this.ID = ID;
            this.Name = Name;
            this.WorldX = WorldX;
            this.WorldY = WorldY;
            this.TilesWide = TilesWide;
            this.TilesHigh = TilesHigh;
            this.StorageGrid = StorageGrid;
            this.IsBackground = IsBackground;
            this.IsDestructible = IsDestructible;
            this.Interaction = Interaction;
            this.InteractionPrompt = InteractionPrompt;
        }

        public short ID { get; set; }
        public string Name { get; set; }
        public long WorldX { get; set; }
        public long WorldY { get; set; }
        public int TilesWide { get; set; }
        public int TilesHigh { get; set; }
        public AGrid StorageGrid { get; set; }
        public bool IsBackground { get; set; }
        public bool IsDestructible { get; set; }
        public EBuildingInteraction Interaction { get; set; }
        public string InteractionPrompt { get; set; }

        public bool ContainsTile(long worldX, long worldY)
        {
            return worldX >= WorldX &&
                   worldX < WorldX + TilesWide &&
                   worldY >= WorldY &&
                   worldY < WorldY + TilesHigh;
        }
    }
}
