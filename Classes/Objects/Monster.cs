using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.Objects
{
    /// <summary>
    /// Repræsenterer et monster i miljøet.
    /// Klassen håndterer placeringen og den grafiske fremstilling af et monster.
    /// </summary>
    public class Monster
    {
        /// <summary>
        /// Gets or sets positionen for monster.
        /// </summary>
        public Vector2 Position { get; set; }

        // Teksturen der anvendes til at tegne monsteret.
        private Texture2D texture;

        /// <summary>
        /// Konstruerer en ny instans af Monster med den specificerede position og tekstur.
        /// </summary>
        /// <param name="position">Positionen for monsteret.</param>
        /// <param name="texture">Teksturen, der skal bruges til at tegne monsteret.</param>
        public Monster(Vector2 position, Texture2D texture)
        {
            Position = position;
            this.texture = texture;
        }

        /// <summary>
        /// Tegner monsteret på skærmen med en angivet lagdybde.
        /// Lagdybden bestemmer tegneordenen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne monsteret.</param>
        /// <param name="layerDepth">Lagdybden for monsterets tegning.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, layerDepth);
        }
    }
}
