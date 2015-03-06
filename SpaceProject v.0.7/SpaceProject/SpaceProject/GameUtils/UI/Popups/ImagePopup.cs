using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ImagePopup : Popup
    {
        protected ImageContainer imageContainer;

        public ImagePopup(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            if (!(this is ImageMessage))
            {
                canvas = spriteSheet.GetSubSprite(new Rectangle(405, 0, 400, 257));
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            imageContainer = new ImageContainer(canvasPosition, canvas.SourceRectangle.Value);
            imageContainer.Initialize();
            imageContainer.SetDefaultPosition(this.GetType());
            usePause = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            imageContainer.Draw(spriteBatch);
        }

        public void SetImages(int numberOfMessages, int[] imageTriggers, params Sprite[] images)
        {
            SetImages(images);
            imageContainer.SetImageTriggers(numberOfMessages, imageTriggers);
        }

        public void SetImages(params Sprite[] images)
        {
            imageContainer.SetImages(images);
        }

        protected override void Hide()
        {
            imageContainer.UpdateImageBuffer();

            if (imageContainer.IsImageBufferEmpty())
            {
                base.Hide();
            }
        }
    }
}
