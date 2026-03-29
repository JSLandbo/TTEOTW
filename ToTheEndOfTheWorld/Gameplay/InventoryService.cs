using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class InventoryService
    {
        public const int MaxStackSize = 64;

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
            var remainingCount = count;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot.Item == null || slot.Item.ID != item.ID || slot.Count >= MaxStackSize)
                    {
                        continue;
                    }

                    var spaceLeft = MaxStackSize - slot.Count;
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

                    var amountToAdd = remainingCount > MaxStackSize ? MaxStackSize : remainingCount;
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

            var availableSpace = 0;
            var grid = inventory.Items.InternalGrid;

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slot = grid[x, y];

                    if (slot.Item == null)
                    {
                        availableSpace += MaxStackSize;
                    }
                    else if (slot.Item.ID == item.ID)
                    {
                        availableSpace += MaxStackSize - slot.Count;
                    }

                    if (availableSpace >= count)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
