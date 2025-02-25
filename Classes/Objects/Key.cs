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
        public Texture2D Texture { get; private set; }
        public bool IsCollected { get; set; } = false;

        private Texture2D[] iceTowerKeyTexture = new Texture2D[5];
        private Texture2D[] stormTowerKeyTexture = new Texture2D[12];
        private Texture2D[] frames;
        private int animationIndex = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;

        public Key(TowerType towerType, Vector2 position, Texture2D texture)
        {
            TowerType = towerType;
            Position = position;
            Texture = texture;
        }

        public void LoadContent(ContentManager content)
        {
            for (int i = 0; i < 5; i++)
            {
                iceTowerKeyTexture[i] = content.Load<Texture2D>($"Assets/Objects/Keys/IceKey/IceKey_{i + 1}");
            }

            for (int i = 0; i < 12; i++)
            {
                // Her skal det sandsynligvis være stormTowerKeyTexture i stedet for iceTowerKeyTexture
                stormTowerKeyTexture[i] = content.Load<Texture2D>($"Assets/Objects/Keys/StormKey/StormKey_{i + 1}");
            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (!IsCollected && Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
        }
    }
}
