using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.Objects
{
    public class Wizard
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        public bool HasPotion { get; set; }

        public Wizard(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        /// <summary>
        /// Flytter Wizarden til en ny position i tile-koordinater.
        /// </summary>
        /// <param name="newPosition">Ny tile-position (x,y).</param>
        public void MoveTo(Vector2 newPosition)
        {
            Position = newPosition;
        }

        /// <summary>
        /// Tegner wizarden på skærmen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch til at tegne wizarden.</param>
        /// <param name="layerDepth">Lagdybden for tegningen.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
        }
    }
}
