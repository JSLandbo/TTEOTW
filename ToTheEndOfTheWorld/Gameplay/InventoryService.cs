using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;

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

            return left.GetType() == right.GetType() && left.ID == right.ID;
        }

        public bool TryAdd(AInventory inventory, AType item, int count)
        {
            if (count <= 0)
            {
                return true;
            }

            if (!CanAccept(inventory, item, count))
            {
                return false;
            }

            var grid = inventory.Items.InternalGrid;
            var maxStackSize = GetMaxStackSize(inventory);
            var remainingCount = count;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot.Item == null || !CanStackTogether(slot.Item, item) || slot.Count >= maxStackSize)
                    {
                        continue;
                    }

                    var spaceLeft = maxStackSize - slot.Count;
                    var amountToAdd = remainingCount > spaceLeft ? spaceLeft : remainingCount;
                    slot.Count += amountToAdd;
                    remainingCount -= amountToAdd;

                    if (remainingCount == 0)
                    {
                        return true;
                    }
                }
            }

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot.Item != null)
                    {
                        continue;
                    }

                    var amountToAdd = remainingCount > maxStackSize ? maxStackSize : remainingCount;
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

        public bool CanAccept(AInventory inventory, AType item, int count)
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
            var usedCapacity = 0;
            var grid = inventory.Items.InternalGrid;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
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

            var usedCapacity = GetUsedCapacity(inventory);
            var percent = (usedCapacity / inventory.SizeLimit) * 100.0f;
            return (int)percent;
        }

        private int GetRemainingCapacity(AInventory inventory)
        {
            var usedCapacity = GetUsedCapacity(inventory);
            var totalCapacity = (int)inventory.SizeLimit;
            var remainingCapacity = totalCapacity - usedCapacity;
            return remainingCapacity > 0 ? remainingCapacity : 0;
        }
    }
}
