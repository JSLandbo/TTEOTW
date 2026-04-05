using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Inventory;

namespace ToTheEndOfTheWorld.UI
{
    public sealed class UiManager
    {
        private readonly List<IGameOverlay> overlays = [];
        private readonly UiHoverLabelRenderer hoverLabelRenderer = new();
        private Point lastMousePosition;
        private bool interactionOpenedInventory;

        public bool BlocksGameplay
        {
            get
            {
                foreach (IGameOverlay overlay in overlays)
                {
                    if (overlay.IsOpen && overlay.BlocksGameplay)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasOpenInteractionOverlay
        {
            get
            {
                foreach (IGameOverlay overlay in overlays)
                {
                    if (overlay is IInteractionOverlay && overlay.IsOpen)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool InventoryHasHeldItem => GetOverlay<InventoryOverlay>()?.HasHeldItem ?? false;

        public bool IsShopSellModeActive => GetOverlay<Shop.ShopOverlay>()?.IsSellModeActive ?? false;

        public bool TryShopSellSlot(ModelWorld world, AGridBox slot)
        {
            return GetOverlay<Shop.ShopOverlay>()?.TrySellSlot(world, slot) ?? false;
        }

        public void Register(IGameOverlay overlay)
        {
            overlays.Add(overlay);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            hoverLabelRenderer.LoadContent(graphicsDevice, content);
            foreach (IGameOverlay overlay in overlays)
            {
                overlay.LoadContent(graphicsDevice, content);
            }
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            lastMousePosition = currentMouseState.Position;
            foreach (IGameOverlay overlay in overlays)
            {
                overlay.Update(gameTime, currentKeyboardState, previousKeyboardState, currentMouseState, previousMouseState, world, viewportWidth, viewportHeight);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            foreach (IGameOverlay overlay in overlays)
            {
                overlay.Draw(spriteBatch, world, viewportWidth, viewportHeight);
            }

            string hoverLabel = GetHoverLabel(world, viewportWidth, viewportHeight);
            if (!string.IsNullOrWhiteSpace(hoverLabel))
            {
                hoverLabelRenderer.Draw(spriteBatch, hoverLabel, lastMousePosition, viewportWidth, viewportHeight);
            }
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            foreach (IGameOverlay overlay in overlays)
            {
                if (!overlay.IsOpen)
                {
                    continue;
                }

                if (overlay.IsPointerOverInteractiveElement(world, mousePosition, viewportWidth, viewportHeight))
                {
                    return true;
                }
            }

            return false;
        }

        private string GetHoverLabel(ModelWorld world, int viewportWidth, int viewportHeight)
        {
            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                IGameOverlay overlay = overlays[i];

                if (!overlay.IsOpen)
                {
                    continue;
                }

                string hoverLabel = overlay.GetHoverLabel(world, lastMousePosition, viewportWidth, viewportHeight);
                if (!string.IsNullOrWhiteSpace(hoverLabel))
                {
                    return hoverLabel;
                }
            }

            return null;
        }

        public bool Open(ABuilding building)
        {
            foreach (IGameOverlay overlay in overlays)
            {
                if (overlay is not IInteractionOverlay interactionOverlay || interactionOverlay.Action != building.Interaction)
                {
                    continue;
                }

                interactionOverlay.Open(building);
                OpenInventoryForInteraction(building);
                return true;
            }

            return false;
        }

        public bool CloseTopmost(ModelWorld world)
        {
            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                IGameOverlay overlay = overlays[i];

                if (!overlay.IsOpen) continue;

                overlay.Close(world);

                if (overlay is IInteractionOverlay)
                {
                    CloseInventoryAfterInteraction(world);
                }

                return true;
            }

            return false;
        }

        public T? GetOverlay<T>() where T : class, IGameOverlay
        {
            foreach (IGameOverlay overlay in overlays)
            {
                if (overlay is T typedOverlay)
                {
                    return typedOverlay;
                }
            }

            return null;
        }

        private void OpenInventoryForInteraction(ABuilding building)
        {
            InventoryOverlay inventoryOverlay = GetOverlay<InventoryOverlay>();

            interactionOpenedInventory = inventoryOverlay != null && building.ShowPlayerInventoryWhenOpen;

            if (!interactionOpenedInventory)
            {
                return;
            }

            inventoryOverlay.Open(UiOverlayLayout.InventoryWithShopPanelOffsetX);
        }

        private void CloseInventoryAfterInteraction(ModelWorld world)
        {
            if (!interactionOpenedInventory)
            {
                return;
            }

            InventoryOverlay inventoryOverlay = GetOverlay<InventoryOverlay>();

            inventoryOverlay?.Close(world);

            interactionOpenedInventory = false;
        }
    }
}