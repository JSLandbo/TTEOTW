using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AHull : AType
    {
        protected AHull()
        {
            Stackable = false;
        }

        public float Health { get; set; }
        public float Durability { get; set; }
    }
}
