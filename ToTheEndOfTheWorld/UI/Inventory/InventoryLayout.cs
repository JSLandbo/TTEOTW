using Microsoft.Xna.Framework;

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
        public Rectangle GetEquipmentSlotRectangle(Gameplay.PlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                Gameplay.PlayerEquipmentSlotType.ThermalPlating => ThermalPlatingSlotRectangle,
                Gameplay.PlayerEquipmentSlotType.Hull => HullSlotRectangle,
                Gameplay.PlayerEquipmentSlotType.Drill => DrillSlotRectangle,
                Gameplay.PlayerEquipmentSlotType.Engine => EngineSlotRectangle,
                Gameplay.PlayerEquipmentSlotType.Inventory => InventorySlotRectangle,
                Gameplay.PlayerEquipmentSlotType.FuelTank => FuelTankSlotRectangle,
                Gameplay.PlayerEquipmentSlotType.Thruster => ThrusterSlotRectangle,
                _ => HullSlotRectangle
            };
        }
    }
}
