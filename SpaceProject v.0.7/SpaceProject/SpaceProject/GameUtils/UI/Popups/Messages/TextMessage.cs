using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class TextMessage : Popup
    {
        protected TextContainer textContainer;

        public TextMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(572, 406, 400, 183));
        }

        public override void Initialize()
        {
            base.Initialize();

            textContainer = new TextContainer(game, canvas.SourceRectangle.Value);
            textContainer.Initialize();
            textContainer.UseScrolling = true;

            usePause = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            textContainer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            textContainer.Draw(spriteBatch);
        }

        public virtual void SetMessage(params string[] messages)
        {
            textContainer.SetMessage(messages);
        }

        protected override void Hide()
        {
            // Has all text finished scrolling?
            if (((textContainer.UseScrolling 
                && textContainer.ScrollingFinished())
                || !textContainer.UseScrolling)
                && textContainer.IsTextBufferEmpty())
            {
                base.Hide();
            }
            // Has all text NOT finished scrolling?
            else if (textContainer.UseScrolling
                && !textContainer.ScrollingFinished()
                && delayTimer < 0)
            {
                textContainer.FlushText();
            }
        }
    }
}
