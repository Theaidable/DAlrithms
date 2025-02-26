using DAlgorithms.Classes.Objects;
using DAlgorithms.Classes.UI;
using DAlgorithms.Classes.Algorithms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using SharpDX.Direct2D1.Effects;
using System;
using System.Reflection.Metadata;

namespace DAlgorithms.Classes.World
{
    /// <summary>
    /// GameWorld styrer spillets hovedløb og indeholder 
    /// tilemap, wizard, portal, towers, keys og UI-knapper.
    /// </summary>
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Reference til andre klasser
        private Portal portal;
        private Wizard wizard;
        private Button button;
        private Tower tower;
        private Key key;
        private TileMap tileMap;
        private Tile tile;

        //Objekter
        private Tower iceTower;
        private Tower stormTower;
        private Key iceKey;
        private Key stormKey;

        //Textures
        private Texture2D tileTexture;
        private Texture2D buttonTexture;
        private Texture2D pressedButtonTexture;
        private Texture2D restartIcon;
        private Texture2D aStarIcon;
        private Texture2D dfsIcon;
        private Texture2D iceTowerTexture;
        private Texture2D stormTowerTexture;
        private Texture2D[] iceTowerKeyTexture = new Texture2D[5];
        private Texture2D[] stormTowerKeyTexture = new Texture2D[12];
        private Texture2D[] portalTexture = new Texture2D[7];
        private Texture2D[] wizardIdleTexture = new Texture2D[4];
        private Texture2D[] wizardRunningTexture = new Texture2D[6];

        //Lists
        private List<Button> buttons = new List<Button>();
        private List<Key> keys = new List<Key>(); 

