using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Buildings;
using ModelLibrary.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldBootstrapper
    {
        private readonly WorldViewportService worldViewportService;
        private readonly EquipmentShopBuildingFactory equipmentShopBuildingFactory;

        public WorldBootstrapper(WorldViewportService worldViewportService, EquipmentShopBuildingFactory equipmentShopBuildingFactory)
        {
            this.worldViewportService = worldViewportService;
            this.equipmentShopBuildingFactory = equipmentShopBuildingFactory;
        }

        public void EnsureInitialized(World world)
        {
            world.Buildings ??= new List<ABuilding>();
            EnsureStarterBuildings(world);
        }

        private void EnsureStarterBuildings(World world)
        {
            var centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            EnsureSellShop(world, centerWorldPosition);
            EnsureEquipmentShop(world, centerWorldPosition);
        }

        private static ABuilding GetBuilding(World world, EBuildingInteraction interaction)
        {
            foreach (var building in world.Buildings)
            {
                if (building.Interaction == interaction)
                {
                    return building;
                }
            }

            return null;
        }

        private static void EnsureSellShop(World world, Vector2 centerWorldPosition)
        {
            var existingShop = GetBuilding(world, EBuildingInteraction.Shop);

            if (existingShop != null)
            {
                existingShop.Name = "Shop";
                existingShop.InteractionPrompt = "Press E to open shop";
                return;
            }

            var shopX = (long)centerWorldPosition.X + 6;
            var shopY = (long)centerWorldPosition.Y - 2;

            world.Buildings.Add(new Building(
                ID: 1,
                Name: "Shop",
                WorldX: shopX,
                WorldY: shopY,
                TilesWide: 2,
                TilesHigh: 3,
                StorageGrid: null,
                IsBackground: true,
                IsDestructible: false,
                Interaction: EBuildingInteraction.Shop,
                InteractionPrompt: "Press E to open shop"));
        }

        private void EnsureEquipmentShop(World world, Vector2 centerWorldPosition)
        {
            var existingShop = GetBuilding(world, EBuildingInteraction.EquipmentShop);

            if (existingShop != null)
            {
                existingShop.Name = "Equipment Shop";
                existingShop.InteractionPrompt = "Press E to open equipment shop";
                equipmentShopBuildingFactory.EnsureStorage(existingShop);
                return;
            }

            var shopX = (long)centerWorldPosition.X - 8;
            var shopY = (long)centerWorldPosition.Y - 1;

            world.Buildings.Add(equipmentShopBuildingFactory.Create(shopX, shopY));
        }
    }
}
