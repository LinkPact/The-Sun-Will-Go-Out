using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Cursor
    {
        private Game1 Game;
        private Sprite spriteSheet;

        private Sprite active;
        private Sprite passive;
        private Sprite activeSmall;
        private Sprite passiveSmall;

        public bool isActive;
        public bool isVisible;
        public bool isSmall;

        public Vector2 position;
        
        private Vector2 origin;

        public Cursor(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;

            active = spriteSheet.GetSubSprite(new Rectangle(371, 14, 14, 14));
            passive = spriteSheet.GetSubSprite(new Rectangle(371, 28, 14, 14));
            activeSmall = spriteSheet.GetSubSprite(new Rectangle(371, 14, 7, 7));
            passiveSmall = spriteSheet.GetSubSprite(new Rectangle(371, 28, 7, 7));

            origin = new Vector2(passive.SourceRectangle.Value.Width / 2, passive.SourceRectangle.Value.Height / 2);

            isSmall = false;
        }

        public Cursor(Game1 Game, Sprite spriteSheet, bool isSmall)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;

            active = spriteSheet.GetSubSprite(new Rectangle(371, 14, 14, 14));
            passive = spriteSheet.GetSubSprite(new Rectangle(371, 28, 14, 14));
            activeSmall = spriteSheet.GetSubSprite(new Rectangle(371, 14, 10, 10));
            passiveSmall = spriteSheet.GetSubSprite(new Rectangle(371, 28, 10, 10));

            origin = new Vector2(passive.SourceRectangle.Value.Width / 2, passive.SourceRectangle.Value.Height / 2);

            isSmall = true;
        }

        public Cursor(Game1 Game, Sprite spriteSheet, Rectangle activeRect, Rectangle passiveRect)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;

            this.active = spriteSheet.GetSubSprite(activeRect);
            this.passive = spriteSheet.GetSubSprite(passiveRect);
            this.activeSmall = spriteSheet.GetSubSprite(new Rectangle(activeRect.X, activeRect.Y, 10, 10));
            this.passiveSmall = spriteSheet.GetSubSprite(new Rectangle(passiveRect.X, passiveRect.Y, 10, 10));

            origin = new Vector2(this.passive.SourceRectangle.Value.Width / 2, this.passive.SourceRectangle.Value.Height / 2);

            //Added this line to your code. I don't think that you'll see any difference in-Game.
            isSmall = false;
        }

        public void Initialize()
        {
            isActive = false;
            isVisible = true;

            position = new Vector2(300, 300);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                if (!isSmall)
                {
                    if (isActive)
                        spriteBatch.Draw(active.Texture, position, active.SourceRectangle, FontManager.FontSelectColor1, 0, origin, 1.0f, SpriteEffects.None, 1f);
                    if (!isActive)
                        spriteBatch.Draw(passive.Texture, position, passive.SourceRectangle, FontManager.FontSelectColor1, 0, origin, 1.0f, SpriteEffects.None, 1f);
                }
                else
                {
                    if (isActive)
                        spriteBatch.Draw(activeSmall.Texture, position, activeSmall.SourceRectangle, FontManager.FontSelectColor1, 0, origin, 1.0f, SpriteEffects.None, 1f);
                    if (!isActive)
                        spriteBatch.Draw(passiveSmall.Texture, position, passiveSmall.SourceRectangle, FontManager.FontSelectColor1, 0, origin, 1.0f, SpriteEffects.None, 1f);
                }
            }   
        }
    }
}
