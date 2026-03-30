using ModelLibrary.Abstract.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public class Drill : ADrill
    {
        public Drill(Drill original)
        {
            ID = original.ID;
            Hardness = original.Hardness;
            Damage = original.Damage;
            MiningAreaSize = original.MiningAreaSize;
            ActiveFuelConsumption = original.ActiveFuelConsumption;
            Name = original.Name;
            Worth = original.Worth;
            Weight = original.Weight;
        }

        [JsonConstructor]
        public Drill(short ID, float Hardness, float Damage, string Name, float Worth = 0, float Weight = 0, int MiningAreaSize = 1, float ActiveFuelConsumption = 0.0f)
        {
            this.ID = ID;
            this.Hardness = Hardness;
            this.Damage = Damage;
            this.MiningAreaSize = MiningAreaSize;
            this.ActiveFuelConsumption = ActiveFuelConsumption;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
        }
    }
}
