using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.WorldRendering
{
    public sealed class WorldBuildingTextureResolver
    {
        private Texture2D sellShopTexture;
        private Texture2D equipmentShopTexture;

        public void LoadContent(ContentManager content)
        {
            sellShopTexture = content.Load<Texture2D>("Buildings/HouseSellYourOres");
            equipmentShopTexture = content.Load<Texture2D>("Buildings/HouseBuyUpgrades");
        }

        public Texture2D Resolve(ABuilding building)
        {
            return building.Interaction switch
            {
                EBuildingInteraction.Shop => sellShopTexture,
                EBuildingInteraction.EquipmentShop => equipmentShopTexture,
                _ => null
            };
        }
    }
}
