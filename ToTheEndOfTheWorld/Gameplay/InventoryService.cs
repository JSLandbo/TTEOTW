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
            return TryAddToGrid(inventory.Items, item, count, GetMaxStackSize(inventory));
        }

        public int AddToInventory(AInventory inventory, AType item, int count)
        {
            return AddToGrid(inventory.Items, item, count, GetMaxStackSize(inventory));
        }

        public bool TryAddToGrid(AGrid grid, AType item, int count, int maxStackSize)
        {
            return AddToGrid(grid, item, count, maxStackSize) == count;
        }

        public int AddToGrid(AGrid grid, AType item, int count, int maxStackSize)
        {
            if (count <= 0)
            {
                return 0;
            }

            AGridBox[,] slots = grid.InternalGrid;
            int remainingCount = TryAddToMatchingStacks(slots, grid, item, count, maxStackSize);

            if (remainingCount == 0) return count;

            for (int y = 0; y < slots.GetLength(1); y++)
            {
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    AGridBox slot = slots[x, y];

                    if (slot.Item != null) continue;
                    if (grid.CanPlaceInSlot(slot, item) == false) continue;

                    int amountToAdd = remainingCount > maxStackSize ? maxStackSize : remainingCount;
                    slot.Item = item;
                    slot.Count = amountToAdd;
                    remainingCount -= amountToAdd;

                    if (remainingCount == 0)
                    {
                        return count;
                    }
                }
            }

            return count - remainingCount;
        }

        public bool TryAddToMatchingStacks(AInventory inventory, AType item, int count)
        {
            if (count <= 0)
            {
                return true;
            }

            return TryAddToMatchingStacks(inventory.Items.InternalGrid, inventory.Items, item, count, GetMaxStackSize(inventory)) == 0;
        }

        public int GetMaxStackSize(AInventory inventory)
        {
            if (inventory == null || inventory.MaxStackSize <= 0)
            {
                return DefaultMaxStackSize;
            }

            return inventory.MaxStackSize;
        }

        public int GetUsedSlots(AInventory inventory)
        {
            int usedSlots = 0;
            AGridBox[,] grid = inventory.Items.InternalGrid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y].Item != null) usedSlots++;
                }
            }

            return usedSlots;
        }

        public int GetTotalSlots(AInventory inventory)
        {
            AGridBox[,] grid = inventory.Items.InternalGrid;
            return grid.GetLength(0) * grid.GetLength(1);
        }

        public int GetUsedSlotsPercent(AInventory inventory)
        {
            int totalSlots = GetTotalSlots(inventory);
            if (totalSlots <= 0) return 0;

            int usedSlots = GetUsedSlots(inventory);
            return (int)((usedSlots / (float)totalSlots) * 100.0f);
        }

        public void SortByName(AInventory inventory)
        {
            SortGridByName(inventory.Items, GetMaxStackSize(inventory));
        }

        public void SortGridByName(AGrid grid, int maxStackSize)
        {
            AGridBox[,] slots = grid.InternalGrid;
            List<(AType Item, int Count)> entries = [];

            for (int y = 0; y < slots.GetLength(1); y++)
            {
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    AGridBox slot = slots[x, y];

                    if (slot.Item == null || slot.Count <= 0)
                    {
                        continue;
                    }

                    entries.Add((slot.Item, slot.Count));
                }
            }

            entries.Sort((left, right) => string.Compare(left.Item.Name, right.Item.Name, StringComparison.OrdinalIgnoreCase));

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

            for (int y = 0; y < slots.GetLength(1); y++)
            {
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    AGridBox slot = slots[x, y];

                    if (entryIndex < groupedEntries.Count)
                    {
                        (AType item, int _) = groupedEntries[entryIndex];
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

        public int GetStackableSpace(AInventory inventory, AType item)
        {
            if (item == null || !item.Stackable) return 0;

            int maxStackSize = GetMaxStackSize(inventory);
            int space = 0;
            AGrid ownerGrid = inventory.Items;

            foreach (AGridBox slot in EnumerateSlots(inventory.Items.InternalGrid))
            {
                if (CanStackTogether(slot.Item, item) && slot.Count < maxStackSize)
                {
                    if (ownerGrid.CanPlaceInSlot(slot, item) != false)
                    {
                        space += maxStackSize - slot.Count;
                    }
                }
            }

            return space;
        }

        public bool HasEmptySlot(AInventory inventory)
        {
            foreach (AGridBox slot in EnumerateSlots(inventory.Items.InternalGrid))
            {
                if (slot.Item == null) return true;
            }
            return false;
        }

        public bool HasEmptySlotFor(AInventory inventory, AType item)
        {
            AGrid ownerGrid = inventory.Items;
            foreach (AGridBox slot in EnumerateSlots(inventory.Items.InternalGrid))
            {
                if (slot.Item == null && ownerGrid.CanPlaceInSlot(slot, item) != false)
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<AGridBox> EnumerateSlots(AGridBox[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    yield return grid[x, y];
                }
            }
        }

        private static int TryAddToMatchingStacks(AGridBox[,] grid, AGrid ownerGrid, AType item, int remainingCount, int maxStackSize)
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

                    if (ownerGrid?.CanPlaceInSlot(slot, item) == false)
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
