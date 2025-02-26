using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.Objects
{
    /// <summary>
    /// Repræsenterer en portal, som Wizard skal spawne ved og vende tilbage til.
    /// </summary>
    public class Portal
    {
        public Vector2 Position { get; private set; }
        private Texture2D[] portalTexture;
        private int animationIndex = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;

        /// <summary>
        /// Konstruerer en ny Portal med de angivne frames og position.
        /// </summary>
        /// <param name="frames">Array af teksturer for portalens animation.</param>
        /// <param name="position">Positionen for portalen.</param>
        public Portal(Vector2 position, Texture2D[] texture)
        {
            this.portalTexture = texture;
            this.Position = position;
        }

        /// <summary>
        /// Opdaterer portalens animation.
        /// </summary>
        /// <param name="gameTime">Spillets tidsdata.</param>
        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer = 0f;
                animationIndex = (animationIndex + 1) % portalTexture.Length;
            }
        }

        /// <summary>
        /// Tegner portalen på skærmen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch til at tegne.</param>
        /// <param name="layerDepth">Lagdybden for tegningen.</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            float scale = 80f / portalTexture[animationIndex].Width; // Dynamisk skalering
            spriteBatch.Draw(portalTexture[animationIndex], Position, null, Color.White, 0f,
                             Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }
    }
}
