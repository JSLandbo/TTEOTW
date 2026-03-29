using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using ModelLibrary.Abstract;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Enums;
using System.Linq;
using UtilityLibrary;
using ModelLibrary.Context;
using System.Threading.Tasks;
using System.Threading;

namespace ToTheEndOfTheWorld
{
    public class MainGame : Game
    {
        private static readonly string GameTitle = "To The End Of The World";
        private static readonly string GameVersion = "V0.01";
        private static readonly int _pixels = 64;
        private static readonly float _tileTransitionOffset = _pixels * 0.5f;
        private const float _collisionPlacementOffset = 0.0f;
        private static readonly float _miningCenterTolerance = _pixels * 0.2f;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private WorldInteractionsRepository interactions;
        private WorldElementsRepository blocks;
        private GameItemsRepository items;
        private World world;
        private KeyboardState previousKeyboardState;

        // private ItemSpriteRepository _items;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override async void Initialize()
        {
            blocks = new WorldElementsRepository(Content);
            items = new GameItemsRepository(Content);

            var _blocksWide = (GraphicsDevice.DisplayMode.Width - (GraphicsDevice.DisplayMode.Width % _pixels)) / _pixels;
            var _blocksHigh = (GraphicsDevice.DisplayMode.Height - (GraphicsDevice.DisplayMode.Height % _pixels)) / _pixels;

            _blocksWide -= _blocksWide % 2 + 2;
            _blocksHigh -= _blocksHigh % 2 + 2;

            Window.Title = $"{GameTitle} {GameVersion}";
            graphics.PreferredBackBufferWidth = _blocksWide * _pixels;
            graphics.PreferredBackBufferHeight = _blocksHigh * _pixels;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            interactions = new WorldInteractionsRepository();

            world = ContextHandler.LoadWorld();

            world ??= CreateNewWorld(_blocksWide, _blocksHigh);
            world.BlocksWide = _blocksWide;
            world.BlocksHigh = _blocksHigh;
            EnsureWorldRenderPadding();
            previousKeyboardState = Keyboard.GetState();

            base.Initialize();
        }

        private World CreateNewWorld(int _blocksWide, int _blocksHigh)
        {
            var player = new Player(
                Engine: new Engine(items[3].type as Engine),
                Hull: new Hull(items[1].type as Hull),
                Drill: new Drill(items[2].type as Drill),
                Inventory: new Inventory(ID: 100, new Grid(ID: 99, new Vector2(0, 0), new GridBox[3, 3]), SizeLimit: 576, Name: "Starter Inventory", Worth: 10, Weight: 0),
                Thruster: new Thruster(items[5].type as Thruster),
                FuelTank: new FuelTank(items[4].type as FuelTank)
            )
            {
                Coordinates = new Vector2((float)Math.Floor(_blocksWide / 2.0d), (float)Math.Floor(_blocksHigh / 2.0d))
            };

            var createdWorldRender = new Dictionary<Vector2, Vector2>();

            for (var x = 0; x <= _blocksWide; x++)
            {
                for (var y = 0; y <= _blocksHigh; y++)
                {
                    createdWorldRender.Add(new Vector2(x, y), new Vector2(x, y));
                }
            }

            return new World(
                Player: player,                                  // ContextHandler.LoadPlayer();
                Buildings: null,                                 // ContextHandler.LoadBuildings();
                BlocksWide: _blocksWide,                         // Calculated
                BlocksHigh: _blocksHigh,                         // Calculated
                WorldRender: createdWorldRender,                 // Dynamically updated
                WorldTrails: new Dictionary<Vector2, bool>()     // ContextHandler.LoadWorldTrails();
            );
        }

        protected override async void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override async void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (state.IsKeyDown(Keys.LeftControl) && state.IsKeyDown(Keys.S))
            {
                ContextHandler.SaveWorld(world);
            }

            var player = world.Player;
            player.UpdateInput(state, previousKeyboardState);
            player.UpdateVelocity(deltaTime);
            player.UpdateOffset(deltaTime);
            ResolvePlayerMovement(player);
            UpdateMining(player);

            previousKeyboardState = state;

            base.Update(gameTime);
        }

