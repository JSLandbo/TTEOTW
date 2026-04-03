using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
            RegisterMaterials(manager);
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

            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Scrap, "Scrap Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/ScrapThermalPlating"), scrapPlating, EEquipmentType.ThermalPlating, 0);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Copper, "Copper Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/CopperThermalPlating"), copperPlating, EEquipmentType.ThermalPlating, 1);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Iron, "Iron Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/IronThermalPlating"), ironPlating, EEquipmentType.ThermalPlating, 2);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Gold, "Gold Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/GoldThermalPlating"), goldPlating, EEquipmentType.ThermalPlating, 3);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Crystal, "Crystal Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/CrystalThermalPlating"), crystalPlating, EEquipmentType.ThermalPlating, 4);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Diamond, "Diamond Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/DiamondThermalPlating"), diamondPlating, EEquipmentType.ThermalPlating, 5);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Radioactive, "Radioactive Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/RadioactiveThermalPlating"), radioactivePlating, EEquipmentType.ThermalPlating, 6);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Rainbow, "Rainbow Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/RainbowThermalPlating"), rainbowPlating, EEquipmentType.ThermalPlating, 7);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Mythril, "Mythril Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/MythrilThermalPlating"), mythrilPlating, EEquipmentType.ThermalPlating, 8);
            AddEquipmentDefinition(GameIds.Items.ThermalPlatings.Adamant, "Adamant Thermal Plating", LoadSingleTexture(manager, "Player/ThermalPlatings/AdamantThermalPlating"), adamantPlating, EEquipmentType.ThermalPlating, 9);
        }

        private void RegisterEngines(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Engines.Scrap, "Scrap Engine", LoadSingleTexture(manager, "Player/Engines/ScrapEngine"), new Engine(ID: GameIds.Items.Engines.Scrap, Speed: 400f, Acceleration: 100f, Name: "Scrap Engine", StandbyFuelConsumption: 0.001f, ActiveFuelConsumption: 0.05f, Worth: ScrapTierWorth, Weight: 5), EEquipmentType.Engine, 0);
            AddEquipmentDefinition(GameIds.Items.Engines.Copper, "Copper Engine", LoadSingleTexture(manager, "Player/Engines/CopperEngine"), new Engine(ID: GameIds.Items.Engines.Copper, Speed: 600f, Acceleration: 150f, Name: "Copper Engine", StandbyFuelConsumption: 0.02f, ActiveFuelConsumption: 0.2f, Worth: CopperTierWorth, Weight: 6), EEquipmentType.Engine, 1);
            AddEquipmentDefinition(GameIds.Items.Engines.Iron, "Iron Engine", LoadSingleTexture(manager, "Player/Engines/IronEngine"), new Engine(ID: GameIds.Items.Engines.Iron, Speed: 900f, Acceleration: 250f, Name: "Iron Engine", StandbyFuelConsumption: 0.06f, ActiveFuelConsumption: 0.4f, Worth: IronTierWorth, Weight: 7), EEquipmentType.Engine, 2);
            AddEquipmentDefinition(GameIds.Items.Engines.Gold, "Gold Engine", LoadSingleTexture(manager, "Player/Engines/GoldEngine"), new Engine(ID: GameIds.Items.Engines.Gold, Speed: 1450f, Acceleration: 400f, Name: "Gold Engine", StandbyFuelConsumption: 0.07f, ActiveFuelConsumption: 0.6f, Worth: GoldTierWorth, Weight: 8), EEquipmentType.Engine, 3);
            AddEquipmentDefinition(GameIds.Items.Engines.Crystal, "Crystal Engine", LoadSingleTexture(manager, "Player/Engines/CrystalEngine"), new Engine(ID: GameIds.Items.Engines.Crystal, Speed: 2600f, Acceleration: 600f, Name: "Crystal Engine", StandbyFuelConsumption: 0.058f, ActiveFuelConsumption: 1.0f, Worth: CrystalTierWorth, Weight: 8), EEquipmentType.Engine, 4);
            AddEquipmentDefinition(GameIds.Items.Engines.Diamond, "Diamond Engine", LoadSingleTexture(manager, "Player/Engines/DiamondEngine"), new Engine(ID: GameIds.Items.Engines.Diamond, Speed: 4800f, Acceleration: 1200f, Name: "Diamond Engine", StandbyFuelConsumption: 0.046f, ActiveFuelConsumption: 2.0f, Worth: DiamondTierWorth, Weight: 7), EEquipmentType.Engine, 5);
            AddEquipmentDefinition(GameIds.Items.Engines.Radioactive, "Radioactive Engine", LoadSingleTexture(manager, "Player/Engines/RadioactiveEngine"), new Engine(ID: GameIds.Items.Engines.Radioactive, Speed: 7000f, Acceleration: 2500f, Name: "Radioactive Engine", StandbyFuelConsumption: 0.034f, ActiveFuelConsumption: 4f, Worth: RadioactiveTierWorth, Weight: 6), EEquipmentType.Engine, 6);
            AddEquipmentDefinition(GameIds.Items.Engines.Rainbow, "Rainbow Engine", LoadSingleTexture(manager, "Player/Engines/RainbowEngine"), new Engine(ID: GameIds.Items.Engines.Rainbow, Speed: 10000f, Acceleration: 5000f, Name: "Rainbow Engine", StandbyFuelConsumption: 0.024f, ActiveFuelConsumption: 6f, Worth: RainbowTierWorth, Weight: 5), EEquipmentType.Engine, 7);
            AddEquipmentDefinition(GameIds.Items.Engines.Mythril, "Mythril Engine", LoadSingleTexture(manager, "Player/Engines/MythrilEngine"), new Engine(ID: GameIds.Items.Engines.Mythril, Speed: 24000f, Acceleration: 7500f, Name: "Mythril Engine", StandbyFuelConsumption: 0.018f, ActiveFuelConsumption: 14f, Worth: MythrilTierWorth, Weight: 4), EEquipmentType.Engine, 8);
            AddEquipmentDefinition(GameIds.Items.Engines.Adamant, "Adamant Engine", LoadSingleTexture(manager, "Player/Engines/AdamantEngine"), new Engine(ID: GameIds.Items.Engines.Adamant, Speed: 36000f, Acceleration: 10000f, Name: "Adamant Engine", StandbyFuelConsumption: 0.012f, ActiveFuelConsumption: 20f, Worth: AdamantTierWorth, Weight: 4), EEquipmentType.Engine, 9);
        }

        private void RegisterFuelTanks(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Scrap, "Scrap Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/ScrapFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Scrap, Capacity: 10, Fuel: 10, Name: "Scrap Fuel Tank", Worth: ScrapTierWorth, Weight: 5), EEquipmentType.FuelTank, 0);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Copper, "Copper Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/CopperFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Copper, Capacity: 25, Fuel: 25, Name: "Copper Fuel Tank", Worth: CopperTierWorth, Weight: 6), EEquipmentType.FuelTank, 1);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Iron, "Iron Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/IronFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Iron, Capacity: 100, Fuel: 100, Name: "Iron Fuel Tank", Worth: IronTierWorth, Weight: 7), EEquipmentType.FuelTank, 2);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Gold, "Gold Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/GoldFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Gold, Capacity: 250, Fuel: 250, Name: "Gold Fuel Tank", Worth: GoldTierWorth, Weight: 8), EEquipmentType.FuelTank, 3);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Crystal, "Crystal Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/CrystalFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Crystal, Capacity: 750, Fuel: 750, Name: "Crystal Fuel Tank", Worth: CrystalTierWorth, Weight: 8), EEquipmentType.FuelTank, 4);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Diamond, "Diamond Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/DiamondFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Diamond, Capacity: 1250, Fuel: 1250, Name: "Diamond Fuel Tank", Worth: DiamondTierWorth, Weight: 7), EEquipmentType.FuelTank, 5);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Radioactive, "Radioactive Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/RadioactiveFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Radioactive, Capacity: 1500, Fuel: 1500, Name: "Radioactive Fuel Tank", Worth: RadioactiveTierWorth, Weight: 6), EEquipmentType.FuelTank, 6);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Rainbow, "Rainbow Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/RainbowFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Rainbow, Capacity: 2000, Fuel: 2000, Name: "Rainbow Fuel Tank", Worth: RainbowTierWorth, Weight: 5), EEquipmentType.FuelTank, 7);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Mythril, "Mythril Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/MythrilFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Mythril, Capacity: 3500, Fuel: 3500, Name: "Mythril Fuel Tank", Worth: MythrilTierWorth, Weight: 4), EEquipmentType.FuelTank, 8);
            AddEquipmentDefinition(GameIds.Items.FuelTanks.Adamant, "Adamant Fuel Tank", LoadSingleTexture(manager, "Player/FuelTanks/AdamantFuelTank"), new FuelTank(ID: GameIds.Items.FuelTanks.Adamant, Capacity: 5000, Fuel: 5000, Name: "Adamant Fuel Tank", Worth: AdamantTierWorth, Weight: 4), EEquipmentType.FuelTank, 9);
        }

        private void RegisterContainers(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Containers.Scrap, "Scrap Container", LoadSingleTexture(manager, "Player/Containers/ScrapContainer"), new Inventory(ID: GameIds.Items.Containers.Scrap, Items: CreateInventoryGrid(3, 1), SizeLimit: 16, Name: "Scrap Container", Worth: ScrapTierWorth, Weight: 5, MaxStackSize: 8), EEquipmentType.Inventory, 0);
            AddEquipmentDefinition(GameIds.Items.Containers.Copper, "Copper Container", LoadSingleTexture(manager, "Player/Containers/CopperContainer"), new Inventory(ID: GameIds.Items.Containers.Copper, Items: CreateInventoryGrid(2, 2), SizeLimit: 64, Name: "Copper Container", Worth: CopperTierWorth, Weight: 6, MaxStackSize: 16), EEquipmentType.Inventory, 1);
            AddEquipmentDefinition(GameIds.Items.Containers.Iron, "Iron Container", LoadSingleTexture(manager, "Player/Containers/IronContainer"), new Inventory(ID: GameIds.Items.Containers.Iron, Items: CreateInventoryGrid(2, 3), SizeLimit: 128, Name: "Iron Container", Worth: IronTierWorth, Weight: 7, MaxStackSize: 32), EEquipmentType.Inventory, 2);
            AddEquipmentDefinition(GameIds.Items.Containers.Gold, "Gold Container", LoadSingleTexture(manager, "Player/Containers/GoldContainer"), new Inventory(ID: GameIds.Items.Containers.Gold, Items: CreateInventoryGrid(3, 3), SizeLimit: 256, Name: "Gold Container", Worth: GoldTierWorth, Weight: 8, MaxStackSize: 64), EEquipmentType.Inventory, 3);
            AddEquipmentDefinition(GameIds.Items.Containers.Crystal, "Crystal Container", LoadSingleTexture(manager, "Player/Containers/CrystalContainer"), new Inventory(ID: GameIds.Items.Containers.Crystal, Items: CreateInventoryGrid(3, 4), SizeLimit: 512, Name: "Crystal Container", Worth: CrystalTierWorth, Weight: 8, MaxStackSize: 64), EEquipmentType.Inventory, 4);
            AddEquipmentDefinition(GameIds.Items.Containers.Diamond, "Diamond Container", LoadSingleTexture(manager, "Player/Containers/DiamondContainer"), new Inventory(ID: GameIds.Items.Containers.Diamond, Items: CreateInventoryGrid(4, 4), SizeLimit: 1024, Name: "Diamond Container", Worth: DiamondTierWorth, Weight: 7, MaxStackSize: 64), EEquipmentType.Inventory, 5);
            AddEquipmentDefinition(GameIds.Items.Containers.Radioactive, "Radioactive Container", LoadSingleTexture(manager, "Player/Containers/RadioactiveContainer"), new Inventory(ID: GameIds.Items.Containers.Radioactive, Items: CreateInventoryGrid(4, 5), SizeLimit: 2048, Name: "Radioactive Container", Worth: RadioactiveTierWorth, Weight: 6, MaxStackSize: 64), EEquipmentType.Inventory, 6);
            AddEquipmentDefinition(GameIds.Items.Containers.Rainbow, "Rainbow Container", LoadSingleTexture(manager, "Player/Containers/RainbowContainer"), new Inventory(ID: GameIds.Items.Containers.Rainbow, Items: CreateInventoryGrid(5, 5), SizeLimit: 4096, Name: "Rainbow Container", Worth: RainbowTierWorth, Weight: 5, MaxStackSize: 64), EEquipmentType.Inventory, 7);
            AddEquipmentDefinition(GameIds.Items.Containers.Mythril, "Mythril Container", LoadSingleTexture(manager, "Player/Containers/MythrilContainer"), new Inventory(ID: GameIds.Items.Containers.Mythril, Items: CreateInventoryGrid(8, 6), SizeLimit: 16384, Name: "Mythril Container", Worth: MythrilTierWorth, Weight: 4, MaxStackSize: 128), EEquipmentType.Inventory, 8);
            AddEquipmentDefinition(GameIds.Items.Containers.Adamant, "Adamant Container", LoadSingleTexture(manager, "Player/Containers/AdamantContainer"), new Inventory(ID: GameIds.Items.Containers.Adamant, Items: CreateInventoryGrid(18, 8), SizeLimit: 73728, Name: "Adamant Container", Worth: AdamantTierWorth, Weight: 4, MaxStackSize: 512), EEquipmentType.Inventory, 9);
        }

        private void RegisterThrusters(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Thrusters.Scrap, "Scrap Thruster", LoadSingleTexture(manager, "Player/Thrusters/ScrapThruster"), new Thruster(ID: GameIds.Items.Thrusters.Scrap, Speed: 500f, Acceleration: 250f, Power: 50, Name: "Scrap Thruster", ActiveFuelConsumption: 0.05f, ActiveHeatGeneration: 0.10f, Weight: 5, Worth: ScrapTierWorth), EEquipmentType.Thruster, 0);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Copper, "Copper Thruster", LoadSingleTexture(manager, "Player/Thrusters/CopperThruster"), new Thruster(ID: GameIds.Items.Thrusters.Copper, Speed: 1100f, Acceleration: 350f, Power: 75, Name: "Copper Thruster", ActiveFuelConsumption: 0.1f, ActiveHeatGeneration: 0.18f, Weight: 6, Worth: CopperTierWorth), EEquipmentType.Thruster, 1);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Iron, "Iron Thruster", LoadSingleTexture(manager, "Player/Thrusters/IronThruster"), new Thruster(ID: GameIds.Items.Thrusters.Iron, Speed: 2000f, Acceleration: 500f, Power: 100, Name: "Iron Thruster", ActiveFuelConsumption: 0.2f, ActiveHeatGeneration: 0.28f, Weight: 7, Worth: IronTierWorth), EEquipmentType.Thruster, 2);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Gold, "Gold Thruster", LoadSingleTexture(manager, "Player/Thrusters/GoldThruster"), new Thruster(ID: GameIds.Items.Thrusters.Gold, Speed: 4000f, Acceleration: 650f, Power: 200, Name: "Gold Thruster", ActiveFuelConsumption: 0.6f, ActiveHeatGeneration: 0.40f, Weight: 8, Worth: GoldTierWorth), EEquipmentType.Thruster, 3);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Crystal, "Crystal Thruster", LoadSingleTexture(manager, "Player/Thrusters/CrystalThruster"), new Thruster(ID: GameIds.Items.Thrusters.Crystal, Speed: 8000f, Acceleration: 800f, Power: 400, Name: "Crystal Thruster", ActiveFuelConsumption: 1.4f, ActiveHeatGeneration: 0.55f, Weight: 8, Worth: CrystalTierWorth), EEquipmentType.Thruster, 4);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Diamond, "Diamond Thruster", LoadSingleTexture(manager, "Player/Thrusters/DiamondThruster"), new Thruster(ID: GameIds.Items.Thrusters.Diamond, Speed: 24000f, Acceleration: 1000f, Power: 600, Name: "Diamond Thruster", ActiveFuelConsumption: 2.8f, ActiveHeatGeneration: 1f, Weight: 7, Worth: DiamondTierWorth), EEquipmentType.Thruster, 5);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Radioactive, "Radioactive Thruster", LoadSingleTexture(manager, "Player/Thrusters/RadioactiveThruster"), new Thruster(ID: GameIds.Items.Thrusters.Radioactive, Speed: 15360f, Acceleration: 1200f, Power: 1250, Name: "Radioactive Thruster", ActiveFuelConsumption: 4.7f, ActiveHeatGeneration: 2f, Weight: 6, Worth: RadioactiveTierWorth), EEquipmentType.Thruster, 6);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Rainbow, "Rainbow Thruster", LoadSingleTexture(manager, "Player/Thrusters/RainbowThruster"), new Thruster(ID: GameIds.Items.Thrusters.Rainbow, Speed: 48000f, Acceleration: 1750f, Power: 2500, Name: "Rainbow Thruster", ActiveFuelConsumption: 4.0f, ActiveHeatGeneration: 3f, Weight: 5, Worth: RainbowTierWorth), EEquipmentType.Thruster, 7);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Mythril, "Mythril Thruster", LoadSingleTexture(manager, "Player/Thrusters/MythrilThruster"), new Thruster(ID: GameIds.Items.Thrusters.Mythril, Speed: 96000f, Acceleration: 2500f, Power: 7500, Name: "Mythril Thruster", ActiveFuelConsumption: 12.0f, ActiveHeatGeneration: 4f, Weight: 4, Worth: MythrilTierWorth), EEquipmentType.Thruster, 8);
            AddEquipmentDefinition(GameIds.Items.Thrusters.Adamant, "Adamant Thruster", LoadSingleTexture(manager, "Player/Thrusters/AdamantThruster"), new Thruster(ID: GameIds.Items.Thrusters.Adamant, Speed: 192000f, Acceleration: 4000f, Power: 15000, Name: "Adamant Thruster", ActiveFuelConsumption: 16.0f, ActiveHeatGeneration: 6f, Weight: 4, Worth: AdamantTierWorth), EEquipmentType.Thruster, 9);
        }

        private void RegisterHulls(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Hulls.Scrap, "Scrap Hull", LoadHullTextures(manager, "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShip", "Player/Hulls/ScrapHull/ScrapShipDown"), new Hull(ID: GameIds.Items.Hulls.Scrap, Durability: 5, Health: 50, Name: "Scrap Hull", Worth: ScrapTierWorth, Weight: 5), EEquipmentType.Hull, 0);
            AddEquipmentDefinition(GameIds.Items.Hulls.Copper, "Copper Hull", LoadHullTextures(manager, "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShip", "Player/Hulls/CopperHull/CopperShipDown"), new Hull(ID: GameIds.Items.Hulls.Copper, Durability: 10, Health: 130, Name: "Copper Hull", Worth: CopperTierWorth, Weight: 8), EEquipmentType.Hull, 1);
            AddEquipmentDefinition(GameIds.Items.Hulls.Iron, "Iron Hull", LoadHullTextures(manager, "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShip", "Player/Hulls/IronHull/IronShipDown"), new Hull(ID: GameIds.Items.Hulls.Iron, Durability: 18, Health: 260, Name: "Iron Hull", Worth: IronTierWorth, Weight: 10), EEquipmentType.Hull, 2);
            AddEquipmentDefinition(GameIds.Items.Hulls.Gold, "Gold Hull", LoadHullTextures(manager, "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShip", "Player/Hulls/GoldHull/GoldShipDown"), new Hull(ID: GameIds.Items.Hulls.Gold, Durability: 30, Health: 480, Name: "Gold Hull", Worth: GoldTierWorth, Weight: 12), EEquipmentType.Hull, 3);
            AddEquipmentDefinition(GameIds.Items.Hulls.Crystal, "Crystal Hull", LoadHullTextures(manager, "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShip", "Player/Hulls/CrystalHull/CrystalShipDown"), new Hull(ID: GameIds.Items.Hulls.Crystal, Durability: 48, Health: 820, Name: "Crystal Hull", Worth: CrystalTierWorth, Weight: 12), EEquipmentType.Hull, 4);
            AddEquipmentDefinition(GameIds.Items.Hulls.Diamond, "Diamond Hull", LoadHullTextures(manager, "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShip", "Player/Hulls/DiamondHull/DiamondShipDown"), new Hull(ID: GameIds.Items.Hulls.Diamond, Durability: 72, Health: 1300, Name: "Diamond Hull", Worth: DiamondTierWorth, Weight: 11), EEquipmentType.Hull, 5);
            AddEquipmentDefinition(GameIds.Items.Hulls.Radioactive, "Radioactive Hull", LoadHullTextures(manager, "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShip", "Player/Hulls/RadioactiveHull/RadioactiveShipDown"), new Hull(ID: GameIds.Items.Hulls.Radioactive, Durability: 104, Health: 2000, Name: "Radioactive Hull", Worth: RadioactiveTierWorth, Weight: 10), EEquipmentType.Hull, 6);
            AddEquipmentDefinition(GameIds.Items.Hulls.Rainbow, "Rainbow Hull", LoadHullTextures(manager, "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShip", "Player/Hulls/RainbowHull/RainbowShipDown"), new Hull(ID: GameIds.Items.Hulls.Rainbow, Durability: 150, Health: 3000, Name: "Rainbow Hull", Worth: RainbowTierWorth, Weight: 9), EEquipmentType.Hull, 7);
            AddEquipmentDefinition(GameIds.Items.Hulls.Mythril, "Mythril Hull", LoadHullTextures(manager, "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShip", "Player/Hulls/MythrilHull/MythrilShipDown"), new Hull(ID: GameIds.Items.Hulls.Mythril, Durability: 220, Health: 4500, Name: "Mythril Hull", Worth: MythrilTierWorth, Weight: 8), EEquipmentType.Hull, 8);
            AddEquipmentDefinition(GameIds.Items.Hulls.Adamant, "Adamant Hull", LoadHullTextures(manager, "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShip", "Player/Hulls/AdamantHull/AdamantShipDown"), new Hull(ID: GameIds.Items.Hulls.Adamant, Durability: 320, Health: 7000, Name: "Adamant Hull", Worth: AdamantTierWorth, Weight: 8), EEquipmentType.Hull, 9);
        }

        private void RegisterDrills(ContentManager manager)
        {
            AddEquipmentDefinition(GameIds.Items.Drills.Scrap, "Scrap Drill", LoadDrillTextures(manager, "Player/Drills/DirtDrill/DirtDrill"), new Drill(ID: GameIds.Items.Drills.Scrap, Hardness: 5f, Damage: 0.25f, Name: "Scrap Drill", Worth: ScrapTierWorth, Weight: 5, MiningAreaSize: 1, ActiveFuelConsumption: 0.1f), EEquipmentType.Drill, 0);
            AddEquipmentDefinition(GameIds.Items.Drills.Copper, "Copper Drill", LoadDrillTextures(manager, "Player/Drills/CopperDrill/CopperDrill"), new Drill(ID: GameIds.Items.Drills.Copper, Hardness: 15f, Damage: 0.60f, Name: "Copper Drill", Worth: CopperTierWorth, Weight: 6, MiningAreaSize: 1, ActiveFuelConsumption: 0.2f), EEquipmentType.Drill, 1);
            AddEquipmentDefinition(GameIds.Items.Drills.Iron, "Iron Drill", LoadDrillTextures(manager, "Player/Drills/IronDrill/IronDrill"), new Drill(ID: GameIds.Items.Drills.Iron, Hardness: 35f, Damage: 1.2f, Name: "Iron Drill", Worth: IronTierWorth, Weight: 7, MiningAreaSize: 1, ActiveFuelConsumption: 0.6f), EEquipmentType.Drill, 2);
            AddEquipmentDefinition(GameIds.Items.Drills.Gold, "Gold Drill", LoadDrillTextures(manager, "Player/Drills/GoldDrill/GoldDrill"), new Drill(ID: GameIds.Items.Drills.Gold, Hardness: 80f, Damage: 3.0f, Name: "Gold Drill", Worth: GoldTierWorth, Weight: 8, MiningAreaSize: 1, ActiveFuelConsumption: 1f), EEquipmentType.Drill, 3);
            AddEquipmentDefinition(GameIds.Items.Drills.Crystal, "Crystal Drill", LoadDrillTextures(manager, "Player/Drills/CrystalDrill/CrystalDrill"), new Drill(ID: GameIds.Items.Drills.Crystal, Hardness: 180f, Damage: 4f, Name: "Crystal Drill", Worth: CrystalTierWorth, Weight: 8, MiningAreaSize: 1, ActiveFuelConsumption: 2f), EEquipmentType.Drill, 4);
            AddEquipmentDefinition(GameIds.Items.Drills.Diamond, "Diamond Drill", LoadDrillTextures(manager, "Player/Drills/DiamondDrill/DiamondDrill"), new Drill(ID: GameIds.Items.Drills.Diamond, Hardness: 400f, Damage: 5.0f, Name: "Diamond Drill", Worth: DiamondTierWorth, Weight: 7, MiningAreaSize: 1, ActiveFuelConsumption: 3f), EEquipmentType.Drill, 5);
            AddEquipmentDefinition(GameIds.Items.Drills.Radioactive, "Radioactive Drill", LoadDrillTextures(manager, "Player/Drills/RadioactiveDrill/RadioactiveDrill"), new Drill(ID: GameIds.Items.Drills.Radioactive, Hardness: 900f, Damage: 10.0f, Name: "Radioactive Drill", Worth: RadioactiveTierWorth, Weight: 6, MiningAreaSize: 1, ActiveFuelConsumption: 5f), EEquipmentType.Drill, 6);
            AddEquipmentDefinition(GameIds.Items.Drills.Rainbow, "Rainbow Drill", LoadDrillTextures(manager, "Player/Drills/RainbowDrill/RainbowDrill"), new Drill(ID: GameIds.Items.Drills.Rainbow, Hardness: 2000f, Damage: 25.0f, Name: "Rainbow Drill", Worth: RainbowTierWorth, Weight: 5, MiningAreaSize: 1, ActiveFuelConsumption: 25f), EEquipmentType.Drill, 7);
            AddEquipmentDefinition(GameIds.Items.Drills.Mythril, "Mythril Drill", LoadDrillTextures(manager, "Player/Drills/MythrilDrill/MythrilDrill"), new Drill(ID: GameIds.Items.Drills.Mythril, Hardness: 4000f, Damage: 50.0f, Name: "Mythril Drill", Worth: MythrilTierWorth, Weight: 4, MiningAreaSize: 3, ActiveFuelConsumption: 100f), EEquipmentType.Drill, 8, frames: 4);
            AddEquipmentDefinition(GameIds.Items.Drills.Adamant, "Adamant Drill", LoadDrillTextures(manager, "Player/Drills/AdamantDrill/AdamantDrill"), new Drill(ID: GameIds.Items.Drills.Adamant, Hardness: 10000f, Damage: 100.0f, Name: "Adamant Drill", Worth: AdamantTierWorth, Weight: 4, MiningAreaSize: 192, ActiveFuelConsumption: 250f), EEquipmentType.Drill, 9, frames: 4);
        }

        private void RegisterGadgets(ContentManager manager)
        {
            AddDefinition(
                GameIds.Items.Gadgets.GadgetBelt,
                "Gadget Belt",
                [],
                new Item(ID: GameIds.Items.Gadgets.GadgetBelt, Name: "Gadget Belt", Worth: 10000.0f, Weight: 1.0f),
                buyable: true
            );
            AddDefinition(
                GameIds.Items.Gadgets.DirtFilter,
                "Dirt Filter",
                LoadSingleTexture(manager, "Blocks/DirtBlock"),
                new Item(ID: GameIds.Items.Gadgets.DirtFilter, Name: "Dirt Filter", Worth: 25000.0f, Weight: 1.0f, Stackable: false),
                buyable: true
            );
            AddDefinition(
                GameIds.Items.Gadgets.RockFilter,
                "Rock Filter",
                LoadSingleTexture(manager, "Blocks/StoneBlock"),
                new Item(ID: GameIds.Items.Gadgets.RockFilter, Name: "Rock Filter", Worth: 50000.0f, Weight: 1.0f, Stackable: false),
                buyable: true
            );
        }

        private void RegisterConsumeables(ContentManager manager)
        {
            AddDefinition(
                GameIds.Items.Consumeables.SmallDynamite,
                "Small Dynamite",
                LoadSingleTexture(manager, "Consumeables/SmallDynamite"),
                new SmallDynamite(ID: GameIds.Items.Consumeables.SmallDynamite, Name: "Small Dynamite", ExplosionAreaSize: 3, ExplosionPlaybackFrames: 40, Damage: 100.0f, MaxHardness: 10000.0f, Worth: 250.0f, Weight: 0.05f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.MediumDynamite,
                "Medium Dynamite",
                LoadSingleTexture(manager, "Consumeables/MediumDynamite"),
                new MediumDynamite(ID: GameIds.Items.Consumeables.MediumDynamite, Name: "Medium Dynamite", ExplosionAreaSize: 5, ExplosionPlaybackFrames: 40, Damage: 1000.0f, MaxHardness: 100000.0f, Worth: 2500.0f, Weight: 0.50f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.LargeDynamite,
                "Large Dynamite",
                LoadSingleTexture(manager, "Consumeables/LargeDynamite"),
                new LargeDynamite(ID: GameIds.Items.Consumeables.LargeDynamite, Name: "Large Dynamite", ExplosionAreaSize: 9, ExplosionPlaybackFrames: 40, Damage: 10000.0f, MaxHardness: 1000000.0f, Worth: 25000.0f, Weight: 5.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.NukeDynamite,
                "Nuke Dynamite",
                LoadSingleTexture(manager, "Consumeables/NukeDynamite"),
                new NukeDynamite(ID: GameIds.Items.Consumeables.NukeDynamite, Name: "Nuke Dynamite", ExplosionAreaSize: 63, ExplosionPlaybackFrames: 40, Damage: 100000.0f, MaxHardness: 10000000.0f, Worth: 2500000.0f, Weight: 1000f, ExplosionType: ExplosionType.Nuclear),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.SmallFuelCapsule,
                "Small Fuel Capsule",
                LoadSingleTexture(manager, "Consumeables/SmallFuelCapsule"),
                new SmallFuelCapsule(ID: GameIds.Items.Consumeables.SmallFuelCapsule, Name: "Small Fuel Capsule", FuelAmount: 25.0f, Worth: 100.0f, Weight: 0.2f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.MediumFuelCapsule,
                "Medium Fuel Capsule",
                LoadSingleTexture(manager, "Consumeables/MediumFuelCapsule"),
                new MediumFuelCapsule(ID: GameIds.Items.Consumeables.MediumFuelCapsule, Name: "Medium Fuel Capsule", FuelAmount: 100.0f, Worth: 400.0f, Weight: 1.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.LargeFuelCapsule,
                "Large Fuel Capsule",
                LoadSingleTexture(manager, "Consumeables/LargeFuelCapsule"),
                new LargeFuelCapsule(ID: GameIds.Items.Consumeables.LargeFuelCapsule, Name: "Large Fuel Capsule", FuelAmount: 500.0f, Worth: 2500.0f, Weight: 5.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.SmallCoolantPatch,
                "Small Coolant Patch",
                LoadSingleTexture(manager, "Consumeables/SmallCoolantPatch"),
                new SmallCoolantPatch(ID: GameIds.Items.Consumeables.SmallCoolantPatch, Name: "Small Coolant Patch", CoolingAmount: 50.0f, Worth: 300.0f, Weight: 0.1f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.MediumCoolantPatch,
                "Medium Coolant Patch",
                LoadSingleTexture(manager, "Consumeables/MediumCoolantPatch"),
                new MediumCoolantPatch(ID: GameIds.Items.Consumeables.MediumCoolantPatch, Name: "Medium Coolant Patch", CoolingAmount: 250.0f, Worth: 2000.0f, Weight: 5.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.LargeCoolantPatch,
                "Large Coolant Patch",
                LoadSingleTexture(manager, "Consumeables/LargeCoolantPatch"),
                new LargeCoolantPatch(ID: GameIds.Items.Consumeables.LargeCoolantPatch, Name: "Large Coolant Patch", CoolingAmount: 1000.0f, Worth: 10000.0f, Weight: 25.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.SmallHullRepairKit,
                "Small Hull Repair Kit",
                LoadSingleTexture(manager, "Consumeables/SmallHullRepairKit"),
                new SmallHullRepairKit(ID: GameIds.Items.Consumeables.SmallHullRepairKit, Name: "Small Hull Repair Kit", RepairAmount: 50.0f, Worth: 350.0f, Weight: 0.1f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.MediumHullRepairKit,
                "Medium Hull Repair Kit",
                LoadSingleTexture(manager, "Consumeables/MediumHullRepairKit"),
                new MediumHullRepairKit(ID: GameIds.Items.Consumeables.MediumHullRepairKit, Name: "Medium Hull Repair Kit", RepairAmount: 250.0f, Worth: 2500.0f, Weight: 0.4f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
            AddDefinition(
                GameIds.Items.Consumeables.LargeHullRepairKit,
                "Large Hull Repair Kit",
                LoadSingleTexture(manager, "Consumeables/LargeHullRepairKit"),
                new LargeHullRepairKit(ID: GameIds.Items.Consumeables.LargeHullRepairKit, Name: "Large Hull Repair Kit", RepairAmount: 1250.0f, Worth: 20000.0f, Weight: 1.0f),
                buyable: true,
                type: EGameItemType.Consumeable
            );
        }

        private void RegisterMaterials(ContentManager manager)
        {
            AddDefinition(
                GameIds.Items.CratingMaterials.IronIngot,
                "Iron Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/IronIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.IronIngot, Name: "Iron Ingot", Worth: 8.0f, Weight: 0.9f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.CopperIngot,
                "Copper Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/CopperIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.CopperIngot, Name: "Copper Ingot", Worth: 14.0f, Weight: 0.8f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.TinIngot,
                "Tin Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/TinIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.TinIngot, Name: "Tin Ingot", Worth: 6.0f, Weight: 0.6f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.SilverIngot,
                "Silver Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/SilverIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.SilverIngot, Name: "Silver Ingot", Worth: 26.0f, Weight: 0.8f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.GoldIngot,
                "Gold Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/GoldIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.GoldIngot, Name: "Gold Ingot", Worth: 70.0f, Weight: 1.0f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.LeadIngot,
                "Lead Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/LeadIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.LeadIngot, Name: "Lead Ingot", Worth: 9.0f, Weight: 1.15f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.AluminiumIngot,
                "Aluminium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/AluminiumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.AluminiumIngot, Name: "Aluminium Ingot", Worth: 12.0f, Weight: 0.5f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.ZincIngot,
                "Zinc Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/ZincIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.ZincIngot, Name: "Zinc Ingot", Worth: 13.0f, Weight: 0.7f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.NickelIngot,
                "Nickel Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/NickelIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.NickelIngot, Name: "Nickel Ingot", Worth: 17.0f, Weight: 0.9f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.BismuthIngot,
                "Bismuth Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/BismuthIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.BismuthIngot, Name: "Bismuth Ingot", Worth: 21.0f, Weight: 0.85f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.TitaniumIngot,
                "Titanium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/TitaniumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.TitaniumIngot, Name: "Titanium Ingot", Worth: 150.0f, Weight: 1.3f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.TungstenIngot,
                "Tungsten Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/TungstenIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.TungstenIngot, Name: "Tungsten Ingot", Worth: 110.0f, Weight: 1.5f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.UraniumIngot,
                "Uranium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/UraniumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.UraniumIngot, Name: "Uranium Ingot", Worth: 235.0f, Weight: 1.1f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.OsmiumIngot,
                "Osmium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/OsmiumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.OsmiumIngot, Name: "Osmium Ingot", Worth: 95.0f, Weight: 1.4f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.CobaltIngot,
                "Cobalt Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/CobaltIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.CobaltIngot, Name: "Cobalt Ingot", Worth: 58.0f, Weight: 0.9f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.ChromiumIngot,
                "Chromium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/ChromiumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.ChromiumIngot, Name: "Chromium Ingot", Worth: 50.0f, Weight: 1.0f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.PlatinumIngot,
                "Platinum Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/PlatinumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.PlatinumIngot, Name: "Platinum Ingot", Worth: 125.0f, Weight: 1.2f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.RainbowIngot,
                "Rainbow Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/RainbowIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.RainbowIngot, Name: "Rainbow Ingot", Worth: 170.0f, Weight: 0.35f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.MythrilIngot,
                "Mythril Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/MythrilIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.MythrilIngot, Name: "Mythril Ingot", Worth: 340.0f, Weight: 0.5f)
            );
            AddDefinition(
                GameIds.Items.CratingMaterials.AdamantiumIngot,
                "Adamantium Ingot",
                LoadSingleTexture(manager, "CraftingMaterials/AdamantiumIngot"),
                new Item(ID: GameIds.Items.CratingMaterials.AdamantiumIngot, Name: "Adamantium Ingot", Worth: 680.0f, Weight: 0.65f),
                frames: 4
            );
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
            return new Grid(Vector2.Zero, new GridBox[columns, rows]);
        }

        private static AType CreateCopy(AType definition)
        {
            return definition switch
            {
                Item item => new Item(item),
                SmallDynamite dynamite => new SmallDynamite(dynamite),
                MediumDynamite mediumDynamite => new MediumDynamite(mediumDynamite),
                LargeDynamite largeDynamite => new LargeDynamite(largeDynamite),
                NukeDynamite nukeDynamite => new NukeDynamite(nukeDynamite),
                SmallFuelCapsule smallFuelCapsule => new SmallFuelCapsule(smallFuelCapsule),
                MediumFuelCapsule mediumFuelCapsule => new MediumFuelCapsule(mediumFuelCapsule),
                LargeFuelCapsule largeFuelCapsule => new LargeFuelCapsule(largeFuelCapsule),
                SmallCoolantPatch smallCoolantPatch => new SmallCoolantPatch(smallCoolantPatch),
                MediumCoolantPatch mediumCoolantPatch => new MediumCoolantPatch(mediumCoolantPatch),
                LargeCoolantPatch largeCoolantPatch => new LargeCoolantPatch(largeCoolantPatch),
                SmallHullRepairKit smallHullRepairKit => new SmallHullRepairKit(smallHullRepairKit),
                MediumHullRepairKit mediumHullRepairKit => new MediumHullRepairKit(mediumHullRepairKit),
                LargeHullRepairKit largeHullRepairKit => new LargeHullRepairKit(largeHullRepairKit),
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


