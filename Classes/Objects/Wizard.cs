using DAlgorithms.Classes.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

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

        /// <summary>
        /// Starter bevægelse langs en given sti. Stien skal være en liste af Tile-objekter.
        /// Vi beregner her tile-center for hvert element og gemmer dem som mål.
        /// </summary>
        /// <param name="tilePath">Stien som en liste af Tiles.</param>
        public void StartPathMovement(List<Tile> tilePath)
        {
            if (tilePath == null || tilePath.Count == 0) return;

            // Omdan stien til en liste af positioner (tile-centre)
            pathPositions = new List<Vector2>();
            foreach (var tile in tilePath)
            {
                Vector2 targetPos = new Vector2(tile.Position.X + tile.Width / 2f - (idleFrames[0].Width / 2f), tile.Position.Y + tile.Height / 2f - (idleFrames[0].Height / 2f));
                pathPositions.Add(targetPos);
            }

            currentTargetIndex = 0;
            // Skift til Running state
            CurrentState = WizardState.Running;
        }

        public void Update(GameTime gameTime, List<Key> keys, Action<Key> onKeyCollected, Action onDestinationReached)
        {
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

            if (pathPositions != null && currentTargetIndex < pathPositions.Count)
            {
                Vector2 target = pathPositions[currentTargetIndex];
                Vector2 direction = target - Position;
                float distance = direction.Length();

                if (distance < 1f)
                {
                    currentTargetIndex++;
                    if (currentTargetIndex >= pathPositions.Count)
                    {
                        pathPositions = null;
                        CurrentState = WizardState.Idle;
                        onDestinationReached?.Invoke();
                    }
                }
                else
                {
                    direction.Normalize();
                    Position += direction * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            // 🚀 Sørg for at fjerne keys og fortsætte
            CheckForKeyPickup(keys, onKeyCollected);
        }




        private void CheckForKeyPickup(List<Key> keys, Action<Key> onKeyCollected)
        {
            for (int i = keys.Count - 1; i >= 0; i--) // Gå baglæns for at kunne fjerne keys sikkert
            {
                Key key = keys[i];

                // 🚀 Tjek om wizarden er på samme tile som key
                if (!key.IsCollected && Vector2.Distance(Position, key.Position) < 40f)
                {
                    key.IsCollected = true;

                    if (key.TowerType == TowerType.Storm)
                    {
                        HasStormKey = true;
                        Debug.WriteLine("Wizarden har samlet Storm Key op!");

                        // 🚀 Gør Storm Tower walkable
                        Point stormTowerTile = GameWorld.nodePositions["StormTower"];
                        GameWorld.tileMap.SetTileType(stormTowerTile.X, stormTowerTile.Y, TileType.OpenStormTower);
                    }

                    else if (key.TowerType == TowerType.Ice)
                    {
                        HasIceKey = true;
                        Debug.WriteLine("Wizarden har samlet Ice Key op!");
                    }

                    // 🚀 Fjern key-objektet fra spillet
                    onKeyCollected?.Invoke(key);
                    keys.RemoveAt(i);
                    Debug.WriteLine("Key fjernet fra spillet");

                    return; // Stop loopet efter første fundne nøgle
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

        /// <summary>
        /// Metoden til at flytte wizarden øjeblikkeligt. Bruges fx til at sætte startpositionen.
        /// </summary>
        public void MoveTo(Vector2 newPosition)
        {
            CurrentState = WizardState.Running;
            Position = newPosition;
        }
    }

}
