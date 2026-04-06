using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class StorageChestBuildingFactory
    {
        public const int GridColumns = 10;
        public const int GridRows = 36;
        public const int MaxStackSize = 512;

        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.StorageChest,
                Name: "Storage Chest",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 2,
                TilesHigh: 2,
                StorageGrid: new Grid(Vector2.Zero, new GridBox[GridColumns, GridRows]),
                IsBackground: true,
                Interaction: EBuildingInteraction.StorageChest,
                ShowPlayerInventoryWhenOpen: true);
        }
    }
}
