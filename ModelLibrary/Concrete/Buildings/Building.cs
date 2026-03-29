using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;

namespace ModelLibrary.Concrete.Buildings
{
    public sealed class Building : ABuilding
    {
        public Building(
            short ID,
            string Name,
            long WorldX,
            long WorldY,
            int TilesWide,
            int TilesHigh,
            AGrid StorageGrid,
            bool IsBackground = true,
            bool IsDestructible = false,
            EBuildingInteraction Interaction = EBuildingInteraction.None,
            string InteractionPrompt = null)
            : base(ID, Name, WorldX, WorldY, TilesWide, TilesHigh, StorageGrid, IsBackground, IsDestructible, Interaction, InteractionPrompt)
        {
        }
    }
}
