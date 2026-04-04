using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Ids;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public sealed class GadgetInventory : AGadgetInventory
    {
        public const int DirtFilterSlotIndex = 4;
        public const int RockFilterSlotIndex = 5;
        public const int UtilitySlotCount = 2;

        public GadgetInventory(GadgetInventory original)
        {
            ID = original.ID;
            Items = CloneGrid(original.Items);
            MaxStackSize = original.MaxStackSize;
            Name = original.Name;
            Worth = original.Worth;
            Weight = original.Weight;
        }

        [JsonConstructor]
        public GadgetInventory(short ID, Grid Items, string Name, float Worth, float Weight, int MaxStackSize = 64)
        {
            this.ID = ID;
            this.Items = Items;
            this.MaxStackSize = MaxStackSize;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
        }

        public override bool CanPlaceInSlot(int slotIndex, AType item)
        {
            if (item == null)
            {
                return false;
            }

            if (IsConsumeableSlot(slotIndex))
            {
                return item is AConsumeable;
            }

            return slotIndex switch
            {
                DirtFilterSlotIndex => item.ID == GameIds.Items.Gadgets.DirtFilter,
                RockFilterSlotIndex => item.ID == GameIds.Items.Gadgets.RockFilter,
                _ => false
            };
        }

        private static bool IsConsumeableSlot(int slotIndex)
        {
            return slotIndex is 0 or 1 or 2 or 3;
        }

        private static Grid CloneGrid(AGrid original)
        {
            AGridBox[,] sourceGrid = original.InternalGrid;
            GridBox[,] clonedGrid = new GridBox[sourceGrid.GetLength(0), sourceGrid.GetLength(1)];

            for (int x = 0; x < sourceGrid.GetLength(0); x++)
            {
                for (int y = 0; y < sourceGrid.GetLength(1); y++)
                {
                    AGridBox slot = sourceGrid[x, y];
                    clonedGrid[x, y] = new GridBox(slot.Item, slot.Count);
                }
            }

            return new Grid(original.InternalCoordinate, clonedGrid);
        }
    }
}
