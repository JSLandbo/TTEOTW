using System.Collections.Generic;
using System.Linq;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Blocks;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class ShopService(GameEventBus eventBus)
    {
        public SellSummary GetSellSummary(ModelWorld world)
        {
            return GetSellSummary(world, SellFilter.All);
        }

        public SellSummary GetOreSellSummary(ModelWorld world)
        {
            return GetSellSummary(world, SellFilter.BlocksOnly);
        }

        public double SellAll(ModelWorld world)
        {
            return Sell(world, SellFilter.All);
        }

        public double SellOres(ModelWorld world)
        {
            return Sell(world, SellFilter.BlocksOnly);
        }

        public double SellSlot(ModelWorld world, AGridBox slot)
        {
            if (slot?.Item == null || slot.Count <= 0 || !TryGetUnitSellValue(slot.Item, out double unitSellValue))
            {
                return 0.0;
            }

            double totalEarned = unitSellValue * slot.Count;
            slot.Item = null;
            slot.Count = 0;
            world.Player.Cash += totalEarned;
            eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Sold));

            return totalEarned;
        }

        private SellSummary GetSellSummary(ModelWorld world, SellFilter filter)
        {
            AInventory inventory = world.Player.Inventory;
            AGridBox[,] grid = inventory.Items.InternalGrid;
            Dictionary<short, SellableInventoryEntry> entries = [];
            double totalValue = 0.0;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (!CanSell(slot.Item, filter) || !TryGetUnitSellValue(slot.Item, out double unitSellValue) || slot.Count <= 0)
                    {
                        continue;
                    }

                    totalValue += unitSellValue * slot.Count;

                    if (!entries.TryGetValue(slot.Item.ID, out SellableInventoryEntry entry))
                    {
                        entry = new SellableInventoryEntry(slot.Item, 0, 0.0);
                    }

                    entries[slot.Item.ID] = new SellableInventoryEntry(
                        entry.Item,
                        entry.Count + slot.Count,
                        entry.TotalValue + (unitSellValue * slot.Count));
                }
            }

            return new SellSummary([.. entries.Values.OrderBy(entry => entry.Item.Name)], totalValue);
        }

        private double Sell(ModelWorld world, SellFilter filter)
        {
            AInventory inventory = world.Player.Inventory;
            AGridBox[,] grid = inventory.Items.InternalGrid;
            double totalEarned = 0.0;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (!TryGetSellValue(slot, filter, out double slotValue))
                    {
                        continue;
                    }

                    totalEarned += slotValue;
                    slot.Item = null;
                    slot.Count = 0;
                }
            }

            if (totalEarned > 0.0)
            {
                world.Player.Cash += totalEarned;
                eventBus.Publish(new ShopTransactionEvent(ShopTransactionType.Sold));
            }

            return totalEarned;
        }

        private bool TryGetSellValue(AGridBox slot, SellFilter filter, out double slotValue)
        {
            slotValue = 0.0;

            if (slot.Item == null || slot.Count <= 0 || !CanSell(slot.Item, filter) || !TryGetUnitSellValue(slot.Item, out double unitSellValue))
            {
                return false;
            }

            slotValue = unitSellValue * slot.Count;

            return true;
        }

        private static bool CanSell(AType item, SellFilter filter)
        {
            if (item == null)
            {
                return false;
            }

            return filter switch
            {
                SellFilter.All => true,
                SellFilter.BlocksOnly => item is Block,
                _ => false
            };
        }

        private static bool TryGetUnitSellValue(AType item, out double unitSellValue)
        {
            unitSellValue = 0.0;

            if (item == null || item.Worth <= 0)
            {
                return false;
            }

            if (item is Block block)
            {
                unitSellValue = block.Worth;

                return unitSellValue > 0;
            }

            unitSellValue = item.Worth * 0.5;

            return unitSellValue > 0;
        }

        private enum SellFilter
        {
            All,
            BlocksOnly
        }

        public sealed record SellableInventoryEntry(AType Item, int Count, double TotalValue);
        public sealed record SellSummary(IReadOnlyList<SellableInventoryEntry> Entries, double TotalValue);
    }
}
