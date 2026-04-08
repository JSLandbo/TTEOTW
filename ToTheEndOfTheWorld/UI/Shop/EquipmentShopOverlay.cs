using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopOverlay(EquipmentShopService equipmentShopService, WorldElementsRepository blocks, GameItemsRepository items, Func<bool> hasHeldItem) : IInteractionOverlay
    {
        private readonly EquipmentShopInteractionController interactionController = new();
        private readonly EquipmentShopRenderer renderer = new(blocks, items);

        private ABuilding building = null!;
        private EquipmentShopLayout currentLayout;
        private bool isOpen;
        private Point mousePosition;
        private int panelOffsetX;
        private int cachedViewportWidth;
        private int cachedViewportHeight;

        public EBuildingInteraction Action => EBuildingInteraction.EquipmentShop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public int PanelWidth => currentLayout.PanelRectangle.Width;

        public void Open(ABuilding building, int viewportWidth, int viewportHeight)
        {
            this.building = building;
            panelOffsetX = 0;
            isOpen = true;
            cachedViewportWidth = viewportWidth;
            cachedViewportHeight = viewportHeight;
            currentLayout = EquipmentShopLayout.Create(viewportWidth, viewportHeight, building.StorageInventory.Items.InternalGrid, panelOffsetX);
        }

        public void SetPanelOffset(int offsetX)
        {
            panelOffsetX = offsetX;
            currentLayout = EquipmentShopLayout.Create(cachedViewportWidth, cachedViewportHeight, building.StorageInventory.Items.InternalGrid, panelOffsetX);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            renderer.LoadContent(graphicsDevice, content);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen || building?.StorageInventory?.Items == null) return;

            if (viewportWidth != cachedViewportWidth || viewportHeight != cachedViewportHeight)
            {
                cachedViewportWidth = viewportWidth;
                cachedViewportHeight = viewportHeight;
                currentLayout = EquipmentShopLayout.Create(viewportWidth, viewportHeight, building.StorageInventory.Items.InternalGrid, panelOffsetX);
            }

            mousePosition = currentMouseState.Position;

            if (hasHeldItem()) return;

            interactionController.TryHandleBuy(currentMouseState, previousMouseState, currentLayout, world, building, equipmentShopService);
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen || building?.StorageInventory?.Items == null)
            {
                return;
            }

            renderer.Draw(spriteBatch, world, building, currentLayout, viewportWidth, viewportHeight, mousePosition);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (hasHeldItem()) return false;
            return TryGetHoveredItem(mousePosition, viewportWidth, viewportHeight, out AType hoveredItem)
                && world.Player.Cash >= hoveredItem.Worth;
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return TryGetHoveredItem(mousePosition, viewportWidth, viewportHeight, out AType hoveredItem)
                ? $"{hoveredItem.Name} {hoveredItem.Worth:0}"
                : null;
        }

        private bool TryGetHoveredItem(Point mousePosition, int viewportWidth, int viewportHeight, out AType item)
        {
            if (building?.StorageInventory?.Items?.InternalGrid == null)
            {
                item = null;
                return false;
            }

            AGridBox[,] grid = building.StorageInventory.Items.InternalGrid;
            bool found = UiGridHitTestHelper.TryGetCoordinates(grid.GetLength(0), grid.GetLength(1), mousePosition, currentLayout.GetSlotRectangle, out int slotX, out int slotY);
            item = found ? grid[slotX, slotY].Item : null;
            return item != null;
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }
    }
}
