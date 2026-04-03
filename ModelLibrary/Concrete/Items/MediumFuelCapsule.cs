using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class MediumFuelCapsule : AFuelCapsule
    {
        public MediumFuelCapsule(MediumFuelCapsule other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            FuelAmount = other.FuelAmount;
        }

        [JsonConstructor]
        public MediumFuelCapsule(short ID, string Name, float FuelAmount, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.FuelAmount = FuelAmount;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }
    }
}