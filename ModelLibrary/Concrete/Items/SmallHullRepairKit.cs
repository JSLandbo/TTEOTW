using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class SmallHullRepairKit : AHullRepairKit
    {
        public SmallHullRepairKit(SmallHullRepairKit other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            RepairAmount = other.RepairAmount;
        }

        [JsonConstructor]
        public SmallHullRepairKit(short ID, string Name, float RepairAmount, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.RepairAmount = RepairAmount;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }
    }
}
