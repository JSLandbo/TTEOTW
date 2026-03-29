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

            var totalWeight = 0.0f;
            var grid = Items.InternalGrid;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot?.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    var itemWeight = slot.Item is ABlock block ? block.Info?.Weight ?? 0.0f : slot.Item.Weight;
                    totalWeight += itemWeight * slot.Count;
                }
            }

            ContentsWeight = totalWeight;
        }
    }
}
