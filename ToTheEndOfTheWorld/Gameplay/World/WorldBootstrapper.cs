namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBootstrapper(
        SellShopBuildingFactory sellShopBuildingFactory,
        EquipmentShopBuildingFactory equipmentShopBuildingFactory,
        FuelStationBuildingFactory fuelStationBuildingFactory,
        GadgetShopBuildingFactory gadgetShopBuildingFactory,
        StorageChestBuildingFactory storageChestBuildingFactory,
        HouseInfoSignBuildingFactory houseInfoSignBuildingFactory)
    {
        public void EnsureInitialized(ModelWorld world)
        {
            if (world.Buildings is { Count: > 0 })
            {
                return;
            }

            world.Buildings ??= [];

            long spawnX = (long)world.SpawnWorldPosition.X;
            long spawnY = (long)world.SpawnWorldPosition.Y;
            long shopX = spawnX + 6;
            long shopY = spawnY - 1;
            long equipmentShopX = spawnX - 8;
            long equipmentShopY = spawnY - 1;
            long fuelStationX = spawnX + 12;
            long fuelStationY = spawnY - 1;
            long gadgetShopX = spawnX + 16;
            long gadgetShopY = spawnY - 2;
            long storageChestX = spawnX - 4;
            long storageChestY = spawnY - 1;
            long houseInfoSignX = spawnX + 2;
            long houseInfoSignY = spawnY - 1;

            world.Buildings.Add(sellShopBuildingFactory.Create(shopX, shopY));
            world.Buildings.Add(equipmentShopBuildingFactory.Create(equipmentShopX, equipmentShopY));
            world.Buildings.Add(fuelStationBuildingFactory.Create(fuelStationX, fuelStationY));
            world.Buildings.Add(gadgetShopBuildingFactory.Create(gadgetShopX, gadgetShopY));
            world.Buildings.Add(storageChestBuildingFactory.Create(storageChestX, storageChestY));
            world.Buildings.Add(houseInfoSignBuildingFactory.Create(houseInfoSignX, houseInfoSignY));
        }
    }
}
