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

            imageContainer = new ImageContainer(game, canvas.SourceRectangle.Value, canvasPosition);
            imageContainer.Initialize();
            usePause = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            imageContainer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            imageContainer.Draw(spriteBatch);
        }

        public void SetImages(List<Sprite> images, List<int> imageTriggers)
        {
            SetImages(images.ToArray<Sprite>());
            imageContainer.SetImageTriggers(imageTriggers.ToArray<int>());
        }

        public void SetImages(params Sprite[] images)
        {
            imageContainer.SetImages(images);
        }

        protected override void Hide()
        {
            base.Hide();
        }
    }
}
