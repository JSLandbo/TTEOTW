using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldBootstrapper
    {
        private readonly WorldViewportService worldViewportService;
        private readonly SellShopBuildingFactory sellShopBuildingFactory;
        private readonly EquipmentShopBuildingFactory equipmentShopBuildingFactory;

        public WorldBootstrapper(
            WorldViewportService worldViewportService,
            SellShopBuildingFactory sellShopBuildingFactory,
            EquipmentShopBuildingFactory equipmentShopBuildingFactory)
        {
            this.worldViewportService = worldViewportService;
            this.sellShopBuildingFactory = sellShopBuildingFactory;
            this.equipmentShopBuildingFactory = equipmentShopBuildingFactory;
        }

        public void EnsureInitialized(World world)
        {
            world.Buildings ??= new List<ABuilding>();

            if (world.Buildings.Count > 0) return;

            var centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);

            var shopX = (long)centerWorldPosition.X + 6;
            var shopY = (long)centerWorldPosition.Y - 1;
            var equipmentShopX = (long)centerWorldPosition.X - 8;
            var equipmentShopY = (long)centerWorldPosition.Y - 1;

            world.Buildings.Add(sellShopBuildingFactory.Create(shopX, shopY));
            world.Buildings.Add(equipmentShopBuildingFactory.Create(equipmentShopX, equipmentShopY));
        }
    }
}
