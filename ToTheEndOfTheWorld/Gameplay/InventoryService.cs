using System;
using System.Collections.Generic;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class InventoryService
    {
        public const int DefaultMaxStackSize = 64;

        public static bool CanStackTogether(AType left, AType right)
        {
            if (left == null || right == null)
            {
                return false;
            }

            if (left is AInventory || right is AInventory)
            {
                return false;
            }

            if (!left.Stackable || !right.Stackable)
            {
                return false;
            }

            return left.GetType() == right.GetType() && left.ID == right.ID;
        }

        public bool TryAdd(AInventory inventory, AType item, int count)
        {
            if (count <= 0)
            {
                return true;
            }

            if (!CanAccept(inventory, count))
            {
                return false;
            }

            AGridBox[,] grid = inventory.Items.InternalGrid;
            int maxStackSize = GetMaxStackSize(inventory);
            int remainingCount = TryAddToMatchingStacks(grid, item, count, maxStackSize);

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (slot.Item != null) continue;

                    int amountToAdd = remainingCount > maxStackSize ? maxStackSize : remainingCount;
                    slot.Item = item;
                    slot.Count = amountToAdd;
                    remainingCount -= amountToAdd;

                    if (remainingCount == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryAddToMatchingStacks(AInventory inventory, AType item, int count)
        {
            if (count <= 0)
            {
                return true;
            }

            if (!CanAccept(inventory, count))
            {
                return false;
            }

            return TryAddToMatchingStacks(inventory.Items.InternalGrid, item, count, GetMaxStackSize(inventory)) == 0;
        }

        public bool CanAccept(AInventory inventory, int count)
        {
            if (count <= 0)
            {
                return true;
            }

            return GetRemainingCapacity(inventory) >= count;
        }

        public int GetMaxStackSize(AInventory inventory)
        {
            if (inventory == null || inventory.MaxStackSize <= 0)
            {
                return DefaultMaxStackSize;
            }

            return inventory.MaxStackSize;
        }

        public int GetUsedCapacity(AInventory inventory)
        {
            int usedCapacity = 0;
            AGridBox[,] grid = inventory.Items.InternalGrid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    usedCapacity += grid[x, y].Count;
                }
            }

            return usedCapacity;
        }

        public int GetUsedCapacityPercent(AInventory inventory)
        {
            if (inventory.SizeLimit <= 0)
            {
                return 0;
            }

            int usedCapacity = GetUsedCapacity(inventory);
            float percent = (usedCapacity / inventory.SizeLimit) * 100.0f;

            return (int)percent;
        }

        public void SortByName(AInventory inventory)
        {
            AGridBox[,] grid = inventory.Items.InternalGrid;
            int maxStackSize = GetMaxStackSize(inventory);
            List<(AType Item, int Count)> entries = [];

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (slot.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    entries.Add((slot.Item, slot.Count));
                }
            }

            entries.Sort((left, right) =>
            {
                return string.Compare(left.Item.Name, right.Item.Name, StringComparison.OrdinalIgnoreCase);
            });

            List<(AType Item, int Count)> groupedEntries = [];

            foreach ((AType item, int count) in entries)
            {
                if (groupedEntries.Count > 0 && CanStackTogether(groupedEntries[^1].Item, item))
                {
                    (AType groupedItem, int groupedCount) = groupedEntries[^1];
                    groupedEntries[^1] = (groupedItem, groupedCount + count);
                    continue;
                }

                groupedEntries.Add((item, count));
            }

            int entryIndex = 0;
            int remainingCount = groupedEntries.Count > 0 ? groupedEntries[0].Count : 0;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (entryIndex < groupedEntries.Count)
                    {
                        (AType item, int count) = groupedEntries[entryIndex];
                        int slotCount = item.Stackable ? Math.Min(remainingCount, maxStackSize) : 1;
                        slot.Item = item;
                        slot.Count = slotCount;
                        remainingCount -= slotCount;

                        if (remainingCount <= 0)
                        {
                            entryIndex++;
                            if (entryIndex < groupedEntries.Count)
                            {
                                remainingCount = groupedEntries[entryIndex].Count;
                            }
                        }

                        continue;
                    }

                    slot.Item = null;
                    slot.Count = 0;
                }
            }
        }

        private int GetRemainingCapacity(AInventory inventory)
        {
            int usedCapacity = GetUsedCapacity(inventory);
            int totalCapacity = (int)inventory.SizeLimit;
            int remainingCapacity = totalCapacity - usedCapacity;

            return remainingCapacity > 0 ? remainingCapacity : 0;
        }

        private static int TryAddToMatchingStacks(AGridBox[,] grid, AType item, int remainingCount, int maxStackSize)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    AGridBox slot = grid[x, y];

                    if (slot.Item == null || !CanStackTogether(slot.Item, item) || slot.Count >= maxStackSize)
                    {
                        continue;
                    }

                    int spaceLeft = maxStackSize - slot.Count;
                    int amountToAdd = remainingCount > spaceLeft ? spaceLeft : remainingCount;
                    slot.Count += amountToAdd;
                    remainingCount -= amountToAdd;

                    if (remainingCount == 0)
                    {
                        return 0;
                    }
                }
            }

            return remainingCount;
        }
    }
}
