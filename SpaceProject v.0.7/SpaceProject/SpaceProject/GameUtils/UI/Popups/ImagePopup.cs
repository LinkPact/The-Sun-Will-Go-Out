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
        private readonly Point ImagePositionOffset = new Point(16, 18);
        private readonly float ImageLayerDepth = 0.96f;

        protected List<Sprite> imageBuffer;
        protected int imageTriggerIndex;
        protected List<int> imageTriggers;
        private Vector2 imagePosition;

        public ImagePopup(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            if (!(this is ImageMessage))
            {
                canvas = spriteSheet.GetSubSprite(new Rectangle(405, 245, 400, 257));
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            imageBuffer = new List<Sprite>();

            imagePosition = new Vector2(canvasPosition.X - canvas.SourceRectangle.Value.Width / 2 + ImagePositionOffset.X,
                                        canvasPosition.Y - canvas.SourceRectangle.Value.Height / 2 + ImagePositionOffset.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(imageBuffer[0].Texture,
                             imagePosition,
                             imageBuffer[0].SourceRectangle,
                             Color.White,
                             0f,
                             Vector2.Zero,
                             1f,
                             SpriteEffects.None,
                             ImageLayerDepth);
        }

        public void SetImages(List<Sprite> images, List<int> imageTriggers)
        {
            SetImages(images.ToArray());
            this.imageTriggers = imageTriggers;
            imageTriggerIndex = 0;
        }

        public void SetImages(params Sprite[] images)
        {
            imageBuffer = images.ToList<Sprite>();
        }

        protected override void Hide()
        {
            TextToSpeech.Stop();
            TextUtils.RefreshTextScrollBuffer();

            if (useScrolling
                && scrollingFinished
                && tempTimer < 0)
            {
                if ((!UpdateTextBuffer()
                    || !UpdateImageBuffer())
                    && usePause)
                {
                    Game1.Paused = false;
                }
            }
            else if (!useScrolling
                && tempTimer < 0)
            {
                if ((!UpdateTextBuffer()
                    || !UpdateImageBuffer())
                    && usePause)
                {
                    Game1.Paused = false;
                }
            }
        }

        /// <summary>
        /// Return true if more text is available to display, returns false otherwise
        /// </summary>
        /// <returns></returns>
        private bool UpdateImageBuffer()
        {
            if (imageBuffer.Count > 0)
            {
                imageBuffer.Remove(imageBuffer[0]);
            }

            if (imageBuffer.Count <= 0)
            {
                return false;
            }

            else
            {
                imageTriggerIndex++;
                
                if (imageTriggers.Count > 0 &&
                    imageTriggerIndex == imageTriggers[0])
                {
                    imageTriggers.RemoveAt(0);
                    imageBuffer.RemoveAt(0);
                }
            }

            return true;
        }
    }
}
