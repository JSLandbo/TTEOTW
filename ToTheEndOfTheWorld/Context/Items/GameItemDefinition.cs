using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class GameItemDefinition
    {
        public GameItemDefinition(
            string name,
            Dictionary<PlayerOrientation, Texture2D> textures,
            AType definition,
            bool buyable = false,
            GameItemType type = GameItemType.Item,
            EquipmentType equipmentType = EquipmentType.None,
            int tier = 0)
        {
            Name = name;
            Textures = textures;
            Definition = definition;
            Buyable = buyable;
            Type = type;
            EquipmentType = equipmentType;
            Tier = tier;
        }

        public string Name { get; }
        public Dictionary<PlayerOrientation, Texture2D> Textures { get; }
        public AType Definition { get; }
        public bool Buyable { get; }
        public GameItemType Type { get; }
        public EquipmentType EquipmentType { get; }
        public int Tier { get; }

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
                _ => null
            };
        }
    }
}
