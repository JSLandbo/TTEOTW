namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBootstrapper
    {
        private readonly SellShopBuildingFactory sellShopBuildingFactory;
        private readonly EquipmentShopBuildingFactory equipmentShopBuildingFactory;
        private readonly FuelStationBuildingFactory fuelStationBuildingFactory;

        public WorldBootstrapper(
            SellShopBuildingFactory sellShopBuildingFactory,
            EquipmentShopBuildingFactory equipmentShopBuildingFactory,
            FuelStationBuildingFactory fuelStationBuildingFactory)
        {
            this.sellShopBuildingFactory = sellShopBuildingFactory;
            this.equipmentShopBuildingFactory = equipmentShopBuildingFactory;
            this.fuelStationBuildingFactory = fuelStationBuildingFactory;
        }

        public void EnsureInitialized(ModelWorld world)
        {
            world.Buildings ??= [];
            world.Buildings.Clear();

            long spawnX = (long)world.SpawnWorldPosition.X;
            long spawnY = (long)world.SpawnWorldPosition.Y;
            long shopX = spawnX + 6;
            long shopY = spawnY - 1;
            long equipmentShopX = spawnX - 8;
            long equipmentShopY = spawnY - 1;
            long fuelStationX = spawnX + 12;
            long fuelStationY = spawnY - 1;

            world.Buildings.Add(sellShopBuildingFactory.Create(shopX, shopY));
            world.Buildings.Add(equipmentShopBuildingFactory.Create(equipmentShopX, equipmentShopY));
            world.Buildings.Add(fuelStationBuildingFactory.Create(fuelStationX, fuelStationY));
        }
    }
}
