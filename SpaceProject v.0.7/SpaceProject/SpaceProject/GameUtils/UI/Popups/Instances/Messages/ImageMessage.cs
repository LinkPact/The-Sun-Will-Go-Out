using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ImageMessage : ImagePopup
    {
        protected TextContainer textContainer;

        public ImageMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 0, 400, 400));
        }

        public override void Initialize()
        {
            base.Initialize();

            textContainer = new TextContainer(canvasPosition, canvas.SourceRectangle.Value);
            textContainer.Initialize();
            textContainer.SetDefaultPosition(this.GetType());
            textContainer.UseScrolling = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            textContainer.Update(gameTime, canvasPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            textContainer.Draw(spriteBatch);
        }

        public void SetMessage(params string[] messages)
        {
            textContainer.SetMessage(messages);
        }

        protected override void Hide()
        {
            textContainer.UpdateTextBuffer();
            imageContainer.UpdateImageBuffer();

            if (textContainer.IsTextBufferEmpty()
                && imageContainer.ImageBufferCount <= 0)
            {
                base.Hide();
            }
        }
    }
}
