using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Concrete;

namespace ToTheEndOfTheWorld.UI
{
    public interface IGameOverlay
    {
        bool IsOpen { get; }
        bool BlocksGameplay { get; }

        void LoadContent(GraphicsDevice graphicsDevice, ContentManager content);
        void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, World world, int viewportWidth, int viewportHeight);
        void Draw(SpriteBatch spriteBatch, World world, int viewportWidth, int viewportHeight);
    }
}
