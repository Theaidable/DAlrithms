using DAlgorithms.Classes.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DAlgorithms.Classes.Objects
{
    public enum TowerType
    {
        Storm,
        Ice
    }
    public class Tower
    {
        public TowerType Type { get; set; }
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }

        private Texture2D iceTowerTexture;
        private Texture2D stormTowerTexture;

        // Towers
        private Tower stormTower;
        private Tower iceTower;

        public Tower(TowerType towerType,Vector2 position, Texture2D texture)
        {
            Type = towerType;
            Position = position;
            Texture = texture;
        }

        /// <summary>
        /// Tegner portalen på skærmen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="layerDepth"></param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            float scale = 90f / Texture.Width;

            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }
    }
}
