using ModelLibrary.Abstract.Grids;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiSlotInteractionHelper
    {
        public static bool CanInteractWithSlot(AGridBox slot, bool hasHeldItem)
        {
            return hasHeldItem || (slot?.Item != null && slot.Count > 0);
        }
    }
}
