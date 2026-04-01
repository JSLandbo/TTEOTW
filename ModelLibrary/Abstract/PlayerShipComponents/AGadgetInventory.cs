using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AGadgetInventory : AInventory
    {
        public abstract bool CanPlaceInSlot(int slotIndex, AType item);
    }
}
