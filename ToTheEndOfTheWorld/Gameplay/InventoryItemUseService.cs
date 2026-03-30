using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class InventoryItemUseService
    {
        private readonly InventoryService inventoryService;
        private readonly GameItemsRepository items;

        public InventoryItemUseService(InventoryService inventoryService, GameItemsRepository items)
        {
            this.inventoryService = inventoryService;
            this.items = items;
        }

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
                _ => null
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

            var createdItem = items.Create(heldItem.ID);

            if (createdItem == null)
            {
                return false;
            }

            var equippedItem = GetEquippedItem(world, slotType);
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
                _ => string.Empty
            };
        }

        public string GetSummaryText(ModelWorld world, EPlayerEquipmentSlotType slotType, AType equippedItem)
        {
            if (equippedItem == null)
            {
                return $"{GetSlotLabel(slotType)} | No upgrade equipped";
            }

            var tier = GetTierLabel(equippedItem);

            return slotType switch
            {
                EPlayerEquipmentSlotType.ThermalPlating => $"{GetSlotLabel(slotType)} | {tier} Tier | {((ThermalPlating)equippedItem).ThermalDissipation:0.#} Dissipation",
                EPlayerEquipmentSlotType.Engine => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Engine)equippedItem).ActiveFuelConsumption:0.#} Fuel/sec",
                EPlayerEquipmentSlotType.Inventory => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Inventory)equippedItem).SizeLimit:0} Capacity | x{((Inventory)equippedItem).MaxStackSize}",
                EPlayerEquipmentSlotType.FuelTank => $"{GetSlotLabel(slotType)} | {tier} Tier | {((FuelTank)equippedItem).Capacity:0} Capacity",
                EPlayerEquipmentSlotType.Hull => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Hull)equippedItem).Health:0} HP",
                EPlayerEquipmentSlotType.Drill => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Drill)equippedItem).Damage:0.##} Damage | {((Drill)equippedItem).MiningAreaSize}x{((Drill)equippedItem).MiningAreaSize}",
                EPlayerEquipmentSlotType.Thruster => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Thruster)equippedItem).Speed:0.#} Speed",
                _ => $"{GetSlotLabel(slotType)} | {tier} Tier"
            };
        }

        public string GetTierLabel(AType item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                return "Unknown";
            }

            var nameParts = item.Name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
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
            var usedCapacity = inventoryService.GetUsedCapacity(currentInventory);

            if (usedCapacity > heldInventory.SizeLimit)
            {
                return false;
            }

            var upgradedInventory = new Inventory(heldInventory);

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
            var createdInventory = items.Create<Inventory>(source.ID);

            if (createdInventory != null)
            {
                return createdInventory;
            }

            return new Inventory(
                ID: source.ID,
                Items: CreateEmptyGrid(source.Items),
                SizeLimit: source.SizeLimit,
                Name: source.Name,
                Worth: source.Worth,
                Weight: source.Weight,
                MaxStackSize: source.MaxStackSize);
        }

        private static Grid CreateEmptyGrid(AGrid source)
        {
            return new Grid(source.InternalCoordinate, new GridBox[source.InternalGrid.GetLength(0), source.InternalGrid.GetLength(1)]);
        }

        private bool TryMoveInventoryContents(Inventory sourceInventory, Inventory targetInventory)
        {
            var sourceGrid = sourceInventory.Items.InternalGrid;

            for (var y = 0; y < sourceGrid.GetLength(1); y++)
            {
                for (var x = 0; x < sourceGrid.GetLength(0); x++)
                {
                    var slot = sourceGrid[x, y];

                    if (slot?.Item == null || slot.Count <= 0)
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
