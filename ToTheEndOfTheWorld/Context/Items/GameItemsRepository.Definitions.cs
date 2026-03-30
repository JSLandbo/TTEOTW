using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using ModelLibrary.Ids;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed partial class GameItemsRepository
    {
        private const int ScrapTierWorth = 25;
        private const int CopperTierWorth = 150;
        private const int IronTierWorth = 900;
        private const int GoldTierWorth = 4500;
        private const int CrystalTierWorth = 22500;
        private const int DiamondTierWorth = 100000;
        private const int RadioactiveTierWorth = 450000;
        private const int RainbowTierWorth = 2000000;
        private const int MythrilTierWorth = 9000000;
        private const int AdamantTierWorth = 40000000;

        private void InitializeCollection(ContentManager manager)
        {
            RegisterThermalPlatings(manager);
            RegisterEngines(manager);
            RegisterFuelTanks(manager);
            RegisterContainers(manager);
            RegisterThrusters(manager);
            RegisterHulls(manager);
            RegisterDrills(manager);
        }

        private void RegisterThermalPlatings(ContentManager manager)
        {
            var scrapPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Scrap, Thermals: 0, MaxThermals: 100, ThermalDissipation: 2, Name: "Scrap Thermal Plating", Worth: ScrapTierWorth, Weight: 5);
            var copperPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Copper, Thermals: 0, MaxThermals: 160, ThermalDissipation: 3, Name: "Copper Thermal Plating", Worth: CopperTierWorth, Weight: 6);
            var ironPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Iron, Thermals: 0, MaxThermals: 240, ThermalDissipation: 5, Name: "Iron Thermal Plating", Worth: IronTierWorth, Weight: 7);
            var goldPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Gold, Thermals: 0, MaxThermals: 360, ThermalDissipation: 7, Name: "Gold Thermal Plating", Worth: GoldTierWorth, Weight: 8);
            var crystalPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Crystal, Thermals: 0, MaxThermals: 520, ThermalDissipation: 10, Name: "Crystal Thermal Plating", Worth: CrystalTierWorth, Weight: 8);
            var diamondPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Diamond, Thermals: 0, MaxThermals: 760, ThermalDissipation: 14, Name: "Diamond Thermal Plating", Worth: DiamondTierWorth, Weight: 7);
            var radioactivePlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Radioactive, Thermals: 0, MaxThermals: 1100, ThermalDissipation: 19, Name: "Radioactive Thermal Plating", Worth: RadioactiveTierWorth, Weight: 6);
            var rainbowPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Rainbow, Thermals: 0, MaxThermals: 1600, ThermalDissipation: 25, Name: "Rainbow Thermal Plating", Worth: RainbowTierWorth, Weight: 5);
            var mythrilPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Mythril, Thermals: 0, MaxThermals: 2300, ThermalDissipation: 34, Name: "Mythril Thermal Plating", Worth: MythrilTierWorth, Weight: 4);
            var adamantPlating = new ThermalPlating(ID: GameIds.Items.ThermalPlatings.Adamant, Thermals: 0, MaxThermals: 3200, ThermalDissipation: 45, Name: "Adamant Thermal Plating", Worth: AdamantTierWorth, Weight: 4);

            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Scrap, "ScrapThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/ScrapThermalPlating"), scrapPlating, EquipmentType.ThermalPlating, 0);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Copper, "CopperThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/CopperThermalPlating"), copperPlating, EquipmentType.ThermalPlating, 1);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Iron, "IronThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/IronThermalPlating"), ironPlating, EquipmentType.ThermalPlating, 2);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Gold, "GoldThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/GoldThermalPlating"), goldPlating, EquipmentType.ThermalPlating, 3);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Crystal, "CrystalThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/CrystalThermalPlating"), crystalPlating, EquipmentType.ThermalPlating, 4);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Diamond, "DiamondThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/DiamondThermalPlating"), diamondPlating, EquipmentType.ThermalPlating, 5);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Radioactive, "RadioactiveThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/RadioactiveThermalPlating"), radioactivePlating, EquipmentType.ThermalPlating, 6);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Rainbow, "RainbowThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/RainbowThermalPlating"), rainbowPlating, EquipmentType.ThermalPlating, 7);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Mythril, "MythrilThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/MythrilThermalPlating"), mythrilPlating, EquipmentType.ThermalPlating, 8);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Adamant, "AdamantThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/AdamantThermalPlating"), adamantPlating, EquipmentType.ThermalPlating, 9);
        }

        private void RegisterEngines(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Engines.Scrap, "ScrapEngine", LoadSingleTexture(manager, "Player/Engines/ScrapEngine"), new Engine(ID: GameIds.Items.Engines.Scrap, Speed: 2, Name: "Scrap Engine", ActiveFuelConsumption: 1.0f, Worth: ScrapTierWorth, Weight: 5), EquipmentType.Engine, 0);
            AddEquipmentDefinition(GameIds.Items.Engines.Copper, "CopperEngine", LoadSingleTexture(manager, "Player/Engines/CopperEngine"), new Engine(ID: GameIds.Items.Engines.Copper, Speed: 4, Name: "Copper Engine", ActiveFuelConsumption: 0.92f, Worth: CopperTierWorth, Weight: 6), EquipmentType.Engine, 1);
            AddEquipmentDefinition(GameIds.Items.Engines.Iron, "IronEngine", LoadSingleTexture(manager, "Player/Engines/IronEngine"), new Engine(ID: GameIds.Items.Engines.Iron, Speed: 6, Name: "Iron Engine", ActiveFuelConsumption: 0.82f, Worth: IronTierWorth, Weight: 7), EquipmentType.Engine, 2);
            AddEquipmentDefinition(GameIds.Items.Engines.Gold, "GoldEngine", LoadSingleTexture(manager, "Player/Engines/GoldEngine"), new Engine(ID: GameIds.Items.Engines.Gold, Speed: 20, Name: "Gold Engine", ActiveFuelConsumption: 0.7f, Worth: GoldTierWorth, Weight: 8), EquipmentType.Engine, 3);
            AddEquipmentDefinition(GameIds.Items.Engines.Crystal, "CrystalEngine", LoadSingleTexture(manager, "Player/Engines/CrystalEngine"), new Engine(ID: GameIds.Items.Engines.Crystal, Speed: 40, Name: "Crystal Engine", ActiveFuelConsumption: 0.58f, Worth: CrystalTierWorth, Weight: 8), EquipmentType.Engine, 4);
            AddEquipmentDefinition(GameIds.Items.Engines.Diamond, "DiamondEngine", LoadSingleTexture(manager, "Player/Engines/DiamondEngine"), new Engine(ID: GameIds.Items.Engines.Diamond, Speed: 80, Name: "Diamond Engine", ActiveFuelConsumption: 0.46f, Worth: DiamondTierWorth, Weight: 7), EquipmentType.Engine, 5);
            AddEquipmentDefinition(GameIds.Items.Engines.Radioactive, "RadioactiveEngine", LoadSingleTexture(manager, "Player/Engines/RadioactiveEngine"), new Engine(ID: GameIds.Items.Engines.Radioactive, Speed: 160, Name: "Radioactive Engine", ActiveFuelConsumption: 0.34f, Worth: RadioactiveTierWorth, Weight: 6), EquipmentType.Engine, 6);
            AddEquipmentDefinition(GameIds.Items.Engines.Rainbow, "RainbowEngine", LoadSingleTexture(manager, "Player/Engines/RainbowEngine"), new Engine(ID: GameIds.Items.Engines.Rainbow, Speed: 320, Name: "Rainbow Engine", ActiveFuelConsumption: 0.24f, Worth: RainbowTierWorth, Weight: 5), EquipmentType.Engine, 7);
            AddEquipmentDefinition(GameIds.Items.Engines.Mythril, "MythrilEngine", LoadSingleTexture(manager, "Player/Engines/MythrilEngine"), new Engine(ID: GameIds.Items.Engines.Mythril, Speed: 640, Name: "Mythril Engine", ActiveFuelConsumption: 0.18f, Worth: MythrilTierWorth, Weight: 4), EquipmentType.Engine, 8);
            AddEquipmentDefinition(GameIds.Items.Engines.Adamant, "AdamantEngine", LoadSingleTexture(manager, "Player/Engines/AdamantEngine"), new Engine(ID: GameIds.Items.Engines.Adamant, Speed: 1200, Name: "Adamant Engine", ActiveFuelConsumption: 0.12f, Worth: AdamantTierWorth, Weight: 4), EquipmentType.Engine, 9);
        }

        private void RegisterFuelTanks(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Scrap, "ScrapFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/ScrapFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Scrap, Capacity: 200, Fuel: 200, Name: "Scrap Fuel Tank", Worth: ScrapTierWorth, Weight: 5), EquipmentType.FuelTank, 0);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Copper, "CopperFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/CopperFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Copper, Capacity: 350, Fuel: 350, Name: "Copper Fuel Tank", Worth: CopperTierWorth, Weight: 6), EquipmentType.FuelTank, 1);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Iron, "IronFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/IronFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Iron, Capacity: 550, Fuel: 550, Name: "Iron Fuel Tank", Worth: IronTierWorth, Weight: 7), EquipmentType.FuelTank, 2);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Gold, "GoldFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/GoldFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Gold, Capacity: 850, Fuel: 850, Name: "Gold Fuel Tank", Worth: GoldTierWorth, Weight: 8), EquipmentType.FuelTank, 3);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Crystal, "CrystalFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/CrystalFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Crystal, Capacity: 1300, Fuel: 1300, Name: "Crystal Fuel Tank", Worth: CrystalTierWorth, Weight: 8), EquipmentType.FuelTank, 4);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Diamond, "DiamondFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/DiamondFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Diamond, Capacity: 1900, Fuel: 1900, Name: "Diamond Fuel Tank", Worth: DiamondTierWorth, Weight: 7), EquipmentType.FuelTank, 5);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Radioactive, "RadioactiveFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/RadioactiveFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Radioactive, Capacity: 2800, Fuel: 2800, Name: "Radioactive Fuel Tank", Worth: RadioactiveTierWorth, Weight: 6), EquipmentType.FuelTank, 6);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Rainbow, "RainbowFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/RainbowFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Rainbow, Capacity: 4000, Fuel: 4000, Name: "Rainbow Fuel Tank", Worth: RainbowTierWorth, Weight: 5), EquipmentType.FuelTank, 7);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Mythril, "MythrilFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/MythrilFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Mythril, Capacity: 6000, Fuel: 6000, Name: "Mythril Fuel Tank", Worth: MythrilTierWorth, Weight: 4), EquipmentType.FuelTank, 8);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Adamant, "AdamantFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/AdamantFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Adamant, Capacity: 10000, Fuel: 10000, Name: "Adamant Fuel Tank", Worth: AdamantTierWorth, Weight: 4), EquipmentType.FuelTank, 9);
        }

        private void RegisterContainers(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Containers.Scrap, "ScrapContainer", LoadSingleTexture(manager, "Player/Containers/ScrapContainer"), new Inventory(ID: GameIds.Items.Containers.Scrap, Items: CreateInventoryGrid(1, 1), SizeLimit: 64, Name: "Scrap Container", Worth: ScrapTierWorth, Weight: 5, MaxStackSize: 64), EquipmentType.Inventory, 0);
            AddEquipmentDefinition(GameIds.Items.Containers.Copper, "CopperContainer", LoadSingleTexture(manager, "Player/Containers/CopperContainer"), new Inventory(ID: GameIds.Items.Containers.Copper, Items: CreateInventoryGrid(1, 3), SizeLimit: 192, Name: "Copper Container", Worth: CopperTierWorth, Weight: 6, MaxStackSize: 64), EquipmentType.Inventory, 1);
            AddEquipmentDefinition(GameIds.Items.Containers.Iron, "IronContainer", LoadSingleTexture(manager, "Player/Containers/IronContainer"), new Inventory(ID: GameIds.Items.Containers.Iron, Items: CreateInventoryGrid(2, 3), SizeLimit: 384, Name: "Iron Container", Worth: IronTierWorth, Weight: 7, MaxStackSize: 64), EquipmentType.Inventory, 2);
            AddEquipmentDefinition(GameIds.Items.Containers.Gold, "GoldContainer", LoadSingleTexture(manager, "Player/Containers/GoldContainer"), new Inventory(ID: GameIds.Items.Containers.Gold, Items: CreateInventoryGrid(3, 3), SizeLimit: 576, Name: "Gold Container", Worth: GoldTierWorth, Weight: 8, MaxStackSize: 64), EquipmentType.Inventory, 3);
            AddEquipmentDefinition(GameIds.Items.Containers.Crystal, "CrystalContainer", LoadSingleTexture(manager, "Player/Containers/CrystalContainer"), new Inventory(ID: GameIds.Items.Containers.Crystal, Items: CreateInventoryGrid(3, 4), SizeLimit: 768, Name: "Crystal Container", Worth: CrystalTierWorth, Weight: 8, MaxStackSize: 64), EquipmentType.Inventory, 4);
            AddEquipmentDefinition(GameIds.Items.Containers.Diamond, "DiamondContainer", LoadSingleTexture(manager, "Player/Containers/DiamondContainer"), new Inventory(ID: GameIds.Items.Containers.Diamond, Items: CreateInventoryGrid(4, 4), SizeLimit: 1024, Name: "Diamond Container", Worth: DiamondTierWorth, Weight: 7, MaxStackSize: 64), EquipmentType.Inventory, 5);
            AddEquipmentDefinition(GameIds.Items.Containers.Radioactive, "RadioactiveContainer", LoadSingleTexture(manager, "Player/Containers/RadioactiveContainer"), new Inventory(ID: GameIds.Items.Containers.Radioactive, Items: CreateInventoryGrid(4, 5), SizeLimit: 1280, Name: "Radioactive Container", Worth: RadioactiveTierWorth, Weight: 6, MaxStackSize: 64), EquipmentType.Inventory, 6);
            AddEquipmentDefinition(GameIds.Items.Containers.Rainbow, "RainbowContainer", LoadSingleTexture(manager, "Player/Containers/RainbowContainer"), new Inventory(ID: GameIds.Items.Containers.Rainbow, Items: CreateInventoryGrid(5, 5), SizeLimit: 1600, Name: "Rainbow Container", Worth: RainbowTierWorth, Weight: 5, MaxStackSize: 64), EquipmentType.Inventory, 7);
            AddEquipmentDefinition(GameIds.Items.Containers.Mythril, "MythrilContainer", LoadSingleTexture(manager, "Player/Containers/MythrilContainer"), new Inventory(ID: GameIds.Items.Containers.Mythril, Items: CreateInventoryGrid(8, 6), SizeLimit: 6144, Name: "Mythril Container", Worth: MythrilTierWorth, Weight: 4, MaxStackSize: 128), EquipmentType.Inventory, 8);
            AddEquipmentDefinition(GameIds.Items.Containers.Adamant, "AdamantContainer", LoadSingleTexture(manager, "Player/Containers/AdamantContainer"), new Inventory(ID: GameIds.Items.Containers.Adamant, Items: CreateInventoryGrid(18, 8), SizeLimit: 73728, Name: "Adamant Container", Worth: AdamantTierWorth, Weight: 4, MaxStackSize: 512), EquipmentType.Inventory, 9);
        }

        private void RegisterThrusters(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Thrusters.Scrap, "ScrapThruster", LoadSingleTexture(manager, "Player/Thrusters/ScrapThruster"), new Thruster(ID: GameIds.Items.Thrusters.Scrap, Speed: 240f, Acceleration: 30f, Power: 50, Name: "Scrap Thruster", ActiveFuelConsumption: 2.0f, Weight: 5, Worth: ScrapTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 0);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Copper, "CopperThruster", LoadSingleTexture(manager, "Player/Thrusters/CopperThruster"), new Thruster(ID: GameIds.Items.Thrusters.Copper, Speed: 480f, Acceleration: 60f, Power: 75, Name: "Copper Thruster", ActiveFuelConsumption: 2.2f, Weight: 6, Worth: CopperTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 1);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Iron, "IronThruster", LoadSingleTexture(manager, "Player/Thrusters/IronThruster"), new Thruster(ID: GameIds.Items.Thrusters.Iron, Speed: 960f, Acceleration: 120f, Power: 110, Name: "Iron Thruster", ActiveFuelConsumption: 2.5f, Weight: 7, Worth: IronTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 2);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Gold, "GoldThruster", LoadSingleTexture(manager, "Player/Thrusters/GoldThruster"), new Thruster(ID: GameIds.Items.Thrusters.Gold, Speed: 1920f, Acceleration: 240f, Power: 160, Name: "Gold Thruster", ActiveFuelConsumption: 2.9f, Weight: 8, Worth: GoldTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 3);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Crystal, "CrystalThruster", LoadSingleTexture(manager, "Player/Thrusters/CrystalThruster"), new Thruster(ID: GameIds.Items.Thrusters.Crystal, Speed: 3840f, Acceleration: 480f, Power: 230, Name: "Crystal Thruster", ActiveFuelConsumption: 3.4f, Weight: 8, Worth: CrystalTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 4);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Diamond, "DiamondThruster", LoadSingleTexture(manager, "Player/Thrusters/DiamondThruster"), new Thruster(ID: GameIds.Items.Thrusters.Diamond, Speed: 7680f, Acceleration: 960f, Power: 320, Name: "Diamond Thruster", ActiveFuelConsumption: 4.0f, Weight: 7, Worth: DiamondTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 5);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Radioactive, "RadioactiveThruster", LoadSingleTexture(manager, "Player/Thrusters/RadioactiveThruster"), new Thruster(ID: GameIds.Items.Thrusters.Radioactive, Speed: 15360f, Acceleration: 1920f, Power: 440, Name: "Radioactive Thruster", ActiveFuelConsumption: 4.7f, Weight: 6, Worth: RadioactiveTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 6);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Rainbow, "RainbowThruster", LoadSingleTexture(manager, "Player/Thrusters/RainbowThruster"), new Thruster(ID: GameIds.Items.Thrusters.Rainbow, Speed: 30720f, Acceleration: 3840f, Power: 1500, Name: "Rainbow Thruster", ActiveFuelConsumption: 5.5f, Weight: 5, Worth: RainbowTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 7);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Mythril, "MythrilThruster", LoadSingleTexture(manager, "Player/Thrusters/MythrilThruster"), new Thruster(ID: GameIds.Items.Thrusters.Mythril, Speed: 61440f, Acceleration: 7680f, Power: 2200, Name: "Mythril Thruster", ActiveFuelConsumption: 6.2f, Weight: 4, Worth: MythrilTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 8);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Adamant, "AdamantThruster", LoadSingleTexture(manager, "Player/Thrusters/AdamantThruster"), new Thruster(ID: GameIds.Items.Thrusters.Adamant, Speed: 122880f, Acceleration: 15360f, Power: 3200, Name: "Adamant Thruster", ActiveFuelConsumption: 7.0f, Weight: 4, Worth: AdamantTierWorth, MinimumVelocity: 10f), EquipmentType.Thruster, 9);
        }

        private void RegisterHulls(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Hulls.Scrap, "ScrapHull", LoadHullTextures(manager, "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShipDown"), new Hull(ID: GameIds.Items.Hulls.Scrap, Durability: 5, Health: 50, Name: "Scrap Hull", Worth: ScrapTierWorth, Weight: 5), EquipmentType.Hull, 0);
            AddEquipmentDefinition(GameIds.Items.Hulls.Copper, "CopperHull", LoadHullTextures(manager, "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShipDown"), new Hull(ID: GameIds.Items.Hulls.Copper, Durability: 10, Health: 130, Name: "Copper Hull", Worth: CopperTierWorth, Weight: 8), EquipmentType.Hull, 1);
            AddEquipmentDefinition(GameIds.Items.Hulls.Iron, "IronHull", LoadHullTextures(manager, "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShipDown"), new Hull(ID: GameIds.Items.Hulls.Iron, Durability: 18, Health: 260, Name: "Iron Hull", Worth: IronTierWorth, Weight: 10), EquipmentType.Hull, 2);
            AddEquipmentDefinition(GameIds.Items.Hulls.Gold, "GoldHull", LoadHullTextures(manager, "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShipDown"), new Hull(ID: GameIds.Items.Hulls.Gold, Durability: 30, Health: 480, Name: "Gold Hull", Worth: GoldTierWorth, Weight: 12), EquipmentType.Hull, 3);
            AddEquipmentDefinition(GameIds.Items.Hulls.Crystal, "CrystalHull", LoadHullTextures(manager, "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShipDown"), new Hull(ID: GameIds.Items.Hulls.Crystal, Durability: 48, Health: 820, Name: "Crystal Hull", Worth: CrystalTierWorth, Weight: 12), EquipmentType.Hull, 4);
            AddEquipmentDefinition(GameIds.Items.Hulls.Diamond, "DiamondHull", LoadHullTextures(manager, "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShipDown"), new Hull(ID: GameIds.Items.Hulls.Diamond, Durability: 72, Health: 1300, Name: "Diamond Hull", Worth: DiamondTierWorth, Weight: 11), EquipmentType.Hull, 5);
            AddEquipmentDefinition(GameIds.Items.Hulls.Radioactive, "RadioactiveHull", LoadHullTextures(manager, "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShipDown"), new Hull(ID: GameIds.Items.Hulls.Radioactive, Durability: 104, Health: 2000, Name: "Radioactive Hull", Worth: RadioactiveTierWorth, Weight: 10), EquipmentType.Hull, 6);
            AddEquipmentDefinition(GameIds.Items.Hulls.Rainbow, "RainbowHull", LoadHullTextures(manager, "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: GameIds.Items.Hulls.Rainbow, Durability: 150, Health: 3000, Name: "Rainbow Hull", Worth: RainbowTierWorth, Weight: 9), EquipmentType.Hull, 7);
            AddEquipmentDefinition(GameIds.Items.Hulls.Mythril, "MythrilHull", LoadHullTextures(manager, "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShipDown"), new Hull(ID: GameIds.Items.Hulls.Mythril, Durability: 220, Health: 4500, Name: "Mythril Hull", Worth: MythrilTierWorth, Weight: 8), EquipmentType.Hull, 8);
            AddEquipmentDefinition(GameIds.Items.Hulls.Adamant, "AdamantHull", LoadHullTextures(manager, "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShipDown"), new Hull(ID: GameIds.Items.Hulls.Adamant, Durability: 320, Health: 7000, Name: "Adamant Hull", Worth: AdamantTierWorth, Weight: 8), EquipmentType.Hull, 9);
        }

        private void RegisterDrills(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Drills.Scrap, "ScrapDrill", LoadDrillTextures(manager, "Player/Drills/DirtDrill/DirtDrill"), new Drill(ID: GameIds.Items.Drills.Scrap, Hardness: 5f, Damage: 0.1f, Name: "Scrap Drill", Worth: ScrapTierWorth, Weight: 5, MiningAreaSize: 1), EquipmentType.Drill, 0);
            AddEquipmentDefinition(GameIds.Items.Drills.Copper, "CopperDrill", LoadDrillTextures(manager, "Player/Drills/CopperDrill/CopperDrill"), new Drill(ID: GameIds.Items.Drills.Copper, Hardness: 15f, Damage: 0.25f, Name: "Copper Drill", Worth: CopperTierWorth, Weight: 6, MiningAreaSize: 1), EquipmentType.Drill, 1);
            AddEquipmentDefinition(GameIds.Items.Drills.Iron, "IronDrill", LoadDrillTextures(manager, "Player/Drills/IronDrill/IronDrill"), new Drill(ID: GameIds.Items.Drills.Iron, Hardness: 35f, Damage: 1.2f, Name: "Iron Drill", Worth: IronTierWorth, Weight: 7, MiningAreaSize: 1), EquipmentType.Drill, 2);
            AddEquipmentDefinition(GameIds.Items.Drills.Gold, "GoldDrill", LoadDrillTextures(manager, "Player/Drills/GoldDrill/GoldDrill"), new Drill(ID: GameIds.Items.Drills.Gold, Hardness: 80f, Damage: 3.0f, Name: "Gold Drill", Worth: GoldTierWorth, Weight: 8, MiningAreaSize: 1), EquipmentType.Drill, 3);
            AddEquipmentDefinition(GameIds.Items.Drills.Crystal, "CrystalDrill", LoadDrillTextures(manager, "Player/Drills/CrystalDrill/CrystalDrill"), new Drill(ID: GameIds.Items.Drills.Crystal, Hardness: 180f, Damage: 6.5f, Name: "Crystal Drill", Worth: CrystalTierWorth, Weight: 8, MiningAreaSize: 1), EquipmentType.Drill, 4);
            AddEquipmentDefinition(GameIds.Items.Drills.Diamond, "DiamondDrill", LoadDrillTextures(manager, "Player/Drills/DiamondDrill/DiamondDrill"), new Drill(ID: GameIds.Items.Drills.Diamond, Hardness: 400f, Damage: 15.0f, Name: "Diamond Drill", Worth: DiamondTierWorth, Weight: 7, MiningAreaSize: 1), EquipmentType.Drill, 5);
            AddEquipmentDefinition(GameIds.Items.Drills.Radioactive, "RadioactiveDrill", LoadDrillTextures(manager, "Player/Drills/RadioactiveDrill/RadioactiveDrill"), new Drill(ID: GameIds.Items.Drills.Radioactive, Hardness: 900f, Damage: 80.0f, Name: "Radioactive Drill", Worth: RadioactiveTierWorth, Weight: 6, MiningAreaSize: 1), EquipmentType.Drill, 6);
            AddEquipmentDefinition(GameIds.Items.Drills.Rainbow, "RainbowDrill", LoadDrillTextures(manager, "Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: GameIds.Items.Drills.Rainbow, Hardness: 2000f, Damage: 400.0f, Name: "Rainbow Drill", Worth: RainbowTierWorth, Weight: 5, MiningAreaSize: 1), EquipmentType.Drill, 7);
            AddEquipmentDefinition(GameIds.Items.Drills.Mythril, "MythrilDrill", LoadDrillTextures(manager, "Player/Drills/MythrilDrill/MythrilDrill"), new Drill(ID: GameIds.Items.Drills.Mythril, Hardness: 4000f, Damage: 1500.0f, Name: "Mythril Drill", Worth: MythrilTierWorth, Weight: 4, MiningAreaSize: 3), EquipmentType.Drill, 8);
            AddEquipmentDefinition(GameIds.Items.Drills.Adamant, "AdamantDrill", LoadDrillTextures(manager, "Player/Drills/AdamantDrill/AdamantDrill"), new Drill(ID: GameIds.Items.Drills.Adamant, Hardness: 10000f, Damage: 8000.0f, Name: "Adamant Drill", Worth: AdamantTierWorth, Weight: 4, MiningAreaSize: 9), EquipmentType.Drill, 9);
        }

        private void AddDefinition(int id, string name, Dictionary<PlayerOrientation, Texture2D> textures, AType definition)
        {
            Add(id, new GameItemDefinition(name, textures, definition));
        }

        private void AddEquipmentDefinition(int id, string name, Dictionary<PlayerOrientation, Texture2D> textures, AType definition, EquipmentType equipmentType, int tier)
        {
            Add(id, new GameItemDefinition(name, textures, definition, buyable: true, type: GameItemType.Equipment, equipmentType: equipmentType, tier: tier));
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

