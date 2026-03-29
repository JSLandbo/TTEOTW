using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using System.Collections.Generic;
using System.Linq;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class GameItemDefinition
    {
        public GameItemDefinition(string name, Dictionary<PlayerOrientation, Texture2D> textures, AType definition, int equipmentShopOrder = -1)
        {
            Name = name;
            Textures = textures;
            Definition = definition;
            EquipmentShopOrder = equipmentShopOrder;
        }

        public string Name { get; }
        public Dictionary<PlayerOrientation, Texture2D> Textures { get; }
        public AType Definition { get; }
        public int EquipmentShopOrder { get; }

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

    public sealed class GameItemsRepository : Dictionary<int, GameItemDefinition>
    {
        public GameItemsRepository(ContentManager manager)
        {
            InitializeCollection(manager);
        }

        public AType Create(short itemId)
        {
            return TryGetValue(itemId, out var itemDefinition) ? itemDefinition.Create() : null;
        }

        public T Create<T>(short itemId) where T : AType
        {
            return Create(itemId) as T;
        }

        public IReadOnlyList<GameItemDefinition> GetEquipmentShopDefinitions()
        {
            return Values
                .Where(item => item.EquipmentShopOrder >= 0)
                .OrderBy(item => item.EquipmentShopOrder)
                .ToList();
        }

        private void InitializeCollection(ContentManager manager)
        {
            var scrapPlating = new ThermalPlating(ID: 10000, Thermals: 0, MaxThermals: 100, ThermalDissipation: 2, Name: "Scrap Thermal Plating", Worth: 5, Weight: 5);
            var copperPlating = new ThermalPlating(ID: 10001, Thermals: 0, MaxThermals: 160, ThermalDissipation: 3, Name: "Copper Thermal Plating", Worth: 100, Weight: 6);
            var ironPlating = new ThermalPlating(ID: 10002, Thermals: 0, MaxThermals: 240, ThermalDissipation: 5, Name: "Iron Thermal Plating", Worth: 400, Weight: 7);
            var goldPlating = new ThermalPlating(ID: 10003, Thermals: 0, MaxThermals: 360, ThermalDissipation: 7, Name: "Gold Thermal Plating", Worth: 1500, Weight: 8);
            var crystalPlating = new ThermalPlating(ID: 10004, Thermals: 0, MaxThermals: 520, ThermalDissipation: 10, Name: "Crystal Thermal Plating", Worth: 5000, Weight: 8);
            var diamondPlating = new ThermalPlating(ID: 10005, Thermals: 0, MaxThermals: 760, ThermalDissipation: 14, Name: "Diamond Thermal Plating", Worth: 15000, Weight: 7);
            var radioactivePlating = new ThermalPlating(ID: 10006, Thermals: 0, MaxThermals: 1100, ThermalDissipation: 19, Name: "Radioactive Thermal Plating", Worth: 50000, Weight: 6);
            var rainbowPlating = new ThermalPlating(ID: 10007, Thermals: 0, MaxThermals: 1600, ThermalDissipation: 25, Name: "Rainbow Thermal Plating", Worth: 150000, Weight: 5);
            var mythrilPlating = new ThermalPlating(ID: 10008, Thermals: 0, MaxThermals: 2300, ThermalDissipation: 34, Name: "Mythril Thermal Plating", Worth: 500000, Weight: 4);
            var adamantPlating = new ThermalPlating(ID: 10009, Thermals: 0, MaxThermals: 3200, ThermalDissipation: 45, Name: "Adamant Thermal Plating", Worth: 1500000, Weight: 4);

            AddDefinition(10000, "ScrapThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), scrapPlating, equipmentShopOrder: 0);
            AddDefinition(10001, "CopperThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), copperPlating, equipmentShopOrder: 10);
            AddDefinition(10002, "IronThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), ironPlating, equipmentShopOrder: 20);
            AddDefinition(10003, "GoldThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), goldPlating, equipmentShopOrder: 30);
            AddDefinition(10004, "CrystalThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), crystalPlating, equipmentShopOrder: 40);
            AddDefinition(10005, "DiamondThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), diamondPlating, equipmentShopOrder: 50);
            AddDefinition(10006, "RadioactiveThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), radioactivePlating, equipmentShopOrder: 60);
            AddDefinition(10007, "RainbowThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), rainbowPlating, equipmentShopOrder: 70);
            AddDefinition(10008, "MythrilThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), mythrilPlating, equipmentShopOrder: 80);
            AddDefinition(10009, "AdamantThermalPlating", LoadSingleTexture(manager, "Graphics/Player/ThermalPlatings/ScrapThermalPlating/ScrapThermalPlating"), adamantPlating, equipmentShopOrder: 90);

            AddDefinition(10100, "ScrapEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10100, Speed: 2, Name: "Scrap Engine", ActiveFuelConsumption: 1.0f, Worth: 5, Weight: 5), equipmentShopOrder: 1);
            AddDefinition(10101, "CopperEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10101, Speed: 4, Name: "Copper Engine", ActiveFuelConsumption: 0.92f, Worth: 100, Weight: 6), equipmentShopOrder: 11);
            AddDefinition(10102, "IronEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10102, Speed: 6, Name: "Iron Engine", ActiveFuelConsumption: 0.82f, Worth: 400, Weight: 7), equipmentShopOrder: 21);
            AddDefinition(10103, "GoldEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10103, Speed: 20, Name: "Gold Engine", ActiveFuelConsumption: 0.7f, Worth: 1500, Weight: 8), equipmentShopOrder: 31);
            AddDefinition(10104, "CrystalEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10104, Speed: 40, Name: "Crystal Engine", ActiveFuelConsumption: 0.58f, Worth: 5000, Weight: 8), equipmentShopOrder: 41);
            AddDefinition(10105, "DiamondEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10105, Speed: 6, Name: "Diamond Engine", ActiveFuelConsumption: 0.46f, Worth: 15000, Weight: 7), equipmentShopOrder: 51);
            AddDefinition(10106, "RadioactiveEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10106, Speed: 12, Name: "Radioactive Engine", ActiveFuelConsumption: 0.34f, Worth: 50000, Weight: 6), equipmentShopOrder: 61);
            AddDefinition(10107, "RainbowEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10107, Speed: 320, Name: "Rainbow Engine", ActiveFuelConsumption: 0.24f, Worth: 150000, Weight: 5), equipmentShopOrder: 71);
            AddDefinition(10108, "MythrilEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10108, Speed: 640, Name: "Mythril Engine", ActiveFuelConsumption: 0.18f, Worth: 500000, Weight: 4), equipmentShopOrder: 81);
            AddDefinition(10109, "AdamantEngine", LoadSingleTexture(manager, "Graphics/Player/Engines/ScrapEngine/ScrapEngine"), new Engine(ID: 10109, Speed: 1200, Name: "Adamant Engine", ActiveFuelConsumption: 0.12f, Worth: 1500000, Weight: 4), equipmentShopOrder: 91);

            AddDefinition(10200, "ScrapFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10200, Capacity: 200, Fuel: 200, Name: "Scrap Fuel Tank", Worth: 5, Weight: 5), equipmentShopOrder: 2);
            AddDefinition(10201, "CopperFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10201, Capacity: 350, Fuel: 350, Name: "Copper Fuel Tank", Worth: 100, Weight: 6), equipmentShopOrder: 12);
            AddDefinition(10202, "IronFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10202, Capacity: 550, Fuel: 550, Name: "Iron Fuel Tank", Worth: 400, Weight: 7), equipmentShopOrder: 22);
            AddDefinition(10203, "GoldFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10203, Capacity: 850, Fuel: 850, Name: "Gold Fuel Tank", Worth: 1500, Weight: 8), equipmentShopOrder: 32);
            AddDefinition(10204, "CrystalFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10204, Capacity: 1300, Fuel: 1300, Name: "Crystal Fuel Tank", Worth: 5000, Weight: 8), equipmentShopOrder: 42);
            AddDefinition(10205, "DiamondFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10205, Capacity: 1900, Fuel: 1900, Name: "Diamond Fuel Tank", Worth: 15000, Weight: 7), equipmentShopOrder: 52);
            AddDefinition(10206, "RadioactiveFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10206, Capacity: 2800, Fuel: 2800, Name: "Radioactive Fuel Tank", Worth: 50000, Weight: 6), equipmentShopOrder: 62);
            AddDefinition(10207, "RainbowFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10207, Capacity: 4000, Fuel: 4000, Name: "Rainbow Fuel Tank", Worth: 150000, Weight: 5), equipmentShopOrder: 72);
            AddDefinition(10208, "MythrilFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10208, Capacity: 6000, Fuel: 6000, Name: "Mythril Fuel Tank", Worth: 500000, Weight: 4), equipmentShopOrder: 82);
            AddDefinition(10209, "AdamantFuelTank", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new FuelTank(ID: 10209, Capacity: 10000, Fuel: 10000, Name: "Adamant Fuel Tank", Worth: 1500000, Weight: 4), equipmentShopOrder: 92);

            AddDefinition(10300, "ScrapContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10300, Items: CreateInventoryGrid(1, 1), SizeLimit: 16, Name: "Scrap Container", Worth: 5, Weight: 5, MaxStackSize: 64), equipmentShopOrder: 4);
            AddDefinition(10301, "CopperContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10301, Items: CreateInventoryGrid(1, 3), SizeLimit: 40, Name: "Copper Container", Worth: 100, Weight: 6, MaxStackSize: 64), equipmentShopOrder: 14);
            AddDefinition(10302, "IronContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10302, Items: CreateInventoryGrid(2, 3), SizeLimit: 80, Name: "Iron Container", Worth: 400, Weight: 7, MaxStackSize: 64), equipmentShopOrder: 24);
            AddDefinition(10303, "GoldContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10303, Items: CreateInventoryGrid(3, 3), SizeLimit: 140, Name: "Gold Container", Worth: 1500, Weight: 8, MaxStackSize: 64), equipmentShopOrder: 34);
            AddDefinition(10304, "CrystalContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10304, Items: CreateInventoryGrid(3, 4), SizeLimit: 220, Name: "Crystal Container", Worth: 5000, Weight: 8, MaxStackSize: 64), equipmentShopOrder: 44);
            AddDefinition(10305, "DiamondContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10305, Items: CreateInventoryGrid(4, 4), SizeLimit: 340, Name: "Diamond Container", Worth: 15000, Weight: 7, MaxStackSize: 64), equipmentShopOrder: 54);
            AddDefinition(10306, "RadioactiveContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10306, Items: CreateInventoryGrid(4, 5), SizeLimit: 500, Name: "Radioactive Container", Worth: 50000, Weight: 6, MaxStackSize: 64), equipmentShopOrder: 64);
            AddDefinition(10307, "RainbowContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10307, Items: CreateInventoryGrid(5, 5), SizeLimit: 750, Name: "Rainbow Container", Worth: 150000, Weight: 5, MaxStackSize: 64), equipmentShopOrder: 74);
            AddDefinition(10308, "MythrilContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10308, Items: CreateInventoryGrid(8, 6), SizeLimit: 8192, Name: "Mythril Container", Worth: 500000, Weight: 4, MaxStackSize: 128), equipmentShopOrder: 84);
            AddDefinition(10309, "AdamantContainer", LoadSingleTexture(manager, "Graphics/Player/FuelTanks/ScrapFuelTank/ScrapFuelTank"), new Inventory(ID: 10309, Items: CreateInventoryGrid(18, 8), SizeLimit: 65536, Name: "Adamant Container", Worth: 1500000, Weight: 4, MaxStackSize: 512), equipmentShopOrder: 94);

            AddDefinition(10400, "ScrapThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10400, Speed: 8, Acceleration: 0.1f, Power: 50, Name: "Scrap Thruster", ActiveFuelConsumption: 2.0f, Weight: 5, Worth: 5, MinimumVelocity: 1f), equipmentShopOrder: 3);
            AddDefinition(10401, "CopperThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10401, Speed: 12, Acceleration: 0.16f, Power: 75, Name: "Copper Thruster", ActiveFuelConsumption: 2.2f, Weight: 6, Worth: 100, MinimumVelocity: 1f), equipmentShopOrder: 13);
            AddDefinition(10402, "IronThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10402, Speed: 17, Acceleration: 0.24f, Power: 110, Name: "Iron Thruster", ActiveFuelConsumption: 2.5f, Weight: 7, Worth: 400, MinimumVelocity: 1f), equipmentShopOrder: 23);
            AddDefinition(10403, "GoldThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10403, Speed: 24, Acceleration: 0.34f, Power: 160, Name: "Gold Thruster", ActiveFuelConsumption: 2.9f, Weight: 8, Worth: 1500, MinimumVelocity: 1f), equipmentShopOrder: 33);
            AddDefinition(10404, "CrystalThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10404, Speed: 32, Acceleration: 0.46f, Power: 230, Name: "Crystal Thruster", ActiveFuelConsumption: 3.4f, Weight: 8, Worth: 5000, MinimumVelocity: 1f), equipmentShopOrder: 43);
            AddDefinition(10405, "DiamondThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10405, Speed: 120, Acceleration: 0.60f, Power: 320, Name: "Diamond Thruster", ActiveFuelConsumption: 4.0f, Weight: 7, Worth: 15000, MinimumVelocity:1f), equipmentShopOrder: 53);
            AddDefinition(10406, "RadioactiveThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10406, Speed: 1520, Acceleration: 0.78f, Power: 440, Name: "Radioactive Thruster", ActiveFuelConsumption: 4.7f, Weight: 6, Worth: 50000, MinimumVelocity: 1f), equipmentShopOrder: 63);
            AddDefinition(10407, "RainbowThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10407, Speed: 7200, Acceleration: 1.0f, Power: 1500, Name: "Rainbow Thruster", ActiveFuelConsumption: 5.5f, Weight: 5, Worth: 150000, MinimumVelocity: 1.0f), equipmentShopOrder: 73);
            AddDefinition(10408, "MythrilThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10408, Speed: 14000, Acceleration: 1.4f, Power: 2200, Name: "Mythril Thruster", ActiveFuelConsumption: 6.2f, Weight: 4, Worth: 500000, MinimumVelocity: 1.0f), equipmentShopOrder: 83);
            AddDefinition(10409, "AdamantThruster", LoadSingleTexture(manager, "Graphics/Player/Thrusters/ScrapThruster/ScrapThruster"), new Thruster(ID: 10409, Speed: 28000, Acceleration: 1.9f, Power: 3200, Name: "Adamant Thruster", ActiveFuelConsumption: 7.0f, Weight: 4, Worth: 1500000, MinimumVelocity: 1.0f), equipmentShopOrder: 93);

            AddDefinition(10500, "ScrapHull", LoadHullTextures(manager, "Graphics/Player/Hulls/ScrapHull/Player", "Graphics/Player/Hulls/ScrapHull/PlayerLeft", "Graphics/Player/Hulls/ScrapHull/PlayerUp", "Graphics/Player/Hulls/ScrapHull/PlayerRight", "Graphics/Player/Hulls/ScrapHull/PlayerDown"), new Hull(ID: 10500, Durability: 5, Health: 50, Name: "Scrap Hull", Worth: 5, Weight: 5), equipmentShopOrder: 5);
            AddDefinition(10501, "CopperHull", LoadHullTextures(manager, "Graphics/Player/Hulls/CopperHull/CopperShip", "Graphics/Player/Hulls/CopperHull/CopperShip", "Graphics/Player/Hulls/CopperHull/CopperShip", "Graphics/Player/Hulls/CopperHull/CopperShip", "Graphics/Player/Hulls/CopperHull/CopperShipDown"), new Hull(ID: 10501, Durability: 10, Health: 130, Name: "Copper Hull", Worth: 100, Weight: 8), equipmentShopOrder: 15);
            AddDefinition(10502, "IronHull", LoadHullTextures(manager, "Graphics/Player/Hulls/IronHull/IronShip", "Graphics/Player/Hulls/IronHull/IronShip", "Graphics/Player/Hulls/IronHull/IronShip", "Graphics/Player/Hulls/IronHull/IronShip", "Graphics/Player/Hulls/IronHull/IronShipDown"), new Hull(ID: 10502, Durability: 18, Health: 260, Name: "Iron Hull", Worth: 400, Weight: 10), equipmentShopOrder: 25);
            AddDefinition(10503, "GoldHull", LoadHullTextures(manager, "Graphics/Player/Hulls/GoldHull/GoldShip", "Graphics/Player/Hulls/GoldHull/GoldShip", "Graphics/Player/Hulls/GoldHull/GoldShip", "Graphics/Player/Hulls/GoldHull/GoldShip", "Graphics/Player/Hulls/GoldHull/GoldShipDown"), new Hull(ID: 10503, Durability: 30, Health: 480, Name: "Gold Hull", Worth: 1500, Weight: 12), equipmentShopOrder: 35);
            AddDefinition(10504, "CrystalHull", LoadHullTextures(manager, "Graphics/Player/Hulls/CrystalHull/CrystalShip", "Graphics/Player/Hulls/CrystalHull/CrystalShip", "Graphics/Player/Hulls/CrystalHull/CrystalShip", "Graphics/Player/Hulls/CrystalHull/CrystalShip", "Graphics/Player/Hulls/CrystalHull/CrystalShipDown"), new Hull(ID: 10504, Durability: 48, Health: 820, Name: "Crystal Hull", Worth: 5000, Weight: 12), equipmentShopOrder: 45);
            AddDefinition(10505, "DiamondHull", LoadHullTextures(manager, "Graphics/Player/Hulls/DiamondHull/DiamondShip", "Graphics/Player/Hulls/DiamondHull/DiamondShip", "Graphics/Player/Hulls/DiamondHull/DiamondShip", "Graphics/Player/Hulls/DiamondHull/DiamondShip", "Graphics/Player/Hulls/DiamondHull/DiamondShipDown"), new Hull(ID: 10505, Durability: 72, Health: 1300, Name: "Diamond Hull", Worth: 15000, Weight: 11), equipmentShopOrder: 55);
            AddDefinition(10506, "RadioactiveHull", LoadHullTextures(manager, "Graphics/Player/Hulls/RadioactiveHull/RadioactiveShip", "Graphics/Player/Hulls/RadioactiveHull/RadioactiveShip", "Graphics/Player/Hulls/RadioactiveHull/RadioactiveShip", "Graphics/Player/Hulls/RadioactiveHull/RadioactiveShip", "Graphics/Player/Hulls/RadioactiveHull/RadioactiveShipDown"), new Hull(ID: 10506, Durability: 104, Health: 2000, Name: "Radioactive Hull", Worth: 50000, Weight: 10), equipmentShopOrder: 65);
            AddDefinition(10507, "RainbowHull", LoadHullTextures(manager, "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: 10507, Durability: 150, Health: 3000, Name: "Rainbow Hull", Worth: 150000, Weight: 9), equipmentShopOrder: 75);
            AddDefinition(10508, "MythrilHull", LoadHullTextures(manager, "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: 10508, Durability: 220, Health: 4500, Name: "Mythril Hull", Worth: 500000, Weight: 8), equipmentShopOrder: 85);
            AddDefinition(10509, "AdamantHull", LoadHullTextures(manager, "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShip", "Graphics/Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: 10509, Durability: 320, Health: 7000, Name: "Adamant Hull", Worth: 1500000, Weight: 8), equipmentShopOrder: 95);

            AddDefinition(10600, "ScrapDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/ScrapDrill/ScrapDrill"), new Drill(ID: 10600, Hardness: 5f, Damage: 0.1f, Name: "Scrap Drill", Worth: 5, Weight: 5, MiningAreaSize: 1), equipmentShopOrder: 6);
            AddDefinition(10601, "CopperDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/CopperDrill/CopperDrill"), new Drill(ID: 10601, Hardness: 15f, Damage: 0.25f, Name: "Copper Drill", Worth: 100, Weight: 6, MiningAreaSize: 1), equipmentShopOrder: 16);
            AddDefinition(10602, "IronDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/IronDrill/IronDrill"), new Drill(ID: 10602, Hardness: 35f, Damage: 1.2f, Name: "Iron Drill", Worth: 400, Weight: 7, MiningAreaSize: 1), equipmentShopOrder: 26);
            AddDefinition(10603, "GoldDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/GoldDrill/GoldDrill"), new Drill(ID: 10603, Hardness: 80f, Damage: 3.0f, Name: "Gold Drill", Worth: 1500, Weight: 8, MiningAreaSize: 1), equipmentShopOrder: 36);
            AddDefinition(10604, "CrystalDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/CrystalDrill/CrystalDrill"), new Drill(ID: 10604, Hardness: 180f, Damage: 6.5f, Name: "Crystal Drill", Worth: 5000, Weight: 8, MiningAreaSize: 1), equipmentShopOrder: 46);
            AddDefinition(10605, "DiamondDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/DiamondDrill/DiamondDrill"), new Drill(ID: 10605, Hardness: 400f, Damage: 15.0f, Name: "Diamond Drill", Worth: 15000, Weight: 7, MiningAreaSize: 1), equipmentShopOrder: 56);
            AddDefinition(10606, "RadioactiveDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/RadioactiveDrill/RadioactiveDrill"), new Drill(ID: 10606, Hardness: 900f, Damage: 80.0f, Name: "Radioactive Drill", Worth: 50000, Weight: 6, MiningAreaSize: 1), equipmentShopOrder: 66);
            AddDefinition(10607, "RainbowDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: 10607, Hardness: 2000f, Damage: 400.0f, Name: "Rainbow Drill", Worth: 150000, Weight: 5, MiningAreaSize: 1), equipmentShopOrder: 76);
            AddDefinition(10608, "MythrilDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: 10608, Hardness: 4000f, Damage: 1500.0f, Name: "Mythril Drill", Worth: 500000, Weight: 4, MiningAreaSize: 3), equipmentShopOrder: 86);
            AddDefinition(10609, "AdamantDrill", LoadDrillTextures(manager, "Graphics/Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: 10609, Hardness: 10000f, Damage: 8000.0f, Name: "Adamant Drill", Worth: 1500000, Weight: 4, MiningAreaSize: 9), equipmentShopOrder: 96);
        }

        private void AddDefinition(int id, string name, Dictionary<PlayerOrientation, Texture2D> textures, AType definition, int equipmentShopOrder = -1)
        {
            Add(id, new GameItemDefinition(name, textures, definition, equipmentShopOrder));
        }

        private static Dictionary<PlayerOrientation, Texture2D> LoadSingleTexture(ContentManager manager, string assetName)
        {
            return new Dictionary<PlayerOrientation, Texture2D>
            {
                { PlayerOrientation.Base, manager.Load<Texture2D>(assetName) }
            };
        }

        private static Dictionary<PlayerOrientation, Texture2D> LoadDrillTextures(ContentManager manager, string assetPrefix)
        {
            return new Dictionary<PlayerOrientation, Texture2D>
            {
                { PlayerOrientation.Left, manager.Load<Texture2D>($"{assetPrefix}Left") },
                { PlayerOrientation.Up, manager.Load<Texture2D>($"{assetPrefix}Up") },
                { PlayerOrientation.Right, manager.Load<Texture2D>($"{assetPrefix}Right") },
                { PlayerOrientation.Down, manager.Load<Texture2D>($"{assetPrefix}Down") }
            };
        }

        private static Dictionary<PlayerOrientation, Texture2D> LoadHullTextures(ContentManager manager, string baseAsset, string leftAsset, string upAsset, string rightAsset, string downAsset)
        {
            return new Dictionary<PlayerOrientation, Texture2D>
            {
                { PlayerOrientation.Base, manager.Load<Texture2D>(baseAsset) },
                { PlayerOrientation.Left, manager.Load<Texture2D>(leftAsset) },
                { PlayerOrientation.Up, manager.Load<Texture2D>(upAsset) },
                { PlayerOrientation.Right, manager.Load<Texture2D>(rightAsset) },
                { PlayerOrientation.Down, manager.Load<Texture2D>(downAsset) }
            };
        }

        private static Grid CreateInventoryGrid(int columns, int rows)
        {
            return new Grid(Microsoft.Xna.Framework.Vector2.Zero, new GridBox[columns, rows]);
        }
    }
}
