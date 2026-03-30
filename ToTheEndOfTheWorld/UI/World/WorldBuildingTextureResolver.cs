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

        public void LoadContent(ContentManager content)
        {
            sellShopTexture = content.Load<Texture2D>("Buildings/HouseSellYourOres");
            equipmentShopTexture = content.Load<Texture2D>("Buildings/HouseBuyUpgrades");
            fuelStationTexture = content.Load<Texture2D>("Buildings/HouseFuelStation");
        }

        public Texture2D Resolve(ABuilding building)
        {
            return building.Interaction switch
            {
                EBuildingInteraction.Shop => sellShopTexture,
                EBuildingInteraction.EquipmentShop => equipmentShopTexture,
                EBuildingInteraction.FuelStation => fuelStationTexture,
                _ => null
            };
        }
    }
}
