using System;
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
                _ => throw new ArgumentOutOfRangeException(nameof(slotType), slotType, null)
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

            if (!TryEquipStandardItem(world, slotType, heldItem))
            {
                return false;
            }

            heldCount--;

            if (heldCount <= 0)
            {
                heldItem = null;
                heldCount = 0;
            }

            return true;
        }

        public bool TryEquip(ModelWorld world, AType item)
        {
            if (item == null || !TryGetEquipmentSlotType(item, out EPlayerEquipmentSlotType slotType))
            {
                return false;
            }

            return slotType == EPlayerEquipmentSlotType.Inventory
                ? TryEquipInventory(world, (Inventory)item)
                : TryEquipStandardItem(world, slotType, item);
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
                EPlayerEquipmentSlotType.ThermalPlating => $"{GetSlotLabel(slotType)} | {tier} | Heat Capacity {((ThermalPlating)equippedItem).MaxThermals:0} | Dissipation {((ThermalPlating)equippedItem).ThermalDissipation:0.#}/s",
                EPlayerEquipmentSlotType.Engine => $"{GetSlotLabel(slotType)} | {tier} | Speed {((Engine)equippedItem).Speed:0.#} | Acceleration {((Engine)equippedItem).Acceleration:0.#} | Active Fuel {((Engine)equippedItem).ActiveFuelConsumption:0.##}/s",
                EPlayerEquipmentSlotType.Inventory => $"{GetSlotLabel(slotType)} | {tier} | Used Capacity {inventoryService.GetUsedCapacity((Inventory)equippedItem):0}/{((Inventory)equippedItem).SizeLimit:0} ({inventoryService.GetUsedCapacityPercent((Inventory)equippedItem)}%) | Max Stack {((Inventory)equippedItem).MaxStackSize}",
                EPlayerEquipmentSlotType.FuelTank => $"{GetSlotLabel(slotType)} | {tier} | Fuel Capacity {((FuelTank)equippedItem).Capacity:0.##}",
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

        public bool TryGetEquipmentSlotType(AType item, out EPlayerEquipmentSlotType slotType)
        {
            switch (item)
            {
                case ThermalPlating:
                    slotType = EPlayerEquipmentSlotType.ThermalPlating;
                    return true;
                case Hull:
                    slotType = EPlayerEquipmentSlotType.Hull;
                    return true;
                case Drill:
                    slotType = EPlayerEquipmentSlotType.Drill;
                    return true;
                case Engine:
                    slotType = EPlayerEquipmentSlotType.Engine;
                    return true;
                case Inventory:
                    slotType = EPlayerEquipmentSlotType.Inventory;
                    return true;
                case FuelTank:
                    slotType = EPlayerEquipmentSlotType.FuelTank;
                    return true;
                case Thruster:
                    slotType = EPlayerEquipmentSlotType.Thruster;
                    return true;
                default:
                    slotType = default;
                    return false;
            }
        }

        public bool TryAlignGadgetSlotsWithInventory(ModelWorld world)
        {
            if (world.Player.Inventory is not Inventory currentInventory
                || world.Player.GadgetSlots is not GadgetInventory currentGadgetSlots)
            {
                return false;
            }

            if (!TryCreateAdjustedGadgetSlots(currentGadgetSlots, currentInventory, out GadgetInventory adjustedGadgetSlots))
            {
                return false;
            }

            world.Player.GadgetSlots = adjustedGadgetSlots;
            return true;
        }

        private bool MatchesSlot(AType item, EPlayerEquipmentSlotType slotType)
        {
            return TryGetEquipmentSlotType(item, out EPlayerEquipmentSlotType itemSlotType) && itemSlotType == slotType;
        }

        private static void ApplyEquippedItem(ModelWorld world, EPlayerEquipmentSlotType slotType, AType item)
        {
            switch (slotType)
            {
                case EPlayerEquipmentSlotType.ThermalPlating:
                    world.Player.ThermalPlating = (ThermalPlating)item;
                    world.Player.CurrentHeat = Math.Clamp(world.Player.CurrentHeat, 0.0f, world.Player.ThermalPlating.MaxThermals);
                    break;
                case EPlayerEquipmentSlotType.Hull:
                    world.Player.Hull = (Hull)item;
                    world.Player.CurrentHull = Math.Clamp(world.Player.CurrentHull, 0.0f, world.Player.Hull.Health);
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
                    world.Player.CurrentFuel = Math.Clamp(world.Player.CurrentFuel, 0.0f, world.Player.FuelTank.Capacity);
                    break;
                case EPlayerEquipmentSlotType.Thruster:
                    world.Player.Thruster = (Thruster)item;
                    break;
            }
        }

        private bool TryEquipStandardItem(ModelWorld world, EPlayerEquipmentSlotType slotType, AType item)
        {
            AType equippedItem = GetEquippedItem(world, slotType);

            if (equippedItem != null && !inventoryService.TryAdd(world.Player.Inventory, equippedItem, 1))
            {
                return false;
            }

            ApplyEquippedItem(world, slotType, item);

            return true;
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

            if (!TryCreateEquippedInventory(currentInventory, heldInventory, out Inventory upgradedInventory))
            {
                return false;
            }

            if (world.Player.GadgetSlots is not GadgetInventory currentGadgetSlots
                || !TryCreateAdjustedGadgetSlots(currentGadgetSlots, upgradedInventory, out GadgetInventory adjustedGadgetSlots))
            {
                return false;
            }

            world.Player.Inventory = upgradedInventory;
            world.Player.GadgetSlots = adjustedGadgetSlots;

            heldItem = CreateEmptyInventoryItem(currentInventory);

            heldCount = 1;

            return true;
        }

        private bool TryEquipInventory(ModelWorld world, Inventory inventoryItem)
        {
            if (world.Player.Inventory is not Inventory currentInventory)
            {
                return false;
            }

            if (!TryCreateEquippedInventory(currentInventory, inventoryItem, out Inventory upgradedInventory))
            {
                return false;
            }

            if (!inventoryService.TryAdd(upgradedInventory, CreateEmptyInventoryItem(currentInventory), 1))
            {
                return false;
            }

            if (world.Player.GadgetSlots is not GadgetInventory currentGadgetSlots
                || !TryCreateAdjustedGadgetSlots(currentGadgetSlots, upgradedInventory, out GadgetInventory adjustedGadgetSlots))
            {
                return false;
            }

            world.Player.Inventory = upgradedInventory;
            world.Player.GadgetSlots = adjustedGadgetSlots;

            return true;
        }

        private bool TryCreateEquippedInventory(Inventory currentInventory, Inventory inventoryItem, out Inventory upgradedInventory)
        {
            upgradedInventory = null;
            int usedCapacity = inventoryService.GetUsedCapacity(currentInventory);

            if (usedCapacity > inventoryItem.SizeLimit)
            {
                return false;
            }

            upgradedInventory = new Inventory(inventoryItem);

            if (!TryMoveInventoryContents(currentInventory, upgradedInventory))
            {
                upgradedInventory = null;
                return false;
            }

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

        private bool TryCreateAdjustedGadgetSlots(GadgetInventory sourceGadgetSlots, Inventory targetInventory, out GadgetInventory adjustedGadgetSlots)
        {
            int targetMaxStackSize = inventoryService.GetMaxStackSize(targetInventory);
            adjustedGadgetSlots = new GadgetInventory(sourceGadgetSlots)
            {
                MaxStackSize = targetMaxStackSize
            };

            AGridBox[,] gadgetGrid = adjustedGadgetSlots.Items.InternalGrid;

            for (int x = 0; x < gadgetGrid.GetLength(0); x++)
            {
                AGridBox slot = gadgetGrid[x, 0];

                if (slot.Item == null || slot.Count <= targetMaxStackSize)
                {
                    continue;
                }

                int overflowCount = slot.Count - targetMaxStackSize;
                slot.Count = targetMaxStackSize;

                if (!inventoryService.TryAdd(targetInventory, slot.Item, overflowCount))
                {
                    adjustedGadgetSlots = null!;
                    return false;
                }
            }

            return true;
        }
    }
}
