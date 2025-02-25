using DAlgorithms.Classes.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DAlgorithms.Classes.World
{
    /// <summary>
    /// GameWorld styrer spillets hovedløb og indeholder 
    /// tilemap, wizard, portal, towers, keys og UI-knapper.
    /// </summary>
    public class GameWorld : Game
    {
        private static GameWorld instance;

        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }
        }

        private static ContentManager content;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Reference til andre klasser
        private Portal portal;
        private Wizard wizard;
        private Button button;
        private Tower tower;
        private Key key;
        private TileMap tileMap;

        // Keys
        private List<Key> keys = new List<Key>();

        // Buttons
        private List<Button> buttons = new List<Button>();

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

            tileMap.LoadContent(content);
            portal.LoadContent(content);
            button.LoadContent(content);
            tower.LoadContent(content);
            key.LoadContent(content);
        }

        /// <summary>
        /// Opdaterer spillets logik pr. frame.
        /// </summary>
        /// <param name="gameTime">Spillets tidsdata.</param>
        protected override void Update(GameTime gameTime)
        {
            // Luk spil med ESC
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Opdater knapper
            foreach (var btn in buttons)
            {
                btn.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Tegner spillets elementer pr. frame.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Tegn tilemap
            tileMap.Draw(_spriteBatch, 0.0f);

            // Tegn portal
            portal.Draw(_spriteBatch, 0.3f);

            // Tegn towers
            tower.Draw(_spriteBatch, 0.3f);

            // Tegn keys
            foreach (Key key in keys)
            {
                key.Draw(_spriteBatch, 0.4f);
            }

            // Tegn wizard
            wizard.Draw(_spriteBatch, 0.5f);

            // Tegn knapper
            foreach (Button btn in buttons)
            {
                btn.Draw(_spriteBatch, 0.9f);
            }

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
