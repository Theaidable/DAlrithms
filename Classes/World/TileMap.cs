using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

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

        public Tile GetTile(int x, int y)
        {
            // Tjek om indekserne er inden for arrayets grænser
            if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
            {
                Debug.WriteLine($"GetTile fejlede: x={x}, y={y} - gyldige intervaller: 0-{tiles.GetLength(0) - 1} og 0-{tiles.GetLength(1) - 1}");
                throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
            }
            return tiles[x, y];
        }

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
        public void SetTileType(int x, int y, TileType newType)
        {
            Tile oldTile = tiles[x, y];
            oldTile.Type = newType;

            switch (newType)
            {
                case TileType.Grass:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(0, 0, 18, 18);
                    break;
                case TileType.Wall:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(36, 0, 18, 18);
                    break;
                case TileType.Path:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.Forest:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(0, 0, 18, 18);
                    break;
                case TileType.NoMonster:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.Monster:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.LockedStormTower:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.OpenStormTower:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.LockedIceTower:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.OpenIceTower:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.LockedPortal:
                    oldTile.IsWalkable = false;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.OpenPortal:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
                case TileType.Start:
                    oldTile.IsWalkable = true;
                    oldTile.SetSourceRectangle(18, 0, 18, 18);
                    break;
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
