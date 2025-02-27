using DAlgorithms.Classes.Algorithms;
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
        public bool HasStormKey { get; set; } = false;
        public bool VisitedStormTower { get; set; } = false;
        public bool VisitedIceTower { get; set; } = false;

        private Texture2D[] idleFrames;
        private Texture2D[] runFrames;
        private int animationIndex = 0;
        private float frameTime = 0.1f;
        private float timer = 0f;
        public WizardState CurrentState { get; set; } = WizardState.Idle;

        // Felter til animeret bevægelse langs en sti:
        private List<Vector2> pathPositions;  // Liste af målpositioner (tile-centre)
        private int currentTargetIndex = 0;
        private float movementSpeed = 200f; // pixels per sekund

        public Wizard(Texture2D[] idleFrames, Texture2D[] runFrames, Vector2 position)
        {
            this.idleFrames = idleFrames;
            this.runFrames = runFrames;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            // Opdater animationen
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer = 0f;
                animationIndex++;
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
    }

}
