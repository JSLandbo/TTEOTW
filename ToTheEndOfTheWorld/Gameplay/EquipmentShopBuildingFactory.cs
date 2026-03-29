using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class EquipmentShopBuildingFactory
    {
        private const int BuildingTilesWide = 3;
        private const int BuildingTilesHigh = 2;
        private const int GridColumns = 7;
        private readonly GameItemsRepository items;

        public EquipmentShopBuildingFactory(GameItemsRepository items)
        {
            this.items = items;
        }

        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: 2,
                Name: "Equipment Shop",
                WorldX: worldX,
                WorldY: worldY,
                TilesWide: BuildingTilesWide,
                TilesHigh: BuildingTilesHigh,
                StorageGrid: CreateShopGrid(),
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.EquipmentShop,
                InteractionPrompt: "Press E to open equipment shop");
        }

        public void EnsureStorage(ABuilding building)
        {
            if (building.StorageGrid == null || IsUnstocked(building.StorageGrid))
            {
                building.StorageGrid = CreateShopGrid();
            }
        }

        private Grid CreateShopGrid()
        {
            var orderedDefinitions = items.GetEquipmentShopDefinitions();
            var rows = GetRequiredRows(orderedDefinitions.Count);
            var storageGrid = new Grid(Vector2.Zero, new GridBox[GridColumns, rows]);
            var index = 0;

            for (var y = 0; y < storageGrid.InternalGrid.GetLength(1); y++)
            {
                for (var x = 0; x < storageGrid.InternalGrid.GetLength(0); x++)
                {
                    if (index >= orderedDefinitions.Count)
                    {
                        storageGrid.InternalGrid[x, y] = new GridBox(null, 0);
                        continue;
                    }

                    storageGrid.InternalGrid[x, y] = new GridBox(orderedDefinitions[index].Definition, 1);
                    index++;
                }
            }

            return storageGrid;
        }

        private static bool IsUnstocked(AGrid grid)
        {
            for (var y = 0; y < grid.InternalGrid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.InternalGrid.GetLength(0); x++)
                {
                    var slot = grid.InternalGrid[x, y];
                    if (slot?.Item != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int GetRequiredRows(int itemCount)
        {
            return System.Math.Max(1, (itemCount + GridColumns - 1) / GridColumns);
        }
    }
}
