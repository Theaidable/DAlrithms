using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.World
{
    /// <summary>
    /// Enum der repræsenterer de forskellige typer af tiles.
    /// </summary>
    public enum TileType
    {
        Grass,
        Wall,
        Path,
        Forest,
        NoMonster,
        Monster,
        IceKey,
        StormKey,
        LockedStormTower,
        OpenStormTower,
        LockedIceTower,
        OpenIceTower,
        LockedPortal,
        OpenPortal,
        Start
    }

    /// <summary>
    /// Repræsenterer en enkelt tile i spillets tilemap.
    /// Tile'en håndterer sin position, dimensioner og den del af teksturet, der skal tegnes.
    /// </summary>
    public class Tile
    {
        // Private felter
        private Rectangle sourceRectangle;
        public Vector2 Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public TileType Type { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsWalkable { get; set; }

        /// <summary>
        /// Konstruerer en ny tile med den angivne position, dimensioner, type og tekstur.
        /// Den definerer også, hvilken del af teksturet (sourceRectangle) der skal bruges.
        /// </summary>
        /// <param name="position">Tile'ens position (øverste venstre hjørne).</param>
        /// <param name="width">Tile'ens bredde.</param>
        /// <param name="height">Tile'ens højde.</param>
        /// <param name="type">Tile-typen (f.eks. Grass).</param>
        /// <param name="texture">Teksturen eller spritesheetet der indeholder tile grafik.</param>
        /// <param name="sourceRectangle">En rectangle der angiver, hvilken del af teksturet der skal bruges.</param>
        public Tile(Vector2 position, int width, int height, TileType type, Texture2D texture, Rectangle sourceRectangle)
        {
            Position = position;
            Width = width;
            Height = height;
            Type = type;
            Texture = texture;
            this.sourceRectangle = sourceRectangle;
        }

        public void SetSourceRectangle(int sourceX, int sourceY, int width, int height)
        {
            sourceRectangle = new Rectangle(sourceX, sourceY, width, height);
        }

        /// <summary>
        /// Tegner tile'en på skærmen med den angivne lagdybde.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne tile'en.</param>
        /// <param name="layerDepth">Lagdybden, der bestemmer, hvor tile'en placeres i forhold til andre objekter (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White, 0f, Vector2.Zero, 5f, SpriteEffects.None, layerDepth);
        }
    }
}
