using DAlgorithms.Classes.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DAlgorithms.Classes.Objects
{
    public enum WizardState
    {
        Idle,
        Running
    }

    public class Wizard
    {
        public Vector2 Position { get; set; }

        public bool HasPotion { get; set; } = false;
        public bool HasIceKey { get; set; } = false;
        public bool HasStormKey {  get; set; } = false;
        public bool VisitedStormTower { get; set; } = false;
        public bool VisitedIceTower { get; set; } = false;

        // To sæt frames
        private Texture2D[] idleFrames;
        private Texture2D[] runFrames;

        private int animationIndex = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;

        public WizardState CurrentState { get; set; } = WizardState.Idle;

        public Wizard(Texture2D[] idleFrames, Texture2D[] runFrames, Vector2 position)
        {
            this.idleFrames = idleFrames;
            this.runFrames = runFrames;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer = 0f;
                animationIndex++;

                // Vælg hvilket frames-array vi bruger
                switch (CurrentState)
                {
                    case WizardState.Idle:
                        animationIndex %= idleFrames.Length;
                        break;
                    case WizardState.Running:
                        animationIndex %= runFrames.Length;
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Texture2D currentFrame = null;
            float scale = 100f / idleFrames[0].Width;

            //Vælg frame alt efter state
            switch (CurrentState)
            {
                case WizardState.Idle:
                    currentFrame = idleFrames[animationIndex];
                    break;
                case WizardState.Running:
                    currentFrame = runFrames[animationIndex];
                    break;
            }

            if (currentFrame != null)
            {
                spriteBatch.Draw(currentFrame, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
            }
        }

        public void MoveTo(Vector2 newPosition)
        {
            CurrentState = WizardState.Running;
            Position = newPosition;
        }

        public void MoveAlongPath(List<Tile> path, TileMap tileMap)
        {
            foreach (Tile tile in path)
            {
                CurrentState = WizardState.Running;

                float xPos = tile.Position.X + tile.Width / 2f - (idleFrames[0].Width / 2f);
                float yPos = tile.Position.Y + tile.Height / 2f - (idleFrames[0].Height / 2f);

                MoveTo(new Vector2(xPos, yPos));

                if (tile.Type == TileType.NoMonster)
                {
                    tileMap.SetTileType((int)xPos, (int)yPos,TileType.Monster);
                }

                if(tile.Type == TileType.OpenStormTower)
                {
                    VisitedStormTower = true;
                    HasPotion = true;
                }

                if(tile.Type == TileType.OpenIceTower)
                {
                    VisitedIceTower = true;
                    HasPotion = false;
                    tileMap.SetTileType((int)xPos, (int)yPos, TileType.OpenPortal);
                }
            }

            CurrentState = WizardState.Idle;
        }
    }
}
