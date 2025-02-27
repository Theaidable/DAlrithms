using DAlgorithms.Classes.Objects;
using DAlgorithms.Classes.UI;
using DAlgorithms.Classes.Algorithms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

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
        private Button btnRestartGame;
        private Button btnDFS;
        private Button btnAStar;
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
        private Texture2D treeTexture;
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
        private List<Tree> trees = new List<Tree>();

        //Graph
        private Graph<string> graph;
        private Dictionary<string, Point> nodePositions = new Dictionary<string, Point>()
        {
            {"Start", new Point(11,2) },
            {"StormTower", new Point(2,7) },
            {"IceTower", new Point(18,9)},
            {"Exit", new Point(11,1) }

        };

        //Map
        public int tileWidth = 80;
        public int tileHeight = 80;
        public char[,] layout = new char[,]
        {
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','L','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','B','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','P','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','P','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','P','G','G' },
            {'G','W','W','G','G','G','G','G','G','G','G','P','G','G' },
            {'G','W','S','P','P','P','P','P','P','P','P','P','G','G' },
            {'G','W','W','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','W','W','W','W','W','W','F','N','F','G' },
            {'G','G','G','P','P','P','P','P','P','I','P','P','G','G' },
            {'G','G','G','G','G','G','G','G','P','P','P','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
            {'G','G','G','G','G','G','G','G','G','G','G','G','G','G' },
        };

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
            LoadTrees();
            LoadButtons();
            LoadTowers();
            LoadKeys();
            LoadPortal();
            LoadWizard();

            btnRestartGame.Click += OnRestartGame_Click;
            btnAStar.Click += OnAStar_Click;
            btnDFS.Click += OnDFS_Click;
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

            //Tree
            treeTexture = Content.Load<Texture2D>("Assets/Tree/Tree");

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
        /// Sætter bogstaverne til deres bestemte TileType
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private TileType CharToTileType(char c)
        {
            switch (c)
            {
                case 'G': return TileType.Grass;
                case 'W': return TileType.Wall;
                case 'P': return TileType.Path;
                case 'F': return TileType.Forest;
                case 'N': return TileType.NoMonster;
                case 'M': return TileType.Monster;
                case 'S': return TileType.LockedStormTower;
                case 'I': return TileType.LockedIceTower;
                case 'L': return TileType.LockedPortal;
                case 's': return TileType.OpenStormTower;
                case 'i': return TileType.OpenIceTower;
                case 'l': return TileType.OpenPortal;
                case 'O': return TileType.OpenPortal;
                case 'B': return TileType.Start;
                default: return TileType.Grass; // fallback
            }
        }

        /// <summary>
        /// Opretter tilemap'et baseret på de angivne dimensioner og tile teksturen.
        /// </summary>
        public void LoadTileMap()
        {

            tileMap = new TileMap(layout.GetLength(0), layout.GetLength(1), tileWidth, tileHeight, tileTexture);

            // Overskriv tile-typer i tileMap efter layout
            for (int x = 0; x < layout.GetLength(0); x++)
            {
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    TileType type = CharToTileType(layout[x, y]);
                    tileMap.SetTileType(x, y, type);
                }
            }
        }

        public void LoadTrees()
        {
            int startTileX = 8;
            int treeTileY = 10;

            for (int i = 0; i < 10; i++)
            {
                int currentTileX = startTileX + i;
                float treeX = currentTileX * tileWidth + 10 - treeTexture.Width / 2;
                float treeY = treeTileY * tileHeight - 40 - treeTexture.Height / 2;

                trees.Add(new Tree(new Vector2(treeX, treeY), treeTexture));
            }
        }

        public void LoadButtons()
        {
            btnRestartGame = new Button(buttonTexture, new Vector2(10, 10))
            {
                Icon = restartIcon,
                PressedTexture = pressedButtonTexture,
            };

            btnAStar = new Button(buttonTexture, new Vector2(btnRestartGame.Position.X + btnRestartGame.Texture.Width + 10, 10))
            {
                Icon = aStarIcon,
                PressedTexture = pressedButtonTexture,
            };

            btnDFS = new Button(buttonTexture, new Vector2(btnAStar.Position.X + btnAStar.Texture.Width + 10, 10))
            {
                Icon = dfsIcon,
                PressedTexture = pressedButtonTexture,
            };

            buttons.Add(btnRestartGame);
            buttons.Add(btnAStar);
            buttons.Add(btnDFS);
        }

        private void OnRestartGame_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void OnAStar_Click(object sender, EventArgs e)
        {
            RunAStar();
        }

        private void OnDFS_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Du har nu klikket på DFS knap");
            RunDFS();
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
            List<Tile> walkableTiles = new List<Tile>();

            for (int x = 0; x < layout.GetLength(0); x++)
            {
                for (int y = 0; y < layout.GetLength(1); y++)
                {
                    Tile walkableTile = tileMap.GetTile(x, y);
                    if (walkableTile.IsWalkable == true)
                    {
                        walkableTiles.Add(walkableTile);
                    }
                }
            }

            //Keys placering

            Random random = new Random();
            int randomTile = random.Next(walkableTiles.Count);
            Tile randomTileForIceKey = walkableTiles[randomTile];

            //Placer nøglen i tile'ens center
            float iKeyX = randomTileForIceKey.Position.X + (randomTileForIceKey.Width / 2f) - (iceTowerKeyTexture[0].Width / 2f);
            float iKeyY = randomTileForIceKey.Position.Y + (randomTileForIceKey.Height / 2f) - (iceTowerKeyTexture[0].Height / 2f);

            iceKey = new Key(TowerType.Ice, new Vector2(iKeyX, iKeyY), iceTowerKeyTexture);

            randomTile = random.Next(walkableTiles.Count);
            Tile randomTileForStormKey = walkableTiles[randomTile];

            float sKeyX = randomTileForStormKey.Position.X + (randomTileForStormKey.Width / 2f) - (stormTowerKeyTexture[0].Width / 2f);
            float sKeyY = randomTileForStormKey.Position.Y + (randomTileForStormKey.Height / 2f) - (stormTowerKeyTexture[0].Height / 2f);

            stormKey = new Key(TowerType.Storm, new Vector2(sKeyX, sKeyY), stormTowerKeyTexture);

            keys.Add(iceKey);
            keys.Add(stormKey);

            nodePositions.Add("IceKey", new Point((int)iKeyX, (int)iKeyY));
            nodePositions.Add("StormKey", new Point((int)sKeyX, (int)sKeyY));
        }

        public void LoadPortal()
        {
            int tileX = 1;  // Tile-koordinat for portalen
            int tileY = 11;  // Justér efter behov

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

            //Tegn tilemap
            tileMap.Draw(_spriteBatch, 0.0f);

            //Tegn Trees
            foreach (Tree tree in trees)
            {
                tree.Draw(_spriteBatch, 0.3f);
            }

            //Tegn towers
            iceTower.Draw(_spriteBatch, 0.3f);
            stormTower.Draw(_spriteBatch, 0.3f);

            //Tegn Keys            
            foreach (Key key in keys)
            {
                key.Draw(_spriteBatch, 0.3f);
            }
            
            //Tegn knapper
            foreach (Button btn in buttons)
            {
                btn.Draw(_spriteBatch, 1.0f);
            }

            //Tegn portal
            portal.Draw(_spriteBatch, 0.4f);

            // Tegn wizard
            wizard.Draw(_spriteBatch, 0.5f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Nulstiller spillet (fx reloader tilemap, wizard position, m.m.).
        /// </summary>
        private void RestartGame()
        {
            // Implementer reset-logik her

            graph = null;
        }

        private void RunAStar()
        {
            // Implementer A* pathfinding her
        }

        private void SetupGraph()
        {
            graph = new Graph<string>();

            //Tilføj noder
            graph.AddNode("Start");
            graph.AddNode("StormKey");
            graph.AddNode("IceKey");
            graph.AddNode("StormTower");
            graph.AddNode("IceTower");
            graph.AddNode("Exit");

            //Fra Entrance
            graph.AddDirectedEdge("Start", "StormKey");
            graph.AddDirectedEdge("Start", "IceKey");

            //Fra StormKey
            graph.AddEdge("StormKey","IceKey");
            graph.AddDirectedEdge("StormKey", "StormTower");

            //Fra IceKey
            graph.AddDirectedEdge("IceKey", "IceTower");

            //Fra StormTower
            graph.AddDirectedEdge("StormTower","IceKey");
            graph.AddDirectedEdge("StormTower", "IceTower");

            //Fra IceTower
            graph.AddDirectedEdge("IceTower", "Exit");
        }

        private void RunDFS()
        {
            if (graph == null) SetupGraph();

            List<Node<string>> path = graph.FindPathDFS("Start", "Exit", wizard);

            if (path.Count == 0)
            {
                Debug.WriteLine("Ingen node-sti fundet");
                return;
            }
            else
            {
                Console.WriteLine("Path fundet:");
                    foreach (var node in path)
                {
                    Debug.WriteLine(node.Data);
                }
            }

            List<Tile> tilePath = new List<Tile>();
            foreach (var node in path)
            {
                if (nodePositions.ContainsKey(node.Data))
                {
                    Point tilePos = nodePositions[node.Data];
                    tilePath.Add(tileMap.GetTile(tilePos.X, tilePos.Y));
                }
            }

            // Start bevægelse langs stien
            wizard.StartPathMovement(tilePath);

            // Opdater tårn-tilstande baseret på nøgler og potioner
            if (wizard.HasStormKey)
            {
                Point stormTilePos = nodePositions["StormTower"];
                tileMap.SetTileType(stormTilePos.X, stormTilePos.Y, TileType.OpenStormTower);
            }
            if (wizard.HasIceKey && wizard.HasPotion)
            {
                Point iceTilePos = nodePositions["IceTower"];
                tileMap.SetTileType(iceTilePos.X, iceTilePos.Y, TileType.OpenIceTower);
            }
        }
    }
}