        /// <summary>
        /// GameWorld konstruktør, sætter isMouseVisible = true.
        /// </summary>
        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Kaldes, når spillet initialiserer.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// Indlæser content (teksturer osv.).
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadTextures();
            LoadTileMap();
            LoadButtons();
            LoadTowers();
            LoadKeys();
            LoadPortal();
            LoadWizard();
        }

        public void LoadTextures()
        {
            // Indlæs tile-texture (spritesheet)
            tileTexture = Content.Load<Texture2D>("Assets/Tiles/Tiles");
            
            //Buttons
            buttonTexture = Content.Load<Texture2D>("Assets/Buttons/Button_Blue");
            pressedButtonTexture = Content.Load<Texture2D>("Assets/Buttons/Button_Blue_Pressed");
            //restartIcon = content.Load<Texture2D>("Assets/Buttons/restartIcon");
            //aStarIcon = content.Load<Texture2D>("Assets/Buttons/aStarIcon");
            //TdfsIcon = content.Load<Texture2D>("Assets/Buttons/dfsIcon");

            //Towers
            iceTowerTexture = Content.Load<Texture2D>("Assets/Tower/IceTower");
            stormTowerTexture = Content.Load<Texture2D>("Assets/Tower/StormTower");

            //Keys
            for (int i = 0; i < 5; i++)
            {
                iceTowerKeyTexture[i] = Content.Load<Texture2D>($"Assets/IceKey/IceKey_{i + 1}");
            }

            for (int i = 0; i < 12; i++)
            {
                stormTowerKeyTexture[i] = Content.Load<Texture2D>($"Assets/StormKey/StormKey_{i + 1}");
            }

            //Portal
            for (int i = 0; i < portalTexture.Length; i++)
            {
                portalTexture[i] = Content.Load<Texture2D>($"Assets/Portal/Portal_{i + 1}");
            }

            //Wizard
            for (int i = 0; i < wizardIdleTexture.Length; i++)
            {
                wizardIdleTexture[i] = Content.Load<Texture2D>($"Assets/Wizard/MiniMage_Idle00{i + 1}");
            }

            for (int i = 0; i < wizardRunningTexture.Length; i++)
            {
                wizardRunningTexture[i] = Content.Load<Texture2D>($"Assets/Wizard/MiniMage_Run00{i + 1}");
            }
        }

        /// <summary>
        /// Opretter tilemap'et baseret på de angivne dimensioner og tile teksturen.
        /// </summary>
        public void LoadTileMap()
        {
            int tileWidth = 80;
            int tileHeight = 80;

            int mapWidth = GraphicsDevice.Viewport.Width / tileWidth;
            int mapHeight = GraphicsDevice.Viewport.Height / tileHeight;

            tileMap = new TileMap(mapWidth, mapHeight, tileWidth, tileHeight, tileTexture);
        }

        public void LoadButtons()
        {
            Button btnRestartGame = new Button(buttonTexture, new Vector2(50, 10))
            {
                Icon = restartIcon,
                IconSourceRect = new Rectangle(0, 0, 150, 150), //Skal muligvis slettes
                PressedTexture = pressedButtonTexture,
            };

            Button btnAStar = new Button(buttonTexture, new Vector2(btnRestartGame.Position.X + btnRestartGame.Texture.Width + 75, 10))
            {
                Icon = aStarIcon,
                IconSourceRect = new Rectangle(0, 0, 150, 150), //Skal muligvis slettes
                PressedTexture = pressedButtonTexture,
            };

            Button btnDFS = new Button(buttonTexture, new Vector2(btnAStar.Position.X + btnAStar.Texture.Width + 75, 10))
            {
                Icon = dfsIcon,
                IconSourceRect = new Rectangle(0, 0, 150, 150), //Skal muligvis slettes
                PressedTexture = pressedButtonTexture,
            };

            buttons.Add(btnRestartGame);
            buttons.Add(btnAStar);
            buttons.Add(btnDFS);
        }

        /// <summary>
        /// Opretter baserne (TownHalls) for de to fraktioner.
        /// </summary>
        public void LoadTowers()
        {
            //int offsetX = 50;
            //int windowHeight = GraphicsDevice.Viewport.Height;
            //int windowWidth = GraphicsDevice.Viewport.Width;

            Vector2 stormTowerPosition = new Vector2(GraphicsDevice.Viewport.Width / 2, 50);
            Vector2 iceTowerPosition = new Vector2(GraphicsDevice.Viewport.Width - 100, GraphicsDevice.Viewport.Height / 2);

            stormTower = new Tower(TowerType.Storm, stormTowerPosition, stormTowerTexture);
            iceTower = new Tower(TowerType.Ice, iceTowerPosition, iceTowerTexture);

        }

        public void LoadKeys()
        {
            //Keys placering

            Random random = new Random();
            Vector2 iceKeyPosition = new Vector2(random.Next(100, GraphicsDevice.Viewport.Width - 200),
                                                 random.Next(100, GraphicsDevice.Viewport.Height - 200));

            Vector2 stormKeyPosition = new Vector2(random.Next(100, GraphicsDevice.Viewport.Width - 200),
                                                   random.Next(100, GraphicsDevice.Viewport.Height - 200));

            iceKey = new Key(TowerType.Ice, iceKeyPosition, iceTowerKeyTexture);
            stormKey = new Key(TowerType.Storm, stormKeyPosition, stormTowerKeyTexture);

            keys.Add(iceKey);
            keys.Add(stormKey);

        }

        public void LoadPortal()
        {
            Vector2 portalPosition = new Vector2(50, GraphicsDevice.Viewport.Height - 100);
            portal = new Portal(portalPosition, portalTexture);
        }

        public void LoadWizard()
        {
            Vector2 wizardPosition = portal.Position;
            wizard = new Wizard(wizardIdleTexture, wizardRunningTexture, wizardPosition);
        }

        /// <summary>
        /// Opdaterer spillets logik pr. frame.
        /// </summary>
        /// <param name="gameTime">Spillets tidsdata.</param>
        protected override void Update(GameTime gameTime)
        {
            // Luk spil med ESC
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Opdater knapper
            foreach (Button btn in buttons)
            {
                btn.Update(gameTime);
            }

            // Opdater keys
            /*
            foreach (Key key in keys)
            {
                key.Update(gameTime);
            }
            */

            portal.Update(gameTime);
            wizard.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Tegner spillets elementer pr. frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Tegn tilemap
            tileMap.Draw(_spriteBatch, 0.0f);            

            // Tegn towers
            iceTower.Draw(_spriteBatch, 0.3f);
            stormTower.Draw(_spriteBatch, 0.3f);

            //Tegn Keys

            
            foreach (Key key in keys)
            {
                key.Draw(_spriteBatch, 0.3f);
            }
            

            // Tegn knapper
            foreach (Button btn in buttons)
            {
                btn.Draw(_spriteBatch, 1.0f);
            }

            // Tegn portal
            portal.Draw(_spriteBatch, 0.4f);

            // Tegn wizard
            wizard.Draw(_spriteBatch, 0.5f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Håndterer klik på knapper (restart, AStar, DFS).
        /// </summary>
        private void OnButtonClick(ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Restart:
                    RestartGame();
                    break;
                case ButtonType.AStar:
                    RunAStar();
                    break;
                case ButtonType.DFS:
                    RunDFS();
                    break;
            }
        }

        /// <summary>
        /// Nulstiller spillet (fx reloader tilemap, wizard position, m.m.).
        /// </summary>
        private void RestartGame()
        {
            // Implementer reset-logik her
        }

        /// <summary>
        /// Kører A* pathfinding fra wizard til Storm Tower (fx).
        /// Derefter flytter wizarden til sidste tile i stien.
        /// </summary>
        private void RunAStar()
        {
            // Implementer A* pathfinding her
        }

        /// <summary>
        /// Kører DFS pathfinding fra wizard til Storm Tower.
        /// </summary>
        private void RunDFS()
        {
            // Implementer DFS pathfinding her
        }
    }
}
