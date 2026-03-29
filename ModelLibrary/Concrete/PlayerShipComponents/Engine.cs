using ModelLibrary.Abstract.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public class Engine : AEngine
    {
        public Engine(Engine original)
        {
            ID = original.ID;
            Name = original.Name;
            ActiveFuelConsumption = original.ActiveFuelConsumption;
            Worth = original.Worth;
            Weight = original.Weight;
        }

        [JsonConstructor]
        public Engine(short ID, float Speed, string Name, float ActiveFuelConsumption, float Worth, float Weight)
        {
            this.ID = ID;
            this.Name = Name;
            this.ActiveFuelConsumption = ActiveFuelConsumption;
            this.Worth = Worth;
            this.Weight = Weight;
        }
    }
}
