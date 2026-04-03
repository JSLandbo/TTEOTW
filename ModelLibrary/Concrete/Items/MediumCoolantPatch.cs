using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class MediumCoolantPatch : ACoolantPatch
    {
        public MediumCoolantPatch(MediumCoolantPatch other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            CoolingAmount = other.CoolingAmount;
        }

        [JsonConstructor]
        public MediumCoolantPatch(short ID, string Name, float CoolingAmount, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.CoolingAmount = CoolingAmount;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }
    }
}
