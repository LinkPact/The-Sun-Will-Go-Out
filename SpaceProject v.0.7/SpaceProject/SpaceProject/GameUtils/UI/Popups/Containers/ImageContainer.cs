using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ImageContainer : Container
    {
        // constants
        private readonly Point ImagePositionOffset = new Point(16, 18);
        private readonly float ImageLayerDepth = 0.96f;

        // variables
        private List<Sprite> imageBuffer;
        private int imageTriggerIndex;
        private List<int> imageTriggers;
        private Vector2 imagePosition;

        private Rectangle canvasSize;
        private Vector2 canvasPosition;

        public ImageContainer(Game1 game, Rectangle canvasSize, Vector2 canvasPosition) :
            base(game)
        {
            this.canvasSize = canvasSize;
            this.canvasPosition = canvasPosition;
        }

        public override void Initialize()
        {
            base.Initialize();

            imageBuffer = new List<Sprite>();

            imagePosition = new Vector2(canvasPosition.X - canvasSize.Width / 2 + ImagePositionOffset.X,
                                        canvasPosition.Y - canvasSize.Height / 2 + ImagePositionOffset.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
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

        public void SetImages(params Sprite[] images)
        {
            foreach (Sprite image in images)
            {
                imageBuffer.Add(image);
            }
        }

        public void SetImageTriggers(params int[] imageTriggers)
        {
            foreach (int trigger in imageTriggers)
            {
                this.imageTriggers.Add(trigger);
            }

            imageTriggerIndex = 0;
        }

        public bool IsImageBufferEmpty()
        {
            UpdateImageBuffer();

            if (imageBuffer.Count <= 0)
            {
                return true;
            }
            return false;
        }

        private void UpdateImageBuffer()
        {
            if (imageBuffer.Count > 0)
            {
                imageBuffer.Remove(imageBuffer[0]);
            }

            if (imageBuffer.Count > 0)
            {
                imageTriggerIndex++;

                if (imageTriggers.Count > 0 &&
                    imageTriggerIndex == imageTriggers[0])
                {
                    imageTriggers.RemoveAt(0);
                    imageBuffer.RemoveAt(0);
                }
            }
        }
    }
}
