using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ToTheEndOfTheWorld.UI
{
    public interface IGameOverlay
    {
        bool IsOpen { get; }
        bool BlocksGameplay { get; }

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);
        void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight);
        void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight);
        bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight) => false;
    }
}
