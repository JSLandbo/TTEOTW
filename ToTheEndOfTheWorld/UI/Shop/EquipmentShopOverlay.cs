using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
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

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return TryGetHoveredItem(mousePosition, viewportWidth, viewportHeight, out AType hoveredItem)
                && hoveredItem != null
                && world.Player.Cash >= hoveredItem.Worth;
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return TryGetHoveredItem(mousePosition, viewportWidth, viewportHeight, out AType hoveredItem)
                ? hoveredItem?.Name
                : null;
        }

        private bool TryGetHoveredItem(Point mousePosition, int viewportWidth, int viewportHeight, out AType item)
        {
            if (building?.StorageGrid?.InternalGrid == null)
            {
                item = null;
                return false;
            }

            EnsureLayout(viewportWidth, viewportHeight);
            AGridBox[,] grid = building.StorageGrid.InternalGrid;

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (!currentLayout.GetSlotRectangle(x, y).Contains(mousePosition))
                    {
                        continue;
                    }

                    item = grid[x, y].Item;
                    return true;
                }
            }

            item = null;
            return false;
        }
    }
}
