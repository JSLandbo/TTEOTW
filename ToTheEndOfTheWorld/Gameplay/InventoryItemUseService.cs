using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class InventoryItemUseService(InventoryService inventoryService, GameItemsRepository items)
    {
        public AType GetEquippedItem(ModelWorld world, EPlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => world.Player.ThermalPlating,
                EPlayerEquipmentSlotType.Hull => world.Player.Hull,
                EPlayerEquipmentSlotType.Drill => world.Player.Drill,
                EPlayerEquipmentSlotType.Engine => world.Player.Engine,
                EPlayerEquipmentSlotType.Inventory => world.Player.Inventory,
                EPlayerEquipmentSlotType.FuelTank => world.Player.FuelTank,
                EPlayerEquipmentSlotType.Thruster => world.Player.Thruster,
                _ => throw new System.ArgumentOutOfRangeException(nameof(slotType), slotType, null)
            };
        }

        public bool CanEquip(AType item, EPlayerEquipmentSlotType slotType)
        {
            return item != null && MatchesSlot(item, slotType);
        }

        public bool TryEquipFromHeld(ModelWorld world, EPlayerEquipmentSlotType slotType, ref AType heldItem, ref int heldCount)
        {
            if (heldItem == null || heldCount <= 0 || !MatchesSlot(heldItem, slotType))
            {
                return false;
            }

            if (slotType == EPlayerEquipmentSlotType.Inventory)
            {
                return TryEquipInventoryFromHeld(world, ref heldItem, ref heldCount);
            }

            AType createdItem = items.Create(heldItem.ID);

            AType equippedItem = GetEquippedItem(world, slotType);

            if (equippedItem != null && !inventoryService.TryAdd(world.Player.Inventory, equippedItem, 1))
            {
                return false;
            }

            ApplyEquippedItem(world, slotType, createdItem);

            heldCount--;

            if (heldCount <= 0)
            {
                heldItem = null;
                heldCount = 0;
            }

            return true;
        }

        public string GetSlotLabel(EPlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => "Plating",
                EPlayerEquipmentSlotType.Hull => "Hull",
                EPlayerEquipmentSlotType.Drill => "Drill",
                EPlayerEquipmentSlotType.Engine => "Engine",
                EPlayerEquipmentSlotType.Inventory => "Inventory",
                EPlayerEquipmentSlotType.FuelTank => "Fuel Tank",
                EPlayerEquipmentSlotType.Thruster => "Thruster",
                _ => throw new System.ArgumentOutOfRangeException(nameof(slotType), slotType, null)
            };
        }

        public string GetSummaryText(EPlayerEquipmentSlotType slotType, AType equippedItem)
        {
            if (equippedItem == null)
            {
                return $"{GetSlotLabel(slotType)} | Empty";
            }

            string tier = GetTierLabel(equippedItem);

            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => $"{GetSlotLabel(slotType)} | {tier} | Heat {((ThermalPlating)equippedItem).Thermals:0}/{((ThermalPlating)equippedItem).MaxThermals:0} | Dissipation {((ThermalPlating)equippedItem).ThermalDissipation:0.#}/s",
                EPlayerEquipmentSlotType.Engine => $"{GetSlotLabel(slotType)} | {tier} | Speed {((Engine)equippedItem).Speed:0.#} | Acceleration {((Engine)equippedItem).Acceleration:0.#} | Active Fuel {((Engine)equippedItem).ActiveFuelConsumption:0.##}/s",
                EPlayerEquipmentSlotType.Inventory => $"{GetSlotLabel(slotType)} | {tier} | Used Capacity {inventoryService.GetUsedCapacity((Inventory)equippedItem):0}/{((Inventory)equippedItem).SizeLimit:0} ({inventoryService.GetUsedCapacityPercent((Inventory)equippedItem)}%) | Max Stack {((Inventory)equippedItem).MaxStackSize}",
                EPlayerEquipmentSlotType.FuelTank => $"{GetSlotLabel(slotType)} | {tier} | Fuel {((FuelTank)equippedItem).Fuel:0.##}/{((FuelTank)equippedItem).Capacity:0.##} ({((((FuelTank)equippedItem).Capacity <= 0 ? 0 : (((FuelTank)equippedItem).Fuel / ((FuelTank)equippedItem).Capacity) * 100f)):0.#}%)",
                EPlayerEquipmentSlotType.Hull => $"{GetSlotLabel(slotType)} | {tier} | Health {((Hull)equippedItem).Health:0} | Durability {((Hull)equippedItem).Durability:0.#}",
                EPlayerEquipmentSlotType.Drill => $"{GetSlotLabel(slotType)} | {tier} | Damage {((Drill)equippedItem).Damage:0.##} | Area {((Drill)equippedItem).MiningAreaSize}x{((Drill)equippedItem).MiningAreaSize} | Hardness {((Drill)equippedItem).Hardness:0.#} | Fuel Usage {((Drill)equippedItem).ActiveFuelConsumption:0.##}/s",
                EPlayerEquipmentSlotType.Thruster => $"{GetSlotLabel(slotType)} | {tier} | Speed {((Thruster)equippedItem).Speed:0.#} | Acceleration {((Thruster)equippedItem).Acceleration:0.#} | Power {((Thruster)equippedItem).Power:0.#} | Heat {((Thruster)equippedItem).ActiveHeatGeneration:0.#}/s",
                _ => $"{GetSlotLabel(slotType)} | {tier}"
            };
        }

        public string GetTierLabel(AType item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                return "Unknown";
            }

            string[] nameParts = item.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            return nameParts.Length == 0 ? "Unknown" : nameParts[0];
        }

        private static bool MatchesSlot(AType item, EPlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => item is ThermalPlating,
                EPlayerEquipmentSlotType.Hull => item is Hull,
                EPlayerEquipmentSlotType.Drill => item is Drill,
                EPlayerEquipmentSlotType.Engine => item is Engine,
                EPlayerEquipmentSlotType.Inventory => item is Inventory,
                EPlayerEquipmentSlotType.FuelTank => item is FuelTank,
                EPlayerEquipmentSlotType.Thruster => item is Thruster,
                _ => false
            };
        }

        private static void ApplyEquippedItem(ModelWorld world, EPlayerEquipmentSlotType slotType, AType item)
        {
            switch (slotType)
            {
                case EPlayerEquipmentSlotType.ThermalPlating:
                    world.Player.ThermalPlating = (ThermalPlating)item;
                    break;
                case EPlayerEquipmentSlotType.Hull:
                    world.Player.Hull = (Hull)item;
                    break;
                case EPlayerEquipmentSlotType.Drill:
                    world.Player.Drill = (Drill)item;
                    break;
                case EPlayerEquipmentSlotType.Engine:
                    world.Player.Engine = (Engine)item;
                    break;
                case EPlayerEquipmentSlotType.Inventory:
                    world.Player.Inventory = (Inventory)item;
                    break;
                case EPlayerEquipmentSlotType.FuelTank:
                    world.Player.FuelTank = (FuelTank)item;
                    break;
                case EPlayerEquipmentSlotType.Thruster:
                    world.Player.Thruster = (Thruster)item;
                    break;
            }
        }

        private bool TryEquipInventoryFromHeld(ModelWorld world, ref AType heldItem, ref int heldCount)
        {
            if (heldItem is not Inventory heldInventory || heldCount <= 0)
            {
                return false;
            }

            if (world.Player.Inventory is not Inventory currentInventory)
            {
                return false;
            }

            int usedCapacity = inventoryService.GetUsedCapacity(currentInventory);

            if (usedCapacity > heldInventory.SizeLimit)
            {
                return false;
            }

            Inventory upgradedInventory = new(heldInventory);

            if (!TryMoveInventoryContents(currentInventory, upgradedInventory))
            {
                return false;
            }

            world.Player.Inventory = upgradedInventory;

            heldItem = CreateEmptyInventoryItem(currentInventory);

            heldCount = 1;

            return true;
        }

        private Inventory CreateEmptyInventoryItem(Inventory source)
        {
            Inventory createdInventory = items.Create<Inventory>(source.ID);
            createdInventory.Items = CreateEmptyGrid(source.Items);
            return createdInventory;
        }

        private static Grid CreateEmptyGrid(AGrid source)
        {
            return new Grid(source.InternalCoordinate, new GridBox[source.InternalGrid.GetLength(0), source.InternalGrid.GetLength(1)]);
        }

        private bool TryMoveInventoryContents(Inventory sourceInventory, Inventory targetInventory)
        {
            AGridBox[,] sourceGrid = sourceInventory.Items.InternalGrid;

            for (int y = 0; y < sourceGrid.GetLength(1); y++)
            {
                for (int x = 0; x < sourceGrid.GetLength(0); x++)
                {
                    AGridBox slot = sourceGrid[x, y];

                    if (slot.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    if (!inventoryService.TryAdd(targetInventory, slot.Item, slot.Count))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
