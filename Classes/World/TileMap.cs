using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace DAlgorithms.Classes.World
{
    public class TileMap
    {
        // Private felter
        private Tile[,] tiles;   // 2D-array med Tile-objekter
        private int mapWidth;    // Antal tiles vandret
        private int mapHeight;   // Antal tiles lodret
        private int tileWidth;   // Bredde af hver tile
        private int tileHeight;  // Højde af hver tile
        private Texture2D tileTexture; // Tekstur/spritesheet for tiles

        /// <summary>
        /// Konstruerer en ny TileMap med de angivne dimensioner og tekstur.
        /// Alle tiles oprettes som Grass-tiles.
        /// </summary>
        /// <param name="mapWidth">Antal tiles vandret.</param>
        /// <param name="mapHeight">Antal tiles lodret.</param>
        /// <param name="tileWidth">Bredden for hver tile.</param>
        /// <param name="tileHeight">Højden for hver tile.</param>
        /// <param name="tileTexture">Teksturen eller spritesheetet, der indeholder tile-grafikken.</param>
        public TileMap(int mapWidth, int mapHeight, int tileWidth, int tileHeight, Texture2D tileTexture)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
            this.tileTexture = tileTexture;

            GenerateMap();
        }

        public void GenerateMap()
        {
            // Opret et 2D-array med Tile-objekter
            tiles = new Tile[mapWidth, mapHeight];

            // Fyld tilemap'et med tiles
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    // Beregn tile'ens position baseret på dens indeks og dimensioner
                    Vector2 position = new Vector2(x * tileWidth, y * tileHeight);

                    // Definer en source rectangle for tile'en. Her bruges hele tile-området.
                    Rectangle sourceRectangle = new Rectangle(0, 0, tileWidth, tileHeight);

                    // Opret en ny tile af typen Grass
                    tiles[x, y] = new Tile(position, tileWidth, tileHeight, TileType.Grass, tileTexture, sourceRectangle);

                }
            }
        }

        private Rectangle GetTileSourceRectangle(TileType type)
        {
            switch (type)
            {
                case TileType.Grass:
                    return new Rectangle(0, 0, tileWidth, tileHeight);
                case TileType.Wall:
                    return new Rectangle(80, 0, tileWidth, tileHeight);
                case TileType.Path:
                    return new Rectangle(160, 0, tileWidth, tileHeight);
                case TileType.Forest:
                    return new Rectangle(240, 0, tileWidth, tileHeight);
                case TileType.Monster:
                    return new Rectangle(320, 0, tileWidth, tileHeight);
                default:
                    return new Rectangle(0, 0, tileWidth, tileHeight);
            }
        }

        public void LoadMapFromFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            tiles = new Tile[mapWidth, mapHeight];

            for (int y = 0; y < lines.Length; y++)
            {
                string[] values = lines[y].Split(' ');

                for (int x = 0; x < values.Length; x++)
                {
                    int tileTypeValue = int.Parse(values[x]);
                    TileType tileType = (TileType)tileTypeValue; // Konverter til enum

                    Vector2 position = new Vector2(x * tileWidth, y * tileHeight);
                    tiles[x, y] = new Tile(position, tileWidth, tileHeight, tileType, tileTexture, GetTileSourceRectangle(tileType));
                }
            }
        }

        /// <summary>
        /// Tegner hele tilemap'et på skærmen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne tiles.</param>
        /// <param name="layerDepth">Lagdybden for tegningen, der bestemmer tegneordenen.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    // Tegn hver enkelt tile med den specificerede layerDepth
                    tiles[x, y].Draw(spriteBatch, layerDepth);
                }
            }
        }
    }
}
