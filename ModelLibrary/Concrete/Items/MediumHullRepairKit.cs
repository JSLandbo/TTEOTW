using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class MediumHullRepairKit : AHullRepairKit
    {
        public MediumHullRepairKit(MediumHullRepairKit other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            RepairAmount = other.RepairAmount;
        }

        [JsonConstructor]
        public MediumHullRepairKit(short ID, string Name, float RepairAmount, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
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
