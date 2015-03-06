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
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 645, 567, 234));
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
            portraitContainer.UpdatePosition(game.camera.cameraPos);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            portraitContainer.Draw(spriteBatch);
        }

        protected override void Hide()
        {
            base.Hide();

            portraitContainer.UpdateImageBuffer();
        }

        public void SetPortrait(params Sprite[] portraits)
        {
            portraitContainer.SetImages(portraits);
        }

        public void SetPortrait(List<Sprite> portraits, int numberOfMessages, List<int> portraitTriggers)
        {
            SetPortrait(portraits.ToArray<Sprite>());
            portraitContainer.SetImageTriggers(numberOfMessages, portraitTriggers.ToArray<int>());
        }
    }
}
