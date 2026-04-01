using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete
{
    [method: JsonConstructor]
    public class Player(Engine Engine, Hull Hull, Drill Drill, Inventory Inventory, Thruster Thruster, FuelTank FuelTank, ThermalPlating ThermalPlating, GadgetInventory? GadgetSlots = null, bool HasGadgetBelt = false)
        : APlayer(ThermalPlating, Engine, Hull, Drill, Inventory, Thruster, FuelTank, GadgetSlots ?? CreateDefaultGadgetSlots(), HasGadgetBelt)
    {
        private static GadgetInventory CreateDefaultGadgetSlots()
        {
            return new GadgetInventory(
                ID: 0,
                Items: new Grid(Vector2.Zero, new GridBox[6, 1]),
                SizeLimit: 6,
                Name: "Gadget Slots",
                Worth: 0,
                Weight: 0,
                MaxStackSize: 64);
        }
    }
}
