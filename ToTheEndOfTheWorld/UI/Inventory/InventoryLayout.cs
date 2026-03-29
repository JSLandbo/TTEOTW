using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public readonly record struct InventoryLayout(
        Rectangle PanelRectangle,
        Point CraftingStart,
        Point InventoryStart,
        Rectangle OutputSlotRectangle,
        Vector2 OutputArrowPosition,
        Rectangle CraftButtonRectangle,
        int SlotSize,
        int SlotSpacing
    );
}
