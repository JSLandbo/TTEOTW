using ModelLibrary.Abstract.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public class Thruster : AThruster
    {
        public Thruster(Thruster original)
        {
            ID = original.ID;
            Speed = original.Speed;
            Acceleration = original.Acceleration;
            Power = original.Power;
            Name = original.Name;
            ActiveFuelConsumption = original.ActiveFuelConsumption;
            ActiveHeatGeneration = original.ActiveHeatGeneration;
            Weight = original.Weight;
            Worth = original.Worth;
            MinimumVelocity = original.MinimumVelocity;
        }

        [JsonConstructor]
        public Thruster(short ID, float Speed, float Acceleration, float Power, string Name, float ActiveFuelConsumption, float ActiveHeatGeneration, float Weight, float Worth, float MinimumVelocity)
        {
            this.ID = ID;
            this.Speed = Speed;
            this.Acceleration = Acceleration;
            this.Power = Power;
            this.Name = Name;
            this.ActiveFuelConsumption = ActiveFuelConsumption;
            this.ActiveHeatGeneration = ActiveHeatGeneration;
            this.Weight = Weight;
            this.Worth = Worth;
            this.MinimumVelocity = MinimumVelocity;
        }
    }
}
