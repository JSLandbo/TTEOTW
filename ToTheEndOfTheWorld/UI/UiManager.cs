using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract;
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
        private short cachedInventoryId;

        public bool BlocksGameplay => overlays.Any(o => o.IsOpen && o.BlocksGameplay);

        public bool HasOpenInteractionOverlay => overlays.Any(o => o is IInteractionOverlay && o.IsOpen);

        public bool HasOpenShopOverlay => overlays.Any(o => o is IInteractionOverlay { IsOpen: true } and not Shop.StorageChestOverlay);

        public bool InventoryHasHeldItem => GetOverlay<InventoryOverlay>()?.HasHeldItem ?? false;

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

            var inventoryId = world.Player.Inventory.ID;

            if (inventoryId != cachedInventoryId)
            {
                cachedInventoryId = inventoryId;

                if (interactionOpenedInventory)
                {
                    RefreshShopAndInventoryLayout(world.Player, viewportWidth, viewportHeight);
                }
                else
                {
                    GetOverlay<InventoryOverlay>()?.RefreshLayout(world.Player);
                }
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
                if (!overlay.IsOpen) continue;

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

                if (!overlay.IsOpen) continue;

                string hoverLabel = overlay.GetHoverLabel(world, lastMousePosition, viewportWidth, viewportHeight);
                if (!string.IsNullOrWhiteSpace(hoverLabel))
                {
                    return hoverLabel;
                }
            }

            return null;
        }

        public bool Open(ABuilding building, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            foreach (IGameOverlay overlay in overlays)
            {
                if (overlay is not IInteractionOverlay interactionOverlay || interactionOverlay.Action != building.Interaction)
                {
                    continue;
                }

                interactionOverlay.Open(building, viewportWidth, viewportHeight);

                if (building.ShowPlayerInventoryWhenOpen)
                {
                    InventoryOverlay inventoryOverlay = GetOverlay<InventoryOverlay>();
                    if (inventoryOverlay != null)
                    {
                        inventoryOverlay.Open(viewportWidth, viewportHeight, world.Player);
                        cachedInventoryId = world.Player.Inventory.ID;

                        int shopWidth = interactionOverlay.PanelWidth;
                        int inventoryWidth = inventoryOverlay.PanelWidth;
                        int shopOffset = UiOverlayLayout.CalculateShopOffset(shopWidth, inventoryWidth);
                        int inventoryOffset = UiOverlayLayout.CalculateInventoryOffset(shopWidth, inventoryWidth);

                        interactionOverlay.SetPanelOffset(shopOffset);
                        inventoryOverlay.SetPanelOffset(inventoryOffset);
                        inventoryOverlay.RefreshLayout(world.Player);
                        interactionOpenedInventory = true;
                    }
                }

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

                if (overlay is IInteractionOverlay)
                {
                    CloseInventoryAfterInteraction(world);
                }

                overlay.Close(world);

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

        private void RefreshShopAndInventoryLayout(APlayer player, int viewportWidth, int viewportHeight)
        {
            cachedInventoryId = player.Inventory.ID;

            InventoryOverlay inventoryOverlay = GetOverlay<InventoryOverlay>();

            IInteractionOverlay openShop = null;
            foreach (IGameOverlay overlay in overlays)
            {
                if (overlay is IInteractionOverlay shop && shop.IsOpen)
                {
                    openShop = shop;
                    break;
                }
            }

            if (openShop == null || inventoryOverlay == null)
            {
                return;
            }

            inventoryOverlay.RefreshLayout(player);
            int shopWidth = openShop.PanelWidth;
            int inventoryWidth = inventoryOverlay.PanelWidth;
            int shopOffset = UiOverlayLayout.CalculateShopOffset(shopWidth, inventoryWidth);
            int inventoryOffset = UiOverlayLayout.CalculateInventoryOffset(shopWidth, inventoryWidth);

            openShop.SetPanelOffset(shopOffset);
            inventoryOverlay.SetPanelOffset(inventoryOffset);
            inventoryOverlay.RefreshLayout(player);
        }
    }
}
