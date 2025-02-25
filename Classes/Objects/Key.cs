using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.Objects
{
    public class Key
    {
        public TowerType TowerType { get; private set; }
        public Vector2 Position { get; private set; }
        private Texture2D[] keyTexture;
        public bool IsCollected { get; set; } = false;

        private int animationIndex = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;

        public Key(TowerType towerType, Vector2 position, Texture2D[] texture)
        {
            TowerType = towerType;
            Position = position;
            this.keyTexture = texture;
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
                animationIndex = (animationIndex + 1) % keyTexture.Length;
            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (!IsCollected && keyTexture != null)
            {
                spriteBatch.Draw(keyTexture[animationIndex], Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
        }
    }
}
