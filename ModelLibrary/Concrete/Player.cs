using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete
{
    public class Player : APlayer
    {
        [JsonConstructor]
        public Player(Engine Engine, Hull Hull, Drill Drill, Inventory Inventory, Thruster Thruster, FuelTank FuelTank, ThermalPlating ThermalPlating, Grid? GadgetSlots = null, bool HasGadgetBelt = false)
            : base(ThermalPlating, Engine, Hull, Drill, Inventory, Thruster, FuelTank, GadgetSlots ?? CreateDefaultGadgetSlots(), HasGadgetBelt)
        {
        }

        private static Grid CreateDefaultGadgetSlots()
        {
            return new Grid(Vector2.Zero, new GridBox[6, 1]);
        }
    }
}
