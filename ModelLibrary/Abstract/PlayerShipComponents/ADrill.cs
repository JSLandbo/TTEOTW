using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class ADrill : AType
    {
        public float Damage { get; set; }
        public float Hardness { get; set; } // How hard a mineral can this drill mine
        public int MiningAreaSize { get; set; }
        public float ActiveFuelConsumption { get; set; } // When mining
    }
}
