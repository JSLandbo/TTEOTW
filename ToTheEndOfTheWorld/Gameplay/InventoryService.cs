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
            int remainingCount = count;

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
                        return true;
                    }
                }
            }

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

        private int GetRemainingCapacity(AInventory inventory)
        {
            int usedCapacity = GetUsedCapacity(inventory);
            int totalCapacity = (int)inventory.SizeLimit;
            int remainingCapacity = totalCapacity - usedCapacity;

            return remainingCapacity > 0 ? remainingCapacity : 0;
        }
    }
}
