using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using System.Linq;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class EquipmentShopBuildingFactory
    {
        private const int BuildingTilesWide = 4;
        private const int BuildingTilesHigh = 2;
        private const int GridColumns = 10;
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
            var equipmentDefinitions = items.Values
                .Where(definition => definition.Buyable && definition.Type == GameItemType.Equipment && definition.EquipmentType != EquipmentType.None)
                .OrderBy(definition => definition.EquipmentType)
                .ThenBy(definition => definition.Tier)
                .ToList();
            var rows = GetRequiredRows(equipmentDefinitions);
            var storageGrid = new Grid(Vector2.Zero, new GridBox[GridColumns, rows]);

            for (var y = 0; y < storageGrid.InternalGrid.GetLength(1); y++)
            {
                for (var x = 0; x < storageGrid.InternalGrid.GetLength(0); x++)
                {
                    storageGrid.InternalGrid[x, y] = new GridBox(null, 0);
                }
            }

            foreach (var definition in equipmentDefinitions)
            {
                var column = definition.Tier;
                var row = (int)definition.EquipmentType - 1;

                if (column >= 0 && column < GridColumns && row >= 0 && row < rows)
                {
                    storageGrid.InternalGrid[column, row] = new GridBox(definition.Definition, 1);
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

        private static int GetRequiredRows(System.Collections.Generic.IReadOnlyCollection<GameItemDefinition> definitions)
        {
            var highestRow = definitions.Count == 0 ? 0 : definitions.Max(definition => (int)definition.EquipmentType - 1);
            return System.Math.Max(1, highestRow + 1);
        }
    }
}
