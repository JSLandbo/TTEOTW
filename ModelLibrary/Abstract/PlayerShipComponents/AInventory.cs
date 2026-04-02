using ModelLibrary.Abstract.Blocks;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AInventory : AType
    {
        private AGrid items = null!;

        public float SizeLimit { get; set; }
        public int MaxStackSize { get; set; }
        public AGrid Items
        {
            get => items;
            set
            {
                items = value;
                if (items != null)
                {
                    items.OnChanged = RecalculateContentsWeight;
                    ConfigureGrid(items);
                }
                RecalculateContentsWeight();
            }
        }

        public float ContentsWeight { get; private set; }

        public void RecalculateContentsWeight()
        {
            if (Items?.InternalGrid == null)
            {
                ContentsWeight = 0.0f;

                return;
            }

            float totalWeight = 0.0f;
            AGridBox[,] grid = Items.InternalGrid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (slot.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    float itemWeight = slot.Item is ABlock block
                        ? block.Info?.Weight ?? 0.0f
                        : slot.Item.Weight;

                    totalWeight += itemWeight * slot.Count;
                }
            }

            ContentsWeight = totalWeight;
        }

        protected virtual void ConfigureGrid(AGrid grid)
        {
            grid.PlacementValidator = null;
        }
    }
}
