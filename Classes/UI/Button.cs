using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace DAlgorithms.Classes.UI
{
    public enum ButtonType
    {
        Restart,
        AStar,
        DFS
    }
    /// <summary>
    /// Repræsenterer en UI-knap, der kan opdateres og tegnes.
    /// Knappen understøtter en normal og pressed tekstur, et ikon samt tekst.
    /// </summary>
    public class Button
    {

        public Texture2D Texture { get; set; }

        public Texture2D PressedTexture { get; set; }

        public Texture2D Icon { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Texture.Width, (int)Texture.Height);
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color Tint { get; set; } = Color.White;

        public bool IsHovering { get; private set; }
        public bool IsPressed { get; private set; }

        public Rectangle IconSourceRect { get; set; } = Rectangle.Empty;

        /// <summary>
        /// Event, der udløses, når knappen klikkes.
        /// </summary>
        public event EventHandler Click;

        private MouseState currentMouse;
        private MouseState previousMouse;
        public Texture2D ButtonTexture { get; set; }
        public Texture2D PressedButtonTexture { get; set; }

        /// <summary>
        /// Konstruerer en ny Button med den angivne tekstur og position.
        /// </summary>
        /// <param name="texture">Teksturen for knappen i normal tilstand.</param>
        /// <param name="position">Positionen (øverste venstre hjørne) for knappen.</param>
        public Button(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Opdaterer knapens tilstand baseret på musens position og knaptryk.
        /// Hvis musen er inden for knapens klikbare område, opdateres IsHovering og IsPressed, og
        /// Click-eventet udløses ved frigivelse af venstre musetast.
        /// </summary>
        /// <param name="gameTime">Spiltidens information.</param>
        public void Update(GameTime gameTime)
        {
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
            IsHovering = false;
            IsPressed = false;

            if (mouseRectangle.Intersects(Bounds))
            {
                IsHovering = true;

                if (currentMouse.LeftButton == ButtonState.Pressed)
                {
                    IsPressed = true;
                }

                if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Tegner knappen, inklusiv baggrund, ikon og tekst, med den angivne lagdybde.
        /// Lagdybden (layerDepth) bruges til at bestemme, hvor knappen vises i forhold til andre objekter.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch, der bruges til at tegne knappen.</param>
        /// <param name="layerDepth">Lagdybden for knappen (lavere værdi vises forrest).</param>
        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            Texture2D textureToDraw = (IsPressed && PressedTexture != null) ? PressedTexture : Texture;
            spriteBatch.Draw(textureToDraw, Position, null, Tint, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0.1f);

            if (Icon != null)
            {
                Rectangle sourceRect = IconSourceRect != Rectangle.Empty ? IconSourceRect : new Rectangle(0, 0, Icon.Width, Icon.Height);
                Vector2 iconPosition = Position + new Vector2((Texture.Width - sourceRect.Width) / 2, (Texture.Height - sourceRect.Height) / 2);
                Vector2 iconOffset = new Vector2(5, -2);
                iconPosition += iconOffset;
                spriteBatch.Draw(Icon, iconPosition, sourceRect, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1.0f);
            }
        }
    }
}