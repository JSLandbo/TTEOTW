using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ToTheEndOfTheWorld.UI.Common;

namespace ToTheEndOfTheWorld.UI
{
    public sealed class UiManager
    {
        private readonly List<IGameOverlay> overlays = [];
        private readonly UiHoverLabelRenderer hoverLabelRenderer = new();
        private Point lastMousePosition;

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
                hoverLabelRenderer.Draw(spriteBatch, hoverLabel, viewportWidth);
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
                return true;
            }

            return false;
        }

        public bool CloseTopmost(ModelWorld world)
        {
            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                IGameOverlay overlay = overlays[i];
                if (!overlay.IsOpen)
                {
                    continue;
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
    }
}
