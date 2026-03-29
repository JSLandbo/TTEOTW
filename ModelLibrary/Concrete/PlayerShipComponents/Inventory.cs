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
            SizeLimit = original.SizeLimit;
            MaxStackSize = original.MaxStackSize;
            Name = original.Name;
            Worth = original.Worth;
            Weight = original.Weight;
        }

        [JsonConstructor]
        public Inventory(short ID, Grid Items, float SizeLimit, string Name, float Worth, float Weight, int MaxStackSize = 64)
        {
            this.ID = ID;
            this.Items = Items;
            this.SizeLimit = SizeLimit;
            this.MaxStackSize = MaxStackSize;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
        }

        private static Grid CloneGrid(ModelLibrary.Abstract.Grids.AGrid original)
        {
            var sourceGrid = original.InternalGrid;
            var clonedGrid = new GridBox[sourceGrid.GetLength(0), sourceGrid.GetLength(1)];

            for (var x = 0; x < sourceGrid.GetLength(0); x++)
            {
                for (var y = 0; y < sourceGrid.GetLength(1); y++)
                {
                    var slot = sourceGrid[x, y];
                    clonedGrid[x, y] = new GridBox(slot?.Item, slot?.Count ?? 0);
                }
            }

            return new Grid(original.InternalCoordinate, clonedGrid);
        }
    }
}
