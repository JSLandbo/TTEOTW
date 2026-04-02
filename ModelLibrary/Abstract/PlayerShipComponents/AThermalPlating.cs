using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AThermalPlating : AType
    {
        protected AThermalPlating()
        {
            Stackable = false;
        }

        public float Thermals { get; set; }
        public float MaxThermals { get; set; }
        public float ThermalDissipation { get; set; }
    }
}
