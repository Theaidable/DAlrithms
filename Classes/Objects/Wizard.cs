using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using DAlgorithms.Classes.World;
using System.Diagnostics;

namespace DAlgorithms.Classes.Objects
{
    public enum WizardState
    {
        Idle,
        Running
    }

    public class Wizard
    {
        public Vector2 Position { get; private set; }
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

        // Bevægelsesfelter
        public List<Vector2> pathPositions = new List<Vector2>();
        public int currentTargetIndex = 0;
        private float movementSpeed = 100f; // Pixels per sekund

        private TileMap tileMap;
        private GameWorld gameWorld; // Reference til GameWorld for at kalde RunDFS()

        public Wizard(Texture2D[] idleFrames, Texture2D[] runFrames, Vector2 position, TileMap tileMap, GameWorld gameWorld)
        {
            this.idleFrames = idleFrames;
            this.runFrames = runFrames;
            this.tileMap = tileMap;
            this.gameWorld = gameWorld;
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= frameTime)
            {
                timer = 0f;
                animationIndex++;
                animationIndex %= (CurrentState == WizardState.Idle ? idleFrames.Length : runFrames.Length);
            }

            MoveAlongPath(gameTime);
        }

        private void MoveAlongPath(GameTime gameTime)
        {
            if (pathPositions.Count == 0 || currentTargetIndex >= pathPositions.Count)
                return;

            Vector2 target = pathPositions[currentTargetIndex];
            Vector2 direction = target - Position;

            if (direction.Length() < 1f) // Wizard har nået en tile
            {
                HandleTileInteraction(target);
                currentTargetIndex++;

                if (currentTargetIndex >= pathPositions.Count)
                {
                    pathPositions.Clear();
                    CurrentState = WizardState.Idle;
                }
            }
            else
            {
                direction.Normalize();
                Position += direction * movementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        private void HandleTileInteraction(Vector2 position)
        {
            // Konverter pixels til tile-koordinater
            int tileX = (int)(position.X / tileMap.tileWidth);
            int tileY = (int)(position.Y / tileMap.tileHeight);

            // Tjek at vi ikke går uden for arrayet
            if (tileX < 0 || tileX >= tileMap.mapWidth || tileY < 0 || tileY >= tileMap.mapHeight)
            {
                Debug.WriteLine($"Advarsel: Wizard prøver at ændre en tile uden for kortet! x={tileX}, y={tileY}");
                return;
            }

            Tile currentTile = tileMap.GetTile(tileX, tileY);

            if (currentTile.Type == TileType.IceKey)
            {
                HasIceKey = true;
                tileMap.SetTileType(tileX, tileY, TileType.Path); // Fjern nøglen
                Debug.WriteLine("Wizard har samlet IceKey!");
            }

            if (currentTile.Type == TileType.StormKey)
            {
                HasStormKey = true;
                tileMap.SetTileType(tileX, tileY, TileType.Path); // Fjern nøglen
                Debug.WriteLine("Wizard har samlet StormKey!");

                // Opdater grafen og find ny sti
                tileMap.SetTileType(7, 2, TileType.OpenStormTower);
                gameWorld.UpdateGraphBasedOnProximity(this);
                gameWorld.RunDFS();
            }

            if (currentTile.Type == TileType.LockedStormTower && HasStormKey)
            {
                HasPotion = true;
                VisitedStormTower = true;
                tileMap.SetTileType(tileX, tileY, TileType.OpenStormTower);
                Debug.WriteLine("Wizard har besøgt StormTower og fået potion!");
            }

            if (currentTile.Type == TileType.LockedIceTower && HasIceKey && HasPotion)
            {
                tileMap.SetTileType(tileX, tileY, TileType.OpenIceTower);
                Debug.WriteLine("Wizard har åbnet IceTower!");
            }

            if (currentTile.Type == TileType.OpenIceTower && VisitedStormTower)
            {
                VisitedIceTower = true;
                tileMap.SetTileType(11, 1, TileType.OpenPortal);
                Debug.WriteLine("Portal er åbnet!");
            }

            if (currentTile.Type == TileType.NoMonster)
            {
                tileMap.SetTileType(tileX, tileY, TileType.Monster);
                Debug.WriteLine("Wizard har gået over NoMonster tile. Den er nu Monster!");
            }
        }


        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Texture2D currentFrame = CurrentState == WizardState.Idle ? idleFrames[animationIndex] : runFrames[animationIndex];
            float scale = 100f / idleFrames[0].Width;

            spriteBatch.Draw(currentFrame, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
        }
    }
}
