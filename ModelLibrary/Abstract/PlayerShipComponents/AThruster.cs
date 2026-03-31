using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AThruster : AType
    {
        public float ActiveFuelConsumption { get; set; }
        public float ActiveHeatGeneration { get; set; }
        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float Power { get; set; } // Lifting capacity
    }
}
