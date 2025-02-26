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


        public int tileWidth = 80;
        public int tileHeight = 80;

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
            _graphics.PreferredBackBufferWidth = tileWidth * 25;
            _graphics.PreferredBackBufferHeight = tileHeight * 14;
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
            restartIcon = Content.Load<Texture2D>("Assets/Icons/refresh");
            aStarIcon = Content.Load<Texture2D>("Assets/Icons/Star");
            dfsIcon = Content.Load<Texture2D>("Assets/Icons/Fruit");

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

            int mapWidth = GraphicsDevice.Viewport.Width / tileWidth;
            int mapHeight = GraphicsDevice.Viewport.Height / tileHeight;

            tileMap = new TileMap(mapWidth, mapHeight, tileWidth, tileHeight, tileTexture);
        }

        public void LoadButtons()
        {
            Button btnRestartGame = new Button(buttonTexture, new Vector2(10, 10))
            {
                Icon = restartIcon,
                PressedTexture = pressedButtonTexture,
            };

            Button btnAStar = new Button(buttonTexture, new Vector2(btnRestartGame.Position.X + btnRestartGame.Texture.Width + 10, 10))
            {
                Icon = aStarIcon,
                PressedTexture = pressedButtonTexture,
            };

            Button btnDFS = new Button(buttonTexture, new Vector2(btnAStar.Position.X + btnAStar.Texture.Width + 10, 10))
            {
                Icon = dfsIcon,
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
            int stormTileX = 7; // Justér efter hvor vi vil have dem
            int stormTileY = 2;
            int iceTileX = 18;
            int iceTileY = 9;

            float stormX = stormTileX * 80 + 17 - stormTowerTexture.Width / 2;
            float stormY = stormTileY * 80 + 2 - stormTowerTexture.Height / 2;
            float iceX = iceTileX * 80 + 17 - iceTowerTexture.Width / 2;
            float iceY = iceTileY * 80 + 2 - iceTowerTexture.Height / 2;

            stormTower = new Tower(TowerType.Storm, new Vector2(stormX, stormY), stormTowerTexture);
            iceTower = new Tower(TowerType.Ice, new Vector2(iceX, iceY), iceTowerTexture);

        }

        public void LoadKeys()
        {
            //Keys placering

            Random random = new Random();
            int keyTileX = random.Next(1, 10);
            int keyTileY = random.Next(1, 6);

            float keyX = keyTileX * 80 + 45 - iceTowerKeyTexture[0].Width / 2;
            float keyY = keyTileY * 80 + 40 - iceTowerKeyTexture[0].Height / 2;

            iceKey = new Key(TowerType.Ice, new Vector2(keyX, keyY), iceTowerKeyTexture);
            stormKey = new Key(TowerType.Storm, new Vector2(keyX + 160, keyY + 160), stormTowerKeyTexture);

            keys.Add(iceKey);
            keys.Add(stormKey);

        }

        public void LoadPortal()
        {
            int tileX = 1;  // Tile-koordinat for portalen
            int tileY = 11;  // Eksempelværdi - justér efter behov

            float xPos = tileX * 80 + 35 - portalTexture[0].Width / 2;
            float yPos = tileY * 80 + 40 - portalTexture[0].Height / 2;

            Vector2 portalPosition = new Vector2(xPos, yPos);
            portal = new Portal(portalPosition, portalTexture);
        }

        public void LoadWizard()
        {
            int tileX = 2;
            int tileY = 11;

            float xPos = tileX * 80 + 10 - wizardIdleTexture[0].Width / 2;
            float yPos = tileY * 80 - 10 - wizardIdleTexture[0].Height / 2;

            Vector2 wizardPosition = new Vector2(xPos, yPos);
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
            foreach (Key key in keys)
            {
                key.Update(gameTime);
            }

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
