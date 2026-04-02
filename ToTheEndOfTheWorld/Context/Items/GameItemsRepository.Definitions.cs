using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.Items;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Context.Items
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
            RegisterConsumeables(manager);
            RegisterGadgets(manager);
        }

        private void RegisterThermalPlatings(ContentManager manager)
        {
            ThermalPlating scrapPlating = new(ID: GameIds.Items.ThermalPlatings.Scrap, Thermals: 0, MaxThermals: 100, ThermalDissipation: 2, Name: "Scrap Thermal Plating", Worth: ScrapTierWorth, Weight: 5);
            ThermalPlating copperPlating = new(ID: GameIds.Items.ThermalPlatings.Copper, Thermals: 0, MaxThermals: 160, ThermalDissipation: 3, Name: "Copper Thermal Plating", Worth: CopperTierWorth, Weight: 6);
            ThermalPlating ironPlating = new(ID: GameIds.Items.ThermalPlatings.Iron, Thermals: 0, MaxThermals: 240, ThermalDissipation: 5, Name: "Iron Thermal Plating", Worth: IronTierWorth, Weight: 7);
            ThermalPlating goldPlating = new(ID: GameIds.Items.ThermalPlatings.Gold, Thermals: 0, MaxThermals: 360, ThermalDissipation: 10, Name: "Gold Thermal Plating", Worth: GoldTierWorth, Weight: 8);
            ThermalPlating crystalPlating = new(ID: GameIds.Items.ThermalPlatings.Crystal, Thermals: 0, MaxThermals: 520, ThermalDissipation: 15, Name: "Crystal Thermal Plating", Worth: CrystalTierWorth, Weight: 8);
            ThermalPlating diamondPlating = new(ID: GameIds.Items.ThermalPlatings.Diamond, Thermals: 0, MaxThermals: 760, ThermalDissipation: 25, Name: "Diamond Thermal Plating", Worth: DiamondTierWorth, Weight: 7);
            ThermalPlating radioactivePlating = new(ID: GameIds.Items.ThermalPlatings.Radioactive, Thermals: 0, MaxThermals: 1100, ThermalDissipation: 35, Name: "Radioactive Thermal Plating", Worth: RadioactiveTierWorth, Weight: 6);
            ThermalPlating rainbowPlating = new(ID: GameIds.Items.ThermalPlatings.Rainbow, Thermals: 0, MaxThermals: 1600, ThermalDissipation: 50, Name: "Rainbow Thermal Plating", Worth: RainbowTierWorth, Weight: 5);
            ThermalPlating mythrilPlating = new(ID: GameIds.Items.ThermalPlatings.Mythril, Thermals: 0, MaxThermals: 2300, ThermalDissipation: 100, Name: "Mythril Thermal Plating", Worth: MythrilTierWorth, Weight: 4);
            ThermalPlating adamantPlating = new(ID: GameIds.Items.ThermalPlatings.Adamant, Thermals: 0, MaxThermals: 3200, ThermalDissipation: 200, Name: "Adamant Thermal Plating", Worth: AdamantTierWorth, Weight: 4);

            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Scrap, "ScrapThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/ScrapThermalPlating"), scrapPlating, EEquipmentType.ThermalPlating, 0);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Copper, "CopperThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/CopperThermalPlating"), copperPlating, EEquipmentType.ThermalPlating, 1);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Iron, "IronThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/IronThermalPlating"), ironPlating, EEquipmentType.ThermalPlating, 2);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Gold, "GoldThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/GoldThermalPlating"), goldPlating, EEquipmentType.ThermalPlating, 3);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Crystal, "CrystalThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/CrystalThermalPlating"), crystalPlating, EEquipmentType.ThermalPlating, 4);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Diamond, "DiamondThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/DiamondThermalPlating"), diamondPlating, EEquipmentType.ThermalPlating, 5);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Radioactive, "RadioactiveThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/RadioactiveThermalPlating"), radioactivePlating, EEquipmentType.ThermalPlating, 6);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Rainbow, "RainbowThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/RainbowThermalPlating"), rainbowPlating, EEquipmentType.ThermalPlating, 7);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Mythril, "MythrilThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/MythrilThermalPlating"), mythrilPlating, EEquipmentType.ThermalPlating, 8);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Adamant, "AdamantThermalPlating", LoadSingleTexture(manager, "Player/ThermalPlatings/AdamantThermalPlating"), adamantPlating, EEquipmentType.ThermalPlating, 9);
        }

        private void RegisterEngines(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Engines.Scrap, "ScrapEngine", LoadSingleTexture(manager, "Player/Engines/ScrapEngine"), new Engine(ID: GameIds.Items.Engines.Scrap, Speed: 400f, Acceleration: 100f, Name: "Scrap Engine", StandbyFuelConsumption: 0.001f, ActiveFuelConsumption: 0.05f, Worth: ScrapTierWorth, Weight: 5), EEquipmentType.Engine, 0);
            AddEquipmentDefinition(GameIds.Items.Engines.Copper, "CopperEngine", LoadSingleTexture(manager, "Player/Engines/CopperEngine"), new Engine(ID: GameIds.Items.Engines.Copper, Speed: 600f, Acceleration: 150f, Name: "Copper Engine", StandbyFuelConsumption: 0.02f, ActiveFuelConsumption: 0.2f, Worth: CopperTierWorth, Weight: 6), EEquipmentType.Engine, 1);
            AddEquipmentDefinition(GameIds.Items.Engines.Iron, "IronEngine", LoadSingleTexture(manager, "Player/Engines/IronEngine"), new Engine(ID: GameIds.Items.Engines.Iron, Speed: 900f, Acceleration: 250f, Name: "Iron Engine", StandbyFuelConsumption: 0.06f, ActiveFuelConsumption: 0.4f, Worth: IronTierWorth, Weight: 7), EEquipmentType.Engine, 2);
            AddEquipmentDefinition(GameIds.Items.Engines.Gold, "GoldEngine", LoadSingleTexture(manager, "Player/Engines/GoldEngine"), new Engine(ID: GameIds.Items.Engines.Gold, Speed: 1450f, Acceleration: 400f, Name: "Gold Engine", StandbyFuelConsumption: 0.07f, ActiveFuelConsumption: 0.6f, Worth: GoldTierWorth, Weight: 8), EEquipmentType.Engine, 3);
            AddEquipmentDefinition(GameIds.Items.Engines.Crystal, "CrystalEngine", LoadSingleTexture(manager, "Player/Engines/CrystalEngine"), new Engine(ID: GameIds.Items.Engines.Crystal, Speed: 2600f, Acceleration: 600f, Name: "Crystal Engine", StandbyFuelConsumption: 0.058f, ActiveFuelConsumption: 1.0f, Worth: CrystalTierWorth, Weight: 8), EEquipmentType.Engine, 4);
            AddEquipmentDefinition(GameIds.Items.Engines.Diamond, "DiamondEngine", LoadSingleTexture(manager, "Player/Engines/DiamondEngine"), new Engine(ID: GameIds.Items.Engines.Diamond, Speed: 4800f, Acceleration: 1200f, Name: "Diamond Engine", StandbyFuelConsumption: 0.046f, ActiveFuelConsumption: 2.0f, Worth: DiamondTierWorth, Weight: 7), EEquipmentType.Engine, 5);
            AddEquipmentDefinition(GameIds.Items.Engines.Radioactive, "RadioactiveEngine", LoadSingleTexture(manager, "Player/Engines/RadioactiveEngine"), new Engine(ID: GameIds.Items.Engines.Radioactive, Speed: 7000f, Acceleration: 2500f, Name: "Radioactive Engine", StandbyFuelConsumption: 0.034f, ActiveFuelConsumption: 4f, Worth: RadioactiveTierWorth, Weight: 6), EEquipmentType.Engine, 6);
            AddEquipmentDefinition(GameIds.Items.Engines.Rainbow, "RainbowEngine", LoadSingleTexture(manager, "Player/Engines/RainbowEngine"), new Engine(ID: GameIds.Items.Engines.Rainbow, Speed: 10000f, Acceleration: 5000f, Name: "Rainbow Engine", StandbyFuelConsumption: 0.024f, ActiveFuelConsumption: 6f, Worth: RainbowTierWorth, Weight: 5), EEquipmentType.Engine, 7);
            AddEquipmentDefinition(GameIds.Items.Engines.Mythril, "MythrilEngine", LoadSingleTexture(manager, "Player/Engines/MythrilEngine"), new Engine(ID: GameIds.Items.Engines.Mythril, Speed: 24000f, Acceleration: 7500f, Name: "Mythril Engine", StandbyFuelConsumption: 0.018f, ActiveFuelConsumption: 14f, Worth: MythrilTierWorth, Weight: 4), EEquipmentType.Engine, 8);
            AddEquipmentDefinition(GameIds.Items.Engines.Adamant, "AdamantEngine", LoadSingleTexture(manager, "Player/Engines/AdamantEngine"), new Engine(ID: GameIds.Items.Engines.Adamant, Speed: 36000f, Acceleration: 10000f, Name: "Adamant Engine", StandbyFuelConsumption: 0.012f, ActiveFuelConsumption: 20f, Worth: AdamantTierWorth, Weight: 4), EEquipmentType.Engine, 9);
        }

        private void RegisterFuelTanks(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Scrap, "ScrapFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/ScrapFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Scrap, Capacity: 10, Fuel: 10, Name: "Scrap Fuel Tank", Worth: ScrapTierWorth, Weight: 5), EEquipmentType.FuelTank, 0);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Copper, "CopperFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/CopperFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Copper, Capacity: 25, Fuel: 25, Name: "Copper Fuel Tank", Worth: CopperTierWorth, Weight: 6), EEquipmentType.FuelTank, 1);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Iron, "IronFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/IronFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Iron, Capacity: 100, Fuel: 100, Name: "Iron Fuel Tank", Worth: IronTierWorth, Weight: 7), EEquipmentType.FuelTank, 2);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Gold, "GoldFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/GoldFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Gold, Capacity: 250, Fuel: 250, Name: "Gold Fuel Tank", Worth: GoldTierWorth, Weight: 8), EEquipmentType.FuelTank, 3);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Crystal, "CrystalFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/CrystalFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Crystal, Capacity: 1000, Fuel: 1000, Name: "Crystal Fuel Tank", Worth: CrystalTierWorth, Weight: 8), EEquipmentType.FuelTank, 4);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Diamond, "DiamondFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/DiamondFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Diamond, Capacity: 2000, Fuel: 2000, Name: "Diamond Fuel Tank", Worth: DiamondTierWorth, Weight: 7), EEquipmentType.FuelTank, 5);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Radioactive, "RadioactiveFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/RadioactiveFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Radioactive, Capacity: 2800, Fuel: 2800, Name: "Radioactive Fuel Tank", Worth: RadioactiveTierWorth, Weight: 6), EEquipmentType.FuelTank, 6);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Rainbow, "RainbowFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/RainbowFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Rainbow, Capacity: 4000, Fuel: 4000, Name: "Rainbow Fuel Tank", Worth: RainbowTierWorth, Weight: 5), EEquipmentType.FuelTank, 7);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Mythril, "MythrilFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/MythrilFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Mythril, Capacity: 6000, Fuel: 6000, Name: "Mythril Fuel Tank", Worth: MythrilTierWorth, Weight: 4), EEquipmentType.FuelTank, 8);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Adamant, "AdamantFuelTank", LoadSingleTexture(manager, "Player/FuelTanks/AdamantFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Adamant, Capacity: 10000, Fuel: 10000, Name: "Adamant Fuel Tank", Worth: AdamantTierWorth, Weight: 4), EEquipmentType.FuelTank, 9);
        }

        private void RegisterContainers(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Containers.Scrap, "ScrapContainer", LoadSingleTexture(manager, "Player/Containers/ScrapContainer"), new Inventory(ID: GameIds.Items.Containers.Scrap, Items: CreateInventoryGrid(2, 1), SizeLimit: 16, Name: "Scrap Container", Worth: ScrapTierWorth, Weight: 5, MaxStackSize: 8), EEquipmentType.Inventory, 0);
            AddEquipmentDefinition(GameIds.Items.Containers.Copper, "CopperContainer", LoadSingleTexture(manager, "Player/Containers/CopperContainer"), new Inventory(ID: GameIds.Items.Containers.Copper, Items: CreateInventoryGrid(1, 3), SizeLimit: 64, Name: "Copper Container", Worth: CopperTierWorth, Weight: 6, MaxStackSize: 32), EEquipmentType.Inventory, 1);
            AddEquipmentDefinition(GameIds.Items.Containers.Iron, "IronContainer", LoadSingleTexture(manager, "Player/Containers/IronContainer"), new Inventory(ID: GameIds.Items.Containers.Iron, Items: CreateInventoryGrid(2, 3), SizeLimit: 384, Name: "Iron Container", Worth: IronTierWorth, Weight: 7, MaxStackSize: 64), EEquipmentType.Inventory, 2);
            AddEquipmentDefinition(GameIds.Items.Containers.Gold, "GoldContainer", LoadSingleTexture(manager, "Player/Containers/GoldContainer"), new Inventory(ID: GameIds.Items.Containers.Gold, Items: CreateInventoryGrid(3, 3), SizeLimit: 576, Name: "Gold Container", Worth: GoldTierWorth, Weight: 8, MaxStackSize: 64), EEquipmentType.Inventory, 3);
            AddEquipmentDefinition(GameIds.Items.Containers.Crystal, "CrystalContainer", LoadSingleTexture(manager, "Player/Containers/CrystalContainer"), new Inventory(ID: GameIds.Items.Containers.Crystal, Items: CreateInventoryGrid(3, 4), SizeLimit: 768, Name: "Crystal Container", Worth: CrystalTierWorth, Weight: 8, MaxStackSize: 64), EEquipmentType.Inventory, 4);
            AddEquipmentDefinition(GameIds.Items.Containers.Diamond, "DiamondContainer", LoadSingleTexture(manager, "Player/Containers/DiamondContainer"), new Inventory(ID: GameIds.Items.Containers.Diamond, Items: CreateInventoryGrid(4, 4), SizeLimit: 1024, Name: "Diamond Container", Worth: DiamondTierWorth, Weight: 7, MaxStackSize: 64), EEquipmentType.Inventory, 5);
            AddEquipmentDefinition(GameIds.Items.Containers.Radioactive, "RadioactiveContainer", LoadSingleTexture(manager, "Player/Containers/RadioactiveContainer"), new Inventory(ID: GameIds.Items.Containers.Radioactive, Items: CreateInventoryGrid(4, 5), SizeLimit: 1280, Name: "Radioactive Container", Worth: RadioactiveTierWorth, Weight: 6, MaxStackSize: 64), EEquipmentType.Inventory, 6);
            AddEquipmentDefinition(GameIds.Items.Containers.Rainbow, "RainbowContainer", LoadSingleTexture(manager, "Player/Containers/RainbowContainer"), new Inventory(ID: GameIds.Items.Containers.Rainbow, Items: CreateInventoryGrid(5, 5), SizeLimit: 1600, Name: "Rainbow Container", Worth: RainbowTierWorth, Weight: 5, MaxStackSize: 64), EEquipmentType.Inventory, 7);
            AddEquipmentDefinition(GameIds.Items.Containers.Mythril, "MythrilContainer", LoadSingleTexture(manager, "Player/Containers/MythrilContainer"), new Inventory(ID: GameIds.Items.Containers.Mythril, Items: CreateInventoryGrid(8, 6), SizeLimit: 6144, Name: "Mythril Container", Worth: MythrilTierWorth, Weight: 4, MaxStackSize: 128), EEquipmentType.Inventory, 8);
            AddEquipmentDefinition(GameIds.Items.Containers.Adamant, "AdamantContainer", LoadSingleTexture(manager, "Player/Containers/AdamantContainer"), new Inventory(ID: GameIds.Items.Containers.Adamant, Items: CreateInventoryGrid(18, 8), SizeLimit: 73728, Name: "Adamant Container", Worth: AdamantTierWorth, Weight: 4, MaxStackSize: 512), EEquipmentType.Inventory, 9);
        }

        private void RegisterThrusters(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Thrusters.Scrap, "ScrapThruster", LoadSingleTexture(manager, "Player/Thrusters/ScrapThruster"), new Thruster(ID: GameIds.Items.Thrusters.Scrap, Speed: 500f, Acceleration: 250f, Power: 50, Name: "Scrap Thruster", ActiveFuelConsumption: 0.05f, ActiveHeatGeneration: 0.10f, Weight: 5, Worth: ScrapTierWorth), EEquipmentType.Thruster, 0);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Copper, "CopperThruster", LoadSingleTexture(manager, "Player/Thrusters/CopperThruster"), new Thruster(ID: GameIds.Items.Thrusters.Copper, Speed: 1100f, Acceleration: 350f, Power: 75, Name: "Copper Thruster", ActiveFuelConsumption: 0.1f, ActiveHeatGeneration: 0.18f, Weight: 6, Worth: CopperTierWorth), EEquipmentType.Thruster, 1);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Iron, "IronThruster", LoadSingleTexture(manager, "Player/Thrusters/IronThruster"), new Thruster(ID: GameIds.Items.Thrusters.Iron, Speed: 2000f, Acceleration: 500f, Power: 100, Name: "Iron Thruster", ActiveFuelConsumption: 0.4f, ActiveHeatGeneration: 0.28f, Weight: 7, Worth: IronTierWorth), EEquipmentType.Thruster, 2);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Gold, "GoldThruster", LoadSingleTexture(manager, "Player/Thrusters/GoldThruster"), new Thruster(ID: GameIds.Items.Thrusters.Gold, Speed: 4000f, Acceleration: 650f, Power: 200, Name: "Gold Thruster", ActiveFuelConsumption: 1.2f, ActiveHeatGeneration: 0.40f, Weight: 8, Worth: GoldTierWorth), EEquipmentType.Thruster, 3);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Crystal, "CrystalThruster", LoadSingleTexture(manager, "Player/Thrusters/CrystalThruster"), new Thruster(ID: GameIds.Items.Thrusters.Crystal, Speed: 8000f, Acceleration: 800f, Power: 400, Name: "Crystal Thruster", ActiveFuelConsumption: 2.4f, ActiveHeatGeneration: 0.55f, Weight: 8, Worth: CrystalTierWorth), EEquipmentType.Thruster, 4);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Diamond, "DiamondThruster", LoadSingleTexture(manager, "Player/Thrusters/DiamondThruster"), new Thruster(ID: GameIds.Items.Thrusters.Diamond, Speed: 24000f, Acceleration: 1000f, Power: 600, Name: "Diamond Thruster", ActiveFuelConsumption: 3.6f, ActiveHeatGeneration: 1f, Weight: 7, Worth: DiamondTierWorth), EEquipmentType.Thruster, 5);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Radioactive, "RadioactiveThruster", LoadSingleTexture(manager, "Player/Thrusters/RadioactiveThruster"), new Thruster(ID: GameIds.Items.Thrusters.Radioactive, Speed: 15360f, Acceleration: 1200f, Power: 1250, Name: "Radioactive Thruster", ActiveFuelConsumption: 4.7f, ActiveHeatGeneration: 2.00f, Weight: 6, Worth: RadioactiveTierWorth), EEquipmentType.Thruster, 6);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Rainbow, "RainbowThruster", LoadSingleTexture(manager, "Player/Thrusters/RainbowThruster"), new Thruster(ID: GameIds.Items.Thrusters.Rainbow, Speed: 48000f, Acceleration: 1750f, Power: 2500, Name: "Rainbow Thruster", ActiveFuelConsumption: 5.5f, ActiveHeatGeneration: 3f, Weight: 5, Worth: RainbowTierWorth), EEquipmentType.Thruster, 7);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Mythril, "MythrilThruster", LoadSingleTexture(manager, "Player/Thrusters/MythrilThruster"), new Thruster(ID: GameIds.Items.Thrusters.Mythril, Speed: 96000f, Acceleration: 2500f, Power: 7500, Name: "Mythril Thruster", ActiveFuelConsumption: 6.2f, ActiveHeatGeneration: 4f, Weight: 4, Worth: MythrilTierWorth), EEquipmentType.Thruster, 8);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Adamant, "AdamantThruster", LoadSingleTexture(manager, "Player/Thrusters/AdamantThruster"), new Thruster(ID: GameIds.Items.Thrusters.Adamant, Speed: 192000f, Acceleration: 4000f, Power: 15000, Name: "Adamant Thruster", ActiveFuelConsumption: 7.0f, ActiveHeatGeneration: 5f, Weight: 4, Worth: AdamantTierWorth), EEquipmentType.Thruster, 9);
        }

        private void RegisterHulls(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Hulls.Scrap, "ScrapHull", LoadHullTextures(manager, "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShipDown"), new Hull(ID: GameIds.Items.Hulls.Scrap, Durability: 5, Health: 50, Name: "Scrap Hull", Worth: ScrapTierWorth, Weight: 5), EEquipmentType.Hull, 0);
            AddEquipmentDefinition(GameIds.Items.Hulls.Copper, "CopperHull", LoadHullTextures(manager, "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShipDown"), new Hull(ID: GameIds.Items.Hulls.Copper, Durability: 10, Health: 130, Name: "Copper Hull", Worth: CopperTierWorth, Weight: 8), EEquipmentType.Hull, 1);
            AddEquipmentDefinition(GameIds.Items.Hulls.Iron, "IronHull", LoadHullTextures(manager, "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShipDown"), new Hull(ID: GameIds.Items.Hulls.Iron, Durability: 18, Health: 260, Name: "Iron Hull", Worth: IronTierWorth, Weight: 10), EEquipmentType.Hull, 2);
            AddEquipmentDefinition(GameIds.Items.Hulls.Gold, "GoldHull", LoadHullTextures(manager, "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShipDown"), new Hull(ID: GameIds.Items.Hulls.Gold, Durability: 30, Health: 480, Name: "Gold Hull", Worth: GoldTierWorth, Weight: 12), EEquipmentType.Hull, 3);
            AddEquipmentDefinition(GameIds.Items.Hulls.Crystal, "CrystalHull", LoadHullTextures(manager, "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShipDown"), new Hull(ID: GameIds.Items.Hulls.Crystal, Durability: 48, Health: 820, Name: "Crystal Hull", Worth: CrystalTierWorth, Weight: 12), EEquipmentType.Hull, 4);
            AddEquipmentDefinition(GameIds.Items.Hulls.Diamond, "DiamondHull", LoadHullTextures(manager, "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShipDown"), new Hull(ID: GameIds.Items.Hulls.Diamond, Durability: 72, Health: 1300, Name: "Diamond Hull", Worth: DiamondTierWorth, Weight: 11), EEquipmentType.Hull, 5);
            AddEquipmentDefinition(GameIds.Items.Hulls.Radioactive, "RadioactiveHull", LoadHullTextures(manager, "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShipDown"), new Hull(ID: GameIds.Items.Hulls.Radioactive, Durability: 104, Health: 2000, Name: "Radioactive Hull", Worth: RadioactiveTierWorth, Weight: 10), EEquipmentType.Hull, 6);
            AddEquipmentDefinition(GameIds.Items.Hulls.Rainbow, "RainbowHull", LoadHullTextures(manager, "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: GameIds.Items.Hulls.Rainbow, Durability: 150, Health: 3000, Name: "Rainbow Hull", Worth: RainbowTierWorth, Weight: 9), EEquipmentType.Hull, 7);
            AddEquipmentDefinition(GameIds.Items.Hulls.Mythril, "MythrilHull", LoadHullTextures(manager, "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShipDown"), new Hull(ID: GameIds.Items.Hulls.Mythril, Durability: 220, Health: 4500, Name: "Mythril Hull", Worth: MythrilTierWorth, Weight: 8), EEquipmentType.Hull, 8);
            AddEquipmentDefinition(GameIds.Items.Hulls.Adamant, "AdamantHull", LoadHullTextures(manager, "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShipDown"), new Hull(ID: GameIds.Items.Hulls.Adamant, Durability: 320, Health: 7000, Name: "Adamant Hull", Worth: AdamantTierWorth, Weight: 8), EEquipmentType.Hull, 9);
        }

        private void RegisterDrills(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Drills.Scrap, "ScrapDrill", LoadDrillTextures(manager, "Player/Drills/DirtDrill/DirtDrill"), new Drill(ID: GameIds.Items.Drills.Scrap, Hardness: 5f, Damage: 0.25f, Name: "Scrap Drill", Worth: ScrapTierWorth, Weight: 5, MiningAreaSize: 1, ActiveFuelConsumption: 0.1f), EEquipmentType.Drill, 0);
            AddEquipmentDefinition(GameIds.Items.Drills.Copper, "CopperDrill", LoadDrillTextures(manager, "Player/Drills/CopperDrill/CopperDrill"), new Drill(ID: GameIds.Items.Drills.Copper, Hardness: 15f, Damage: 0.60f, Name: "Copper Drill", Worth: CopperTierWorth, Weight: 6, MiningAreaSize: 1, ActiveFuelConsumption: 0.2f), EEquipmentType.Drill, 1);
            AddEquipmentDefinition(GameIds.Items.Drills.Iron, "IronDrill", LoadDrillTextures(manager, "Player/Drills/IronDrill/IronDrill"), new Drill(ID: GameIds.Items.Drills.Iron, Hardness: 35f, Damage: 1.2f, Name: "Iron Drill", Worth: IronTierWorth, Weight: 7, MiningAreaSize: 1, ActiveFuelConsumption: 0.6f), EEquipmentType.Drill, 2);
            AddEquipmentDefinition(GameIds.Items.Drills.Gold, "GoldDrill", LoadDrillTextures(manager, "Player/Drills/GoldDrill/GoldDrill"), new Drill(ID: GameIds.Items.Drills.Gold, Hardness: 80f, Damage: 3.0f, Name: "Gold Drill", Worth: GoldTierWorth, Weight: 8, MiningAreaSize: 1, ActiveFuelConsumption: 1f), EEquipmentType.Drill, 3);
            AddEquipmentDefinition(GameIds.Items.Drills.Crystal, "CrystalDrill", LoadDrillTextures(manager, "Player/Drills/CrystalDrill/CrystalDrill"), new Drill(ID: GameIds.Items.Drills.Crystal, Hardness: 180f, Damage: 4f, Name: "Crystal Drill", Worth: CrystalTierWorth, Weight: 8, MiningAreaSize: 1, ActiveFuelConsumption: 2f), EEquipmentType.Drill, 4);
            AddEquipmentDefinition(GameIds.Items.Drills.Diamond, "DiamondDrill", LoadDrillTextures(manager, "Player/Drills/DiamondDrill/DiamondDrill"), new Drill(ID: GameIds.Items.Drills.Diamond, Hardness: 400f, Damage: 5.0f, Name: "Diamond Drill", Worth: DiamondTierWorth, Weight: 7, MiningAreaSize: 1, ActiveFuelConsumption: 3f), EEquipmentType.Drill, 5);
            AddEquipmentDefinition(GameIds.Items.Drills.Radioactive, "RadioactiveDrill", LoadDrillTextures(manager, "Player/Drills/RadioactiveDrill/RadioactiveDrill"), new Drill(ID: GameIds.Items.Drills.Radioactive, Hardness: 900f, Damage: 10.0f, Name: "Radioactive Drill", Worth: RadioactiveTierWorth, Weight: 6, MiningAreaSize: 1, ActiveFuelConsumption: 5f), EEquipmentType.Drill, 6);
            AddEquipmentDefinition(GameIds.Items.Drills.Rainbow, "RainbowDrill", LoadDrillTextures(manager, "Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: GameIds.Items.Drills.Rainbow, Hardness: 2000f, Damage: 25.0f, Name: "Rainbow Drill", Worth: RainbowTierWorth, Weight: 5, MiningAreaSize: 1, ActiveFuelConsumption: 25f), EEquipmentType.Drill, 7);
            AddEquipmentDefinition(GameIds.Items.Drills.Mythril, "MythrilDrill", LoadDrillTextures(manager, "Player/Drills/MythrilDrill/MythrilDrill"), new Drill(ID: GameIds.Items.Drills.Mythril, Hardness: 4000f, Damage: 50.0f, Name: "Mythril Drill", Worth: MythrilTierWorth, Weight: 4, MiningAreaSize: 3, ActiveFuelConsumption: 100f), EEquipmentType.Drill, 8, frames: 4);
            AddEquipmentDefinition(GameIds.Items.Drills.Adamant, "AdamantDrill", LoadDrillTextures(manager, "Player/Drills/AdamantDrill/AdamantDrill"), new Drill(ID: GameIds.Items.Drills.Adamant, Hardness: 10000f, Damage: 100.0f, Name: "Adamant Drill", Worth: AdamantTierWorth, Weight: 4, MiningAreaSize: 9, ActiveFuelConsumption: 250f), EEquipmentType.Drill, 9, frames: 4);
        }

        private void RegisterGadgets(ContentManager manager)
        {
            AddDefinition(
                GameIds.Items.Gadgets.GadgetBelt,
                "GadgetBelt",
                [],
                new Item(ID: GameIds.Items.Gadgets.GadgetBelt, Name: "Gadget Belt", Worth: 10000.0f, Weight: 1.0f),
                buyable: true);
            AddDefinition(
                GameIds.Items.Gadgets.DirtFilter,
                "DirtFilter",
                LoadSingleTexture(manager, "Blocks/DirtBlock"),
                new Item(ID: GameIds.Items.Gadgets.DirtFilter, Name: "Dirt Filter", Worth: 25000.0f, Weight: 1.0f, Stackable: false),
                buyable: true);
            AddDefinition(
                GameIds.Items.Gadgets.RockFilter,
                "RockFilter",
                LoadSingleTexture(manager, "Blocks/StoneBlock"),
                new Item(ID: GameIds.Items.Gadgets.RockFilter, Name: "Rock Filter", Worth: 50000.0f, Weight: 1.0f, Stackable: false),
                buyable: true);
        }

        private void RegisterConsumeables(ContentManager manager)
        {
            AddDefinition(
                GameIds.Items.Consumeables.SmallDynamite,
                "SmallDynamite",
                LoadSingleTexture(manager, "Dynamite/DynamiteTierOne"),
                new SmallDynamite(ID: GameIds.Items.Consumeables.SmallDynamite, Name: "Small Dynamite", ExplosionAreaSize: 3, Damage: 100.0f, MaxHardness: 1000000.0f, Worth: 250.0f, Weight: 1.0f),
                buyable: true,
                type: EGameItemType.Consumeable);
        }

        private void AddDefinition<T>(int id, string name, Dictionary<PlayerOrientation, Texture2D> textures, T definition, bool buyable = false, EGameItemType type = EGameItemType.Item, int frames = 1) where T : AType
        {
            Add(id, new GameItemDefinition(name, textures, definition, () => CreateCopy(definition), buyable: buyable, type: type, frames: frames));
        }

        private void AddEquipmentDefinition<T>(int id, string name, Dictionary<PlayerOrientation, Texture2D> textures, T definition, EEquipmentType equipmentType, int tier, int frames = 1) where T : AType
        {
            Add(id, new GameItemDefinition(name, textures, definition, () => CreateCopy(definition), buyable: true, type: EGameItemType.Equipment, equipmentType: equipmentType, tier: tier, frames: frames));
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

        private static AType CreateCopy(AType definition)
        {
            return definition switch
            {
                Item item => new Item(item),
                SmallDynamite dynamite => new SmallDynamite(dynamite),
                ThermalPlating plating => new ThermalPlating(plating),
                Hull hull => new Hull(hull),
                Drill drill => new Drill(drill),
                Engine engine => new Engine(engine),
                Inventory inventory => new Inventory(inventory),
                FuelTank fuelTank => new FuelTank(fuelTank),
                Thruster thruster => new Thruster(thruster),
                _ => throw new InvalidOperationException($"Unsupported item definition type '{definition.GetType().Name}'.")
            };
        }
    }
}


