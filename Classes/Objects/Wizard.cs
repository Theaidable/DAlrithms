using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        public bool HasPotion { get; set; }

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

            // Vælg frame alt efter state
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
                spriteBatch.Draw(currentFrame, Position, null, Color.White, 0f,
                                 Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
            }
        }

        public void MoveTo(Vector2 newPosition)
        {
            Position = newPosition;
            // Sæt currentState = Running under bevægelse, eller sæt til Idle når stoppet
        }
    }
}
