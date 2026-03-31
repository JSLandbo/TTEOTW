using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Items;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context.Items
{
    public sealed class GameItemDefinition(
        string name,
        Dictionary<PlayerOrientation, Texture2D> textures,
        AType definition,
        bool buyable = false,
        EGameItemType type = EGameItemType.Item,
        EEquipmentType equipmentType = EEquipmentType.None,
        int tier = 0)
    {
        public string Name { get; } = name; // TODO: Name not used? What is this class even for?
        public Dictionary<PlayerOrientation, Texture2D> Textures { get; } = textures;
        public AType Definition { get; } = definition;
        public bool Buyable { get; } = buyable;
        public EGameItemType Type { get; } = type;
        public EEquipmentType EquipmentType { get; } = equipmentType;
        public int Tier { get; } = tier;

        public AType Create()
        {
            return Definition switch
            {
                ThermalPlating plating => new ThermalPlating(plating),
                Hull hull => new Hull(hull),
                Drill drill => new Drill(drill),
                Engine engine => new Engine(engine),
                Inventory inventory => new Inventory(inventory),
                FuelTank fuelTank => new FuelTank(fuelTank),
                Thruster thruster => new Thruster(thruster),
                Item item => new Item(item),
                _ => null
            };
        }
    }
}
