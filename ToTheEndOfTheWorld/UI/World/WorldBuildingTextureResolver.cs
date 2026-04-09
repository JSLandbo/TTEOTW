using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class WorldBuildingTextureResolver
    {
        private Texture2D sellShopTexture;
        private Texture2D equipmentShopTexture;
        private Texture2D fuelStationTexture;
        private Texture2D gadgetShopTexture;
        private Texture2D storageChestTexture;
        private Texture2D houseInfoSignTexture;

        public void LoadContent(ContentManager content)
        {
            sellShopTexture = content.Load<Texture2D>("Buildings/HouseSellYourOres");
            equipmentShopTexture = content.Load<Texture2D>("Buildings/HouseBuyUpgrades");
            fuelStationTexture = content.Load<Texture2D>("Buildings/HouseFuelStation");
            gadgetShopTexture = content.Load<Texture2D>("Buildings/HouseGadgets");
            storageChestTexture = content.Load<Texture2D>("Buildings/HouseStorageChest");
            houseInfoSignTexture = content.Load<Texture2D>("Buildings/HouseInfoSign");
        }

        public Texture2D Resolve(ABuilding building)
        {
            return building.Interaction switch
            {
                EBuildingInteraction.Shop => sellShopTexture,
                EBuildingInteraction.EquipmentShop => equipmentShopTexture,
                EBuildingInteraction.FuelStation => fuelStationTexture,
                EBuildingInteraction.GadgetShop => gadgetShopTexture,
                EBuildingInteraction.StorageChest => storageChestTexture,
                EBuildingInteraction.HouseInfoSign => houseInfoSignTexture,
                _ => throw new InvalidOperationException($"No building texture registered for interaction '{building.Interaction}'.")
            };
        }
    }
}
