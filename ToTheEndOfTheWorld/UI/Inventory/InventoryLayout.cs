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
        Rectangle SelfDestructButtonRectangle,
        Point CraftingStart,
        Point InventoryStart,
        Rectangle SortButtonRectangle,
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
        public Rectangle GetEquipmentSlotRectangle(EPlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => ThermalPlatingSlotRectangle,
                EPlayerEquipmentSlotType.Hull => HullSlotRectangle,
                EPlayerEquipmentSlotType.Drill => DrillSlotRectangle,
                EPlayerEquipmentSlotType.Engine => EngineSlotRectangle,
                EPlayerEquipmentSlotType.Inventory => InventorySlotRectangle,
                EPlayerEquipmentSlotType.FuelTank => FuelTankSlotRectangle,
                EPlayerEquipmentSlotType.Thruster => ThrusterSlotRectangle,
                _ => HullSlotRectangle
            };
        }
    }
}
