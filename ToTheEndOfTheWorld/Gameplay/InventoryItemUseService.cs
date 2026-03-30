using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Context;

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

        public AType GetEquippedItem(World world, PlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                PlayerEquipmentSlotType.ThermalPlating => world.Player.ThermalPlating,
                PlayerEquipmentSlotType.Hull => world.Player.Hull,
                PlayerEquipmentSlotType.Drill => world.Player.Drill,
                PlayerEquipmentSlotType.Engine => world.Player.Engine,
                PlayerEquipmentSlotType.Inventory => world.Player.Inventory,
                PlayerEquipmentSlotType.FuelTank => world.Player.FuelTank,
                PlayerEquipmentSlotType.Thruster => world.Player.Thruster,
                _ => null
            };
        }

        public bool CanEquip(AType item, PlayerEquipmentSlotType slotType)
        {
            return item != null && MatchesSlot(item, slotType);
        }

        public bool TryEquipFromHeld(World world, PlayerEquipmentSlotType slotType, ref AType heldItem, ref int heldCount)
        {
            if (heldItem == null || heldCount <= 0 || !MatchesSlot(heldItem, slotType))
            {
                return false;
            }

            if (slotType == PlayerEquipmentSlotType.Inventory)
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

        public string GetSlotLabel(PlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                PlayerEquipmentSlotType.ThermalPlating => "Plating",
                PlayerEquipmentSlotType.Hull => "Hull",
                PlayerEquipmentSlotType.Drill => "Drill",
                PlayerEquipmentSlotType.Engine => "Engine",
                PlayerEquipmentSlotType.Inventory => "Inventory",
                PlayerEquipmentSlotType.FuelTank => "Fuel Tank",
                PlayerEquipmentSlotType.Thruster => "Thruster",
                _ => string.Empty
            };
        }

        public string GetSummaryText(World world, PlayerEquipmentSlotType slotType, AType equippedItem)
        {
            if (equippedItem == null)
            {
                return $"{GetSlotLabel(slotType)} | No upgrade equipped";
            }

            var tier = GetTierLabel(equippedItem);

            return slotType switch
            {
                PlayerEquipmentSlotType.ThermalPlating => $"{GetSlotLabel(slotType)} | {tier} Tier | {((ThermalPlating)equippedItem).ThermalDissipation:0.#} Dissipation",
                PlayerEquipmentSlotType.Engine => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Engine)equippedItem).ActiveFuelConsumption:0.#} Fuel/sec",
                PlayerEquipmentSlotType.Inventory => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Inventory)equippedItem).SizeLimit:0} Capacity | x{((Inventory)equippedItem).MaxStackSize}",
                PlayerEquipmentSlotType.FuelTank => $"{GetSlotLabel(slotType)} | {tier} Tier | {((FuelTank)equippedItem).Capacity:0} Capacity",
                PlayerEquipmentSlotType.Hull => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Hull)equippedItem).Health:0} HP",
                PlayerEquipmentSlotType.Drill => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Drill)equippedItem).Damage:0.##} Damage | {((Drill)equippedItem).MiningAreaSize}x{((Drill)equippedItem).MiningAreaSize}",
                PlayerEquipmentSlotType.Thruster => $"{GetSlotLabel(slotType)} | {tier} Tier | {((Thruster)equippedItem).Speed:0.#} Speed",
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

        private static bool MatchesSlot(AType item, PlayerEquipmentSlotType slotType)
        {
            return slotType switch
            {
                PlayerEquipmentSlotType.ThermalPlating => item is ThermalPlating,
                PlayerEquipmentSlotType.Hull => item is Hull,
                PlayerEquipmentSlotType.Drill => item is Drill,
                PlayerEquipmentSlotType.Engine => item is Engine,
                PlayerEquipmentSlotType.Inventory => item is Inventory,
                PlayerEquipmentSlotType.FuelTank => item is FuelTank,
                PlayerEquipmentSlotType.Thruster => item is Thruster,
                _ => false
            };
        }

        private static void ApplyEquippedItem(World world, PlayerEquipmentSlotType slotType, AType item)
        {
            switch (slotType)
            {
                case PlayerEquipmentSlotType.ThermalPlating:
                    world.Player.ThermalPlating = (ThermalPlating)item;
                    break;
                case PlayerEquipmentSlotType.Hull:
                    world.Player.Hull = (Hull)item;
                    break;
                case PlayerEquipmentSlotType.Drill:
                    world.Player.Drill = (Drill)item;
                    break;
                case PlayerEquipmentSlotType.Engine:
                    world.Player.Engine = (Engine)item;
                    break;
                case PlayerEquipmentSlotType.Inventory:
                    world.Player.Inventory = (Inventory)item;
                    break;
                case PlayerEquipmentSlotType.FuelTank:
                    world.Player.FuelTank = (FuelTank)item;
                    break;
                case PlayerEquipmentSlotType.Thruster:
                    world.Player.Thruster = (Thruster)item;
                    break;
            }
        }

        private bool TryEquipInventoryFromHeld(World world, ref AType heldItem, ref int heldCount)
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
