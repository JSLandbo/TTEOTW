using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Enums;
using System;
using System.Collections.Generic;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using ToTheEndOfTheWorld.Gameplay;
using ToTheEndOfTheWorld.Gameplay.Events;
using ToTheEndOfTheWorld.UI;

namespace ToTheEndOfTheWorld
{
    public class MainGame : Game
    {
        private static readonly string GameTitle = "To The End Of The World";
        private static readonly string GameVersion = "V0.01";
        private static readonly int _pixels = 64;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private WorldInteractionsRepository interactions;
        private WorldElementsRepository blocks;
        private GameItemsRepository items;
        private World world;
        private readonly PlayerInputMapper inputMapper = new();
        private readonly PlayerFacingResolver playerFacingResolver = new();
        private readonly PlayerMovementSystem playerMovementSystem = new();
        private readonly WorldViewportService worldViewportService = new();
        private readonly GameEventBus eventBus = new();
        private readonly InventoryService inventoryService = new();
        private WorldQueryService worldQueryService;
        private PlayerWorldMovementResolver playerWorldMovementResolver;
        private PlayerMiningSystem playerMiningSystem;
        private CraftingService craftingService;
        private WorldBlockLootSystem worldBlockLootSystem;
        private UiManager uiManager;
        private SpriteFont textFont;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        // private ItemSpriteRepository _items;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Exiting += (_, _) =>
            {
                if (world != null)
                {
                    ContextHandler.SaveWorld(world);
                }
            };
        }

        protected override void Initialize()
        {
            blocks = new WorldElementsRepository(Content);
            items = new GameItemsRepository(Content);
            worldQueryService = new WorldQueryService(blocks);
            craftingService = new CraftingService();
            worldBlockLootSystem = new WorldBlockLootSystem(eventBus, new BlockLootResolver(blocks), inventoryService);
            uiManager = UiComposition.Create(inventoryService, craftingService, blocks, items);

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
            playerWorldMovementResolver = new PlayerWorldMovementResolver(worldQueryService, worldViewportService, _pixels);
            playerMiningSystem = new PlayerMiningSystem(worldQueryService, interactions, eventBus, _pixels);

            world = ContextHandler.LoadWorld();

            world ??= CreateNewWorld(_blocksWide, _blocksHigh);
            world.BlocksWide = _blocksWide;
            world.BlocksHigh = _blocksHigh;
            worldViewportService.EnsurePadding(world);
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        private World CreateNewWorld(int _blocksWide, int _blocksHigh)
        {
            var player = new Player(
                Engine: new Engine(items[3].type as Engine),
                Hull: new Hull(items[1].type as Hull),
                Drill: new Drill(items[2].type as Drill),
                Inventory: new Inventory(ID: 100, new Grid(new Vector2(0, 0), new GridBox[3, 3]), SizeLimit: 576, Name: "Starter Inventory", Worth: 10, Weight: 0),
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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textFont = Content.Load<SpriteFont>("Fonts/text");
            uiManager.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            uiManager.Update(gameTime, keyboardState, previousKeyboardState, mouseState, previousMouseState, world, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            if (uiManager.BlocksGameplay)
            {
                previousKeyboardState = keyboardState;
                previousMouseState = mouseState;
                base.Update(gameTime);
                return;
            }

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var intent = inputMapper.ReadPlayerIntent(keyboardState, previousKeyboardState);

            var player = world.Player;
            var facingDirection = playerFacingResolver.Resolve(player, intent);
            player.ApplyIntent(intent.MovementInput, facingDirection);
            playerMovementSystem.Update(player, deltaTime);
            playerWorldMovementResolver.Resolve(world, player);
            playerMiningSystem.Update(world, player);

            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            DrawRenderedWorld();
            //DrawRenderedBuildings();
            DrawPlayerShip();
            DrawStatistics();
            uiManager.Draw(spriteBatch, world, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawStatistics()
        {
            var player = world.Player;
            var worldPosition = world.WorldRender[new Vector2(player.Coordinates.X, player.Coordinates.Y)];
            spriteBatch.DrawString(textFont, $"World position: X: {worldPosition.X}, Y: {worldPosition.Y}", new Vector2(5, 5), Color.Black);
            spriteBatch.DrawString(textFont, $"Player velocity: X: {player.XVelocity}, Y: {player.YVelocity}", new Vector2(5, 25), Color.Black);
            spriteBatch.DrawString(textFont, $"Player offset: X: {player.XOffset}, Y: {player.YOffset}", new Vector2(5, 45), Color.Black);
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
                    spriteBatch.Draw(blocks[-1].Texture, location, Color.White);
                }
                else
                {
                    var block = worldQueryService.GetWorldBlock(pair.Value.X, pair.Value.Y);

                    spriteBatch.Draw(block.Value.Texture, location, Color.White);

                    if (interactions.TryGet(new WorldTile((long)pair.Value.X, (long)pair.Value.Y), WorldInteractionType.Mining, out var interaction))
                    {
                        var percentDamaged = interaction.Block.PercentDamaged();

                        spriteBatch.Draw(blocks[-2].Texture, location, Color.White * percentDamaged);
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
            var drillExtended = player.DrillExtended;
            var drill = items[player.Drill.ID];
            var hull = items[player.Hull.ID];

            if (orientation.Equals(PlayerOrientation.Base))
            {
                spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], PlayerPosition, Color.White);
            }
            else
            {
                if (drillExtended)
                {
                    spriteBatch.Draw(hull.Textures[orientation], PlayerPosition, Color.White);

                    var drillPositionX = PlayerPosition.X + (player.FacingDirection.X * _pixels);
                    var drillPositionY = PlayerPosition.Y + (player.FacingDirection.Y * _pixels);

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

    }
}
