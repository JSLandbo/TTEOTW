using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete
{
    [method: JsonConstructor]
    public class Player(Engine Engine, Hull Hull, Drill Drill, Inventory Inventory, Thruster Thruster, FuelTank FuelTank, ThermalPlating ThermalPlating, GadgetInventory? GadgetSlots = null, bool HasGadgetBelt = false, float? CurrentHeat = null, float? CurrentHull = null, float? CurrentFuel = null)
        : APlayer(ThermalPlating, Engine, Hull, Drill, Inventory, Thruster, FuelTank, GadgetSlots ?? CreateDefaultGadgetSlots(Inventory.MaxStackSize), HasGadgetBelt, CurrentHeat, CurrentHull, CurrentFuel)
    {
        private static GadgetInventory CreateDefaultGadgetSlots(int maxStackSize)
        {
            return new GadgetInventory(
                ID: 0,
                Items: new Grid(Vector2.Zero, new GridBox[6, 1]),
                Name: "Gadget Slots",
                Worth: 0,
                Weight: 0,
                MaxStackSize: maxStackSize
            );
        }
    }
}
