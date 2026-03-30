using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Blocks;
using System.Collections.Generic;
using System.Linq;

namespace ToTheEndOfTheWorld.Gameplay.Buildings
{
    public sealed class ShopService
    {
        public SellSummary GetSellSummary(ModelWorld world)
        {
            ModelLibrary.Abstract.PlayerShipComponents.AInventory inventory = world.Player.Inventory;
            AGridBox[,] grid = inventory.Items.InternalGrid;
            Dictionary<short, SellableInventoryEntry> entries = [];
            double totalValue = 0.0;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (!TryGetUnitSellValue(slot.Item, out double unitSellValue) || slot.Count <= 0)
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

            return new SellSummary(entries.Values.OrderBy(entry => entry.Item.Name).ToList(), totalValue);
        }

        public double SellAll(ModelWorld world)
        {
            ModelLibrary.Abstract.PlayerShipComponents.AInventory inventory = world.Player.Inventory;
            AGridBox[,] grid = inventory.Items.InternalGrid;
            double totalEarned = GetSellSummary(world).TotalValue;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (!TryGetSellValue(slot, out _))
                    {
                        continue;
                    }

                    slot.Item = null;
                    slot.Count = 0;
                }
            }

            world.Player.Cash += totalEarned;
            return totalEarned;
        }

        private bool TryGetSellValue(AGridBox slot, out double slotValue)
        {
            slotValue = 0.0;

            if (slot.Item == null || slot.Count <= 0 || !TryGetUnitSellValue(slot.Item, out double unitSellValue))
            {
                return false;
            }

            slotValue = unitSellValue * slot.Count;
            return true;
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

        public sealed record SellableInventoryEntry(AType Item, int Count, double TotalValue);
        public sealed record SellSummary(IReadOnlyList<SellableInventoryEntry> Entries, double TotalValue);
    }
}
