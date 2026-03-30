using Microsoft.Xna.Framework;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public readonly record struct InventoryLayout(
        Rectangle PanelRectangle,
        Rectangle HeaderRectangle,
        Rectangle CraftingSectionRectangle,
        Rectangle EquipmentSectionRectangle,
        Rectangle EquipmentInfoRectangle,
        Rectangle InventorySectionRectangle,
        Rectangle DividerRectangle,
        Point CraftingStart,
        Point InventoryStart,
        Rectangle TrashBinRectangle,
        Rectangle OutputSlotRectangle,
        Rectangle CraftButtonRectangle,
        Rectangle ThermalPlatingSlotRectangle,
        Rectangle HullSlotRectangle,
        Rectangle DrillSlotRectangle,
        Rectangle EngineSlotRectangle,
        Rectangle InventorySlotRectangle,
        Rectangle FuelTankSlotRectangle,
        Rectangle ThrusterSlotRectangle,
        int SlotSize,
        int SlotSpacing
    )
    {
        public Rectangle GetEquipmentSlotRectangle(PlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                PlayerEquipmentSlotType.ThermalPlating => ThermalPlatingSlotRectangle,
                PlayerEquipmentSlotType.Hull => HullSlotRectangle,
                PlayerEquipmentSlotType.Drill => DrillSlotRectangle,
                PlayerEquipmentSlotType.Engine => EngineSlotRectangle,
                PlayerEquipmentSlotType.Inventory => InventorySlotRectangle,
                PlayerEquipmentSlotType.FuelTank => FuelTankSlotRectangle,
                PlayerEquipmentSlotType.Thruster => ThrusterSlotRectangle,
                _ => HullSlotRectangle
            };
        }
    }
}
