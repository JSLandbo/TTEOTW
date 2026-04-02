using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AGadgetInventory : AInventory
    {
        public abstract bool CanPlaceInSlot(int slotIndex, AType item);

        public bool CanPlaceInSlot(AGridBox slot, AType item)
        {
            for (int x = 0; x < Items.InternalGrid.GetLength(0); x++)
            {
                if (ReferenceEquals(Items.InternalGrid[x, 0], slot))
                {
                    return CanPlaceInSlot(x, item);
                }
            }

            return false;
        }

        protected override void ConfigureGrid(AGrid grid)
        {
            base.ConfigureGrid(grid);
            grid.PlacementValidator = CanPlaceInSlot;
        }
    }
}
