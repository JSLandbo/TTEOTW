using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopOverlay(EquipmentShopService equipmentShopService, WorldElementsRepository blocks, GameItemsRepository items) : IInteractionOverlay
    {
        private readonly EquipmentShopInteractionController interactionController = new();
        private readonly EquipmentShopRenderer renderer = new(blocks, items);

        private ABuilding building = null!;
        private EquipmentShopLayout currentLayout;
        private bool isOpen;
        private Point mousePosition;

        public EBuildingInteraction Action => EBuildingInteraction.EquipmentShop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public void Open(ABuilding building)
        {
            this.building = building;
            isOpen = true;
            currentLayout = default;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            renderer.LoadContent(graphicsDevice, content);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            if (interactionController.ShouldClose(currentKeyboardState, previousKeyboardState))
            {
                isOpen = false;
                return;
            }

            if (building?.StorageGrid == null)
            {
                return;
            }

            mousePosition = currentMouseState.Position;
            EnsureLayout(viewportWidth, viewportHeight);
            interactionController.TryHandleBuy(currentMouseState, previousMouseState, currentLayout, world, building, equipmentShopService);
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen || building?.StorageGrid == null)
            {
                return;
            }

            EnsureLayout(viewportWidth, viewportHeight);
            renderer.Draw(spriteBatch, world, building, currentLayout, viewportWidth, viewportHeight, mousePosition);
        }

        private void EnsureLayout(int viewportWidth, int viewportHeight)
        {
            currentLayout = EquipmentShopLayout.Create(viewportWidth, viewportHeight, building.StorageGrid.InternalGrid);
        }
    }
}
