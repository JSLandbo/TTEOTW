using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Concrete.Grids;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public class Inventory : AInventory
    {
        public Inventory(Inventory original)
        {
            ID = original.ID;
            Items = CloneGrid(original.Items);
            MaxStackSize = original.MaxStackSize;
            Inexhaustible = original.Inexhaustible;
            Name = original.Name;
            Worth = original.Worth;
            Weight = original.Weight;
        }

        [JsonConstructor]
        public Inventory(short ID, Grid Items, string Name, float Worth, float Weight, int MaxStackSize = 64, bool Inexhaustible = false)
        {
            this.ID = ID;
            this.Items = Items;
            this.MaxStackSize = MaxStackSize;
            this.Inexhaustible = Inexhaustible;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
        }

        private static Grid CloneGrid(AGrid original)
        {
            AGridBox[,] sourceGrid = original.InternalGrid;
            GridBox[,] clonedGrid = new GridBox[sourceGrid.GetLength(0), sourceGrid.GetLength(1)];

            for (int x = 0; x < sourceGrid.GetLength(0); x++)
            {
                for (int y = 0; y < sourceGrid.GetLength(1); y++)
                {
                    AGridBox slot = sourceGrid[x, y];
                    clonedGrid[x, y] = new GridBox(slot.Item, slot.Count);
                }
            }

            return new Grid(original.InternalCoordinate, clonedGrid);
        }
    }
}
