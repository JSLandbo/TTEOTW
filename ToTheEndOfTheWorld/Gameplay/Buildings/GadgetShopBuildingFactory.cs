using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class GadgetShopBuildingFactory(GameItemsRepository items)
    {
        private const int GridColumns = 6;
        private const int GridRows = 6;

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
                Interaction: EBuildingInteraction.GadgetShop);
        }

        private Grid CreateShopGrid()
        {
            Grid grid = new(Vector2.Zero, new GridBox[GridColumns, GridRows]);

            grid.InternalGrid[0, 2] = new GridBox(items.Create(GameIds.Items.Gadgets.DirtFilter), 1);
            grid.InternalGrid[1, 2] = new GridBox(items.Create(GameIds.Items.Gadgets.RockFilter), 1);

            grid.InternalGrid[0, 1] = new GridBox(items.Create(GameIds.Items.Consumeables.SmallDynamite), 1);

            grid.InternalGrid[0, 0] = new GridBox(items.Create(GameIds.Items.Consumeables.SmallFuelCapsule), 1);
            grid.InternalGrid[1, 0] = new GridBox(items.Create(GameIds.Items.Consumeables.MediumFuelCapsule), 1);
            grid.InternalGrid[2, 0] = new GridBox(items.Create(GameIds.Items.Consumeables.LargeFuelCapsule), 1);
            grid.InternalGrid[3, 0] = new GridBox(items.Create(GameIds.Items.Consumeables.SmallCoolantPatch), 1);
            grid.InternalGrid[4, 0] = new GridBox(items.Create(GameIds.Items.Consumeables.SmallHullRepairKit), 1);

            return grid;
        }
    }
}
