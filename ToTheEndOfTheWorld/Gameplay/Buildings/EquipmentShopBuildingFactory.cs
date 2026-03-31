using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class EquipmentShopBuildingFactory(GameItemsRepository items)
    {
        private const int BuildingTilesWide = 4;
        private const int BuildingTilesHigh = 2;
        private const int GridColumns = 10;
        private const int GridRows = 7;

        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.EquipmentShop,
                Name: "Equipment Shop",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
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
            Grid storageGrid = new(Vector2.Zero, new GridBox[GridColumns, GridRows]);

            for (int y = 0; y < storageGrid.InternalGrid.GetLength(1); y++)
            {
                for (int x = 0; x < storageGrid.InternalGrid.GetLength(0); x++)
                {
                    storageGrid.InternalGrid[x, y] = new GridBox(null, 0);
                }
            }

            foreach (GameItemDefinition itemDefinition in items.Values)
            {
                if (!itemDefinition.Buyable || itemDefinition.Type != EGameItemType.Equipment || itemDefinition.EquipmentType == EEquipmentType.None)
                {
                    continue;
                }

                int row = (int)itemDefinition.EquipmentType - 1;
                int column = itemDefinition.Tier;

                if (row < 0 || row >= GridRows || column < 0 || column >= GridColumns)
                {
                    continue;
                }

                storageGrid.InternalGrid[column, row] = new GridBox(itemDefinition.Definition, 1);
            }

            return storageGrid;
        }

        private static bool IsUnstocked(AGrid grid)
        {
            for (int y = 0; y < grid.InternalGrid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.InternalGrid.GetLength(0); x++)
                {
                    AGridBox slot = grid.InternalGrid[x, y];

                    if (slot?.Item != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
