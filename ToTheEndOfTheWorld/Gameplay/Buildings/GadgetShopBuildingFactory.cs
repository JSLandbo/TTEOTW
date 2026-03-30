using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class GadgetShopBuildingFactory
    {
        private const int GridColumns = 6;
        private const int GridRows = 6;
        private readonly GameItemsRepository items;

        public GadgetShopBuildingFactory(GameItemsRepository items)
        {
            this.items = items;
        }

        public ABuilding Create(long worldX, long worldY)
        {
            return new Building(
                ID: GameIds.Buildings.GadgetShop,
                Name: "Gadget Shop",
                WorldX: worldX,
                WorldY: worldY,
                XOffset: 0,
                YOffset: 8,
                TilesWide: 3,
                TilesHigh: 3,
                StorageGrid: CreateShopGrid(),
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.GadgetShop,
                InteractionPrompt: "Press E to open shop");
        }

        private Grid CreateShopGrid()
        {
            Grid grid = new Grid(Vector2.Zero, new GridBox[GridColumns, GridRows]);
            grid.InternalGrid[0, 0] = new GridBox(items.Create(GameIds.Items.Gadgets.DirtFilter), 1);
            grid.InternalGrid[1, 0] = new GridBox(items.Create(GameIds.Items.Gadgets.RockFilter), 1);
            return grid;
        }
    }
}
