using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class Bar
    {
        private Game1 game;

        private Sprite borderSprite;
        private Sprite barSprite;

        private Vector2 borderPosition;
        public Vector2 Position { 
            get { return borderPosition; } 
            set { borderPosition = value; } 
        }
        private Vector2 barPosition;
        private float scale;
        private Color color;
        private bool horizontal;

        private float maxValue;
        private float value;

        public Bar(Game1 Game, Sprite spriteSheet, Color color, bool horizontal)
        {
            game = Game;

            borderSprite = new Sprite(spriteSheet.Texture, new Rectangle(0, 7, 139, 7));
            barSprite = new Sprite(spriteSheet.Texture, new Rectangle(1, 15, 137, 5));
            this.color = color;
            this.horizontal = horizontal;
        }

        public Bar(Game1 Game, Sprite spriteSheet, Color color, bool horizontal, Rectangle? borderSourceRect, Rectangle? barSourceRect)
        {
            game = Game;

            if (borderSourceRect != null) { borderSprite = new Sprite(spriteSheet.Texture, borderSourceRect); }
            else { borderSprite = new Sprite(spriteSheet.Texture, new Rectangle(0, 7, 139, 7)); }
            if (barSourceRect != null) { barSprite = new Sprite(spriteSheet.Texture, barSourceRect); }
            else { barSprite = new Sprite(spriteSheet.Texture, new Rectangle(1, 15, 135, 5)); }
            this.horizontal = horizontal;

            this.color = color;
        }

        public void Initialize()
        {            
        }

        public void Update(GameTime gameTime, float value, float maxValue, Vector2 pos)
        {
            Position = pos;
            barPosition = new Vector2(pos.X + 1, pos.Y + 1);

            this.value = value;
            this.maxValue = maxValue;

            scale = value / maxValue;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(borderSprite.Texture, borderPosition, borderSprite.SourceRectangle,
                Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.94f);

            if (horizontal)
            {
                spriteBatch.Draw(barSprite.Texture, barPosition, barSprite.SourceRectangle, color,
                    0f, Vector2.Zero, new Vector2(scale, 1f), SpriteEffects.None, 0.91f);
            }
            else
            {
                spriteBatch.Draw(barSprite.Texture, new Vector2(barPosition.X, barPosition.Y + barSprite.SourceRectangle.Value.Height), barSprite.SourceRectangle, color,
                    0f, new Vector2(0, barSprite.SourceRectangle.Value.Height), new Vector2(1f, scale), SpriteEffects.None, 0.91f);
            }
        }
    }
}
