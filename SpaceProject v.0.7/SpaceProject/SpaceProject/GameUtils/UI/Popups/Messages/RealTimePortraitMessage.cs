using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RealTimePortraitMessage : RealTimeMessage
    {
        private ImageContainer portraitContainer;

        public RealTimePortraitMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(354, 0, 400, 128));
        }

        public override void Initialize()
        {
            base.Initialize();

            portraitContainer = new ImageContainer(game, canvas.SourceRectangle.Value, canvasPosition);
            portraitContainer.Initialize();
            portraitContainer.SetImageType(ImageType.Portrait);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            portraitContainer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            portraitContainer.Draw(spriteBatch);
        }

        public void SetPortrait(Sprite portrait)
        {
            portraitContainer.SetImages(portrait);
        }
    }
}