        protected override async void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            DrawRenderedWorld();
            //DrawRenderedBuildings();
            DrawPlayerShip();
            DrawStatistics();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStatistics()
        {
            var font = Content.Load<SpriteFont>("Fonts/text");
            var first = world.WorldRender.OrderBy(x => x.Key.X).OrderBy(x => x.Key.Y).FirstOrDefault();
            var player = world.Player;
            spriteBatch.DrawString(font, $"World position: X: {first.Value.X}, Y: {first.Value.Y}", new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(font, $"Player velocity: X: {player.XVelocity}, Y: {player.YVelocity}", new Vector2(5, 25), Color.Black);
            spriteBatch.DrawString(font, $"Player offset: X: {player.XOffset}, Y: {player.YOffset}", new Vector2(5, 45), Color.Black);
        }

        private void DrawRenderedWorld()
        {
            foreach (var pair in world.WorldRender)
            {
                var player = world.Player;
                var location = new Vector2(
                    pair.Key.X * _pixels - (0.5f * _pixels),
                    pair.Key.Y * _pixels - (0.5f * _pixels)
                );

                location.X -= player.XOffset;
                location.Y -= player.YOffset;


                if (world.WorldTrails.ContainsKey(pair.Value))
                {
                    spriteBatch.Draw(blocks.First(x => x.Key == -1).Value.Texture, location, Color.White);
                }
                else
                {
                    var block = GetWorldBlock(pair.Value.X, pair.Value.Y);

                    spriteBatch.Draw(block.Value.Texture, location, Color.White);

                    if (interactions.ContainsKey(pair.Value))
                    {
                        var percentDamaged = interactions[pair.Value].PercentDamaged();

                        spriteBatch.Draw(blocks.First(x => x.Key == -2).Value.Texture, location, Color.White * percentDamaged);
                    }
                }
            }
        }

        private void DrawPlayerShip()
        {
            Vector2 PlayerPosition = new Vector2(
                GetCenterScreenCoordinates().X - (0.5f * _pixels),
                GetCenterScreenCoordinates().Y - (0.5f * _pixels)
            );

            var player = world.Player;
            var orientation = player.Orientation;
            var mining = player.Mining;
            var drill = items[player.Drill.ID];
            var hull = items[player.Hull.ID];

            if (orientation.Equals(PlayerOrientation.Base))
            {
                spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], PlayerPosition, Color.White);
            }
            else
            {
                if (mining)
                {
                    spriteBatch.Draw(hull.Textures[orientation], PlayerPosition, Color.White);

                    var drillPositionX = PlayerPosition.X + (player.Direction.X * _pixels) - player.XOffset;
                    var drillPositionY = PlayerPosition.Y + (player.Direction.Y * _pixels) - player.YOffset;

                    spriteBatch.Draw(drill.Textures[orientation], new Vector2(drillPositionX, drillPositionY), Color.White);
                }
                else
                {
                    spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], PlayerPosition, Color.White);
                }

                if (player.MaximumActiveVelocity > 0)
                {
                    // draw thrusters
                }
            }
        }

        private Vector2 GetCenterScreenCoordinates()
        {
            return new Vector2(
                (float)(graphics.PreferredBackBufferWidth / 2.0),
                (float)(graphics.PreferredBackBufferHeight / 2.0)
            );
        }

        private void DealDamageToBlock(float x, float y)
        {
            var vector = new Vector2(x, y);

            if (world.WorldTrails.ContainsKey(vector))
            {
                return;
            }

            var block = GetWorldBlock(x, y).Value.Block;

            if (interactions.ContainsKey(vector) == false)
            {
                block.OnBlockDestroyed += (sender, e) => OnBlockDestroyed(sender as Block, e, vector);
                interactions.Add(vector, block);
            }

            if (interactions[vector].Hardness <= world.Player.Drill.Hardness)
            {
                interactions[vector].TakeDamage(world.Player.Drill.Damage);
            }
        }

        private void OnBlockDestroyed(Block block, EventArgs e, Vector2 location)
        {
            world.WorldTrails.Add(location, true);
            interactions.Remove(location);
        }

        private KeyValuePair<int, (string Name, Texture2D Texture, Block Block)> GetWorldBlock(float x, float y)
        {
            var simplex = (float)SimplexNoise.Singleton.Noise01(x, y) * 100.0f;

            foreach (var block in blocks.OrderByDescending(e => e.Key))
            {
                var info = block.Value.block.Info;

                if (y > info.MaximumDepth || y < info.MinimumDepth)
                {
                    continue;
                }

                if (simplex >= info.OccurrenceSpan.X && simplex <= info.OccurrenceSpan.Y)
                {
                    var keyValuePair = new KeyValuePair<int, (string Name, Texture2D Texture, Block Block)>
                    (
                        block.Key, (block.Value.Name, block.Value.Texture, new Block(block.Value.block))
                    );

                    if (block.Key == 2 && x > 0) // Dirt gets compressed slowly
                    {
                        //keyValuePair.Value.Block.Hardness += 0.01f * x; // TODO: Implement this.
                        keyValuePair.Value.Block.CurrentHealth += 0.01f * x;
                        keyValuePair.Value.Block.MaximumHealth += 0.01f * x;
                    }
                    return keyValuePair;
                }
            }

            return new KeyValuePair<int, (string Name, Texture2D Texture, Block Block)>(-1, (null, null, null));
        }

        private bool Obstructed(Block block, Vector2 nextBlock)
        {
            if (block.Ethereal || world.WorldTrails.ContainsKey(nextBlock))
            {
                return false;
            }
            return true;
        }

        private void MoveScreen(float x, float y)
        {
            var updated = new Dictionary<Vector2, Vector2>();

            foreach (var block in world.WorldRender)
            {
                updated.Add(new Vector2(block.Key.X, block.Key.Y), new Vector2(block.Value.X + x, block.Value.Y + y));
            }

            world.WorldRender = updated;
        }

        private void EnsureWorldRenderPadding()
        {
            var playerKey = new Vector2(world.Player.Coordinates.X, world.Player.Coordinates.Y);
            var centerWorldPosition = world.WorldRender[playerKey];
            var paddedRender = new Dictionary<Vector2, Vector2>();

            for (var x = -1; x <= world.BlocksWide + 1; x++)
            {
                for (var y = -1; y <= world.BlocksHigh + 1; y++)
                {
                    var renderKey = new Vector2(x, y);
                    var worldLocation = new Vector2(
                        centerWorldPosition.X + (x - playerKey.X),
                        centerWorldPosition.Y + (y - playerKey.Y)
                    );

                    paddedRender[renderKey] = worldLocation;
                }
            }

            world.WorldRender = paddedRender;
        }

        private void ResolvePlayerMovement(APlayer player)
        {
            const int maxIterations = 8;

            for (var i = 0; i < maxIterations; i++)
            {
                var processedMovement = false;

                if (Math.Abs(player.XOffset) >= Math.Abs(player.YOffset))
                {
                    processedMovement |= TryProcessMovementAxis(player, horizontal: true);
                    processedMovement |= TryProcessMovementAxis(player, horizontal: false);
                }
                else
                {
                    processedMovement |= TryProcessMovementAxis(player, horizontal: false);
                    processedMovement |= TryProcessMovementAxis(player, horizontal: true);
                }

                if (!processedMovement)
                {
                    return;
                }
            }
        }

        private bool TryProcessMovementAxis(APlayer player, bool horizontal)
        {
            var offset = horizontal ? player.XOffset : player.YOffset;
            var direction = Math.Sign(offset);

            if (direction == 0)
            {
                return false;
            }

            if (IsAxisObstructed(player, horizontal, direction))
            {
                if (horizontal)
                {
                    player.XOffset = direction * _collisionPlacementOffset;
                    player.XVelocity = 0.0f;
                }
                else
                {
                    player.YOffset = direction * _collisionPlacementOffset;
                    player.YVelocity = 0.0f;
                }

                return true;
            }

            if (Math.Abs(offset) < _tileTransitionOffset)
            {
                return false;
            }

            MoveScreen(horizontal ? direction : 0, horizontal ? 0 : direction);

            if (horizontal)
            {
                player.XOffset -= direction * _pixels;
            }
            else
            {
                player.YOffset -= direction * _pixels;
            }

            return true;
        }

        private void UpdateMining(APlayer player)
        {
            player.Mining = false;

            if (player.FacingDirection == Vector2.Zero)
            {
                return;
            }

            if (Vector2.Dot(player.MovementInput, player.FacingDirection) <= 0.0f)
            {
                return;
            }

            if (!IsCenteredForMining(player))
            {
                return;
            }

            var location = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            var blockVector = new Vector2(location.X + player.FacingDirection.X, location.Y + player.FacingDirection.Y);
            var block = GetWorldBlock(blockVector.X, blockVector.Y).Value.Block;

            if (!Obstructed(block, blockVector))
            {
                return;
            }

            SnapPlayerToMiningBlock(player);
            player.Mining = true;
            DealDamageToBlock(blockVector.X, blockVector.Y);
        }

        private void SnapPlayerToMiningBlock(APlayer player)
        {
            player.ResetVelocity();

            if (player.FacingDirection.X != 0)
            {
                player.XOffset = player.FacingDirection.X * _collisionPlacementOffset;
                player.YOffset = 0.0f;
                return;
            }

            if (player.FacingDirection.Y != 0)
            {
                player.XOffset = 0.0f;
                player.YOffset = player.FacingDirection.Y * _collisionPlacementOffset;
            }
        }

        private bool IsAxisObstructed(APlayer player, bool horizontal, int direction)
        {
            var location = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            var nextBlockVector = new Vector2(
                location.X + (horizontal ? direction : 0),
                location.Y + (horizontal ? 0 : direction)
            );
            var nextBlock = GetWorldBlock(nextBlockVector.X, nextBlockVector.Y).Value.Block;

            return Obstructed(nextBlock, nextBlockVector);
        }

        private bool IsCenteredForMining(APlayer player)
        {
            if (player.FacingDirection.X != 0)
            {
                return Math.Abs(player.YOffset) <= _miningCenterTolerance;
            }

            if (player.FacingDirection.Y != 0)
            {
                return Math.Abs(player.XOffset) <= _miningCenterTolerance;
            }

            return false;
        }
    }
}
