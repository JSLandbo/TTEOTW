namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBootstrapper
    {
        private readonly WorldViewportService worldViewportService;
        private readonly SellShopBuildingFactory sellShopBuildingFactory;
        private readonly EquipmentShopBuildingFactory equipmentShopBuildingFactory;
        private readonly FuelStationBuildingFactory fuelStationBuildingFactory;

        public WorldBootstrapper(
            WorldViewportService worldViewportService,
            SellShopBuildingFactory sellShopBuildingFactory,
            EquipmentShopBuildingFactory equipmentShopBuildingFactory,
            FuelStationBuildingFactory fuelStationBuildingFactory)
        {
            this.worldViewportService = worldViewportService;
            this.sellShopBuildingFactory = sellShopBuildingFactory;
            this.equipmentShopBuildingFactory = equipmentShopBuildingFactory;
            this.fuelStationBuildingFactory = fuelStationBuildingFactory;
        }

        public void EnsureInitialized(ModelWorld world)
        {
            world.Buildings ??= [];
            world.Buildings.Clear();

            Microsoft.Xna.Framework.Vector2 centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);

            long shopX = (long)centerWorldPosition.X + 6;
            long shopY = (long)centerWorldPosition.Y - 1;
            long equipmentShopX = (long)centerWorldPosition.X - 8;
            long equipmentShopY = (long)centerWorldPosition.Y - 1;
            long fuelStationX = (long)centerWorldPosition.X + 12;
            long fuelStationY = (long)centerWorldPosition.Y - 1;

            world.Buildings.Add(sellShopBuildingFactory.Create(shopX, shopY));
            world.Buildings.Add(equipmentShopBuildingFactory.Create(equipmentShopX, equipmentShopY));
            world.Buildings.Add(fuelStationBuildingFactory.Create(fuelStationX, fuelStationY));
        }
    }
}
