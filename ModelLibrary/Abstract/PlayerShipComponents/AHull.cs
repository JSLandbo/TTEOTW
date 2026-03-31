using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.PlayerShipComponents
{
    public abstract class AHull : AType
    {
        public float Health { get; set; }
        public float Durability { get; set; }

        public void TakeDamage(float damage)
        {
            if (damage <= 0.0f)
            {
                return;
            }

            Health = Math.Max(0.0f, Health - damage);
        }
    }
}