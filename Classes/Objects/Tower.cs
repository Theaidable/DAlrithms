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
        public bool IsVisited { get; set; } = false;
        public Texture2D Texture { get; private set; }

        private Texture2D iceTowerTexture;
        private Texture2D stormTowerTexture;

        // Towers
        private Tower stormTower;
        private Tower iceTower;

        public Tower(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }

        public void LoadContent(ContentManager content)
        {
            iceTowerTexture = content.Load<Texture2D>("Assets/Objects/Towers/Tower_Blue");
            stormTowerTexture = content.Load<Texture2D>("Assets/Objects/Towers/Tower_Yellow");

            LoadTowers();
        }

        /// <summary>
        /// Opretter baserne (TownHalls) for de to fraktioner.
        /// </summary>
        public void LoadTowers()
        {
            int offsetX = 50;
            int windowHeight = GameWorld.Instance.GraphicsDevice.Viewport.Height;
            int windowWidth = GameWorld.Instance.GraphicsDevice.Viewport.Width;

            Vector2 icePosition = new Vector2(offsetX, (windowHeight - iceTowerTexture.Height) / 2);
            Vector2 stormPosition = new Vector2(windowWidth - stormTowerTexture.Width - offsetX, (windowHeight - stormTowerTexture.Height) / 2);

            iceTower = new Tower(icePosition, iceTowerTexture);
            stormTower = new Tower(stormPosition, stormTowerTexture);
        }

        /// <summary>
        /// Tegner portalen på skærmen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="layerDepth"></param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
        }
    }
}
