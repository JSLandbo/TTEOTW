using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AEngine : AType
    {
        public float StandbyFuelConsumption { get; set; } // When idling
        public float ActiveFuelConsumption { get; set; } // When Mining
    }
}
