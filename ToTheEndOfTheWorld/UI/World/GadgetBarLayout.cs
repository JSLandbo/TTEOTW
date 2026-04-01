using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.World
{
    public static class GadgetBarLayout
    {
        public const int HotbarSlotCount = 4;
        public const int TotalSlotCount = 6;
        public const int SlotSize = 64;
        public const int SlotSpacing = 8;
        public const int SpecialSlotGap = 18;
        public const int BottomMargin = 18;

        public static Rectangle GetSlotRectangle(int viewportWidth, int viewportHeight, int slotIndex)
        {
            int totalWidth = (HotbarSlotCount * SlotSize) + ((HotbarSlotCount - 1) * SlotSpacing) + SpecialSlotGap + (2 * SlotSize) + SlotSpacing;
            int startX = (viewportWidth - totalWidth) / 2;
            int y = viewportHeight - BottomMargin - SlotSize;

            if (slotIndex < HotbarSlotCount)
            {
                return new Rectangle(startX + (slotIndex * (SlotSize + SlotSpacing)), y, SlotSize, SlotSize);
            }

            int specialStartX = startX + (HotbarSlotCount * (SlotSize + SlotSpacing)) - SlotSpacing + SpecialSlotGap;
            int specialIndex = slotIndex - HotbarSlotCount;
            return new Rectangle(specialStartX + (specialIndex * (SlotSize + SlotSpacing)), y, SlotSize, SlotSize);
        }
    }
}
