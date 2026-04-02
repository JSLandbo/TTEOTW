using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AEngine : AType
    {
        protected AEngine()
        {
            Stackable = false;
        }

        public float Speed { get; set; }
        public float Acceleration { get; set; }
        public float StandbyFuelConsumption { get; set; }
        public float ActiveFuelConsumption { get; set; }
    }
}
