using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class ImageContainer : Container
    {
        // constants
        private readonly float ImageLayerDepth = 0.96f;

        private readonly Vector2 PortraitOffset = new Vector2(18, 21);
        private readonly Vector2 ImageOffset = new Vector2(16, 15);

        // variables
        private List<Sprite> imageBuffer;
        public int ImageBufferCount { get { return imageBuffer.Count; } }

        private bool useImageTriggers;
        private int imageTriggerIndex;
        private List<int> imageTriggers;
        private int messageCount;

        public ImageContainer(Vector2 canvasPosition, Rectangle canvasRectangle) :
            base(canvasPosition, canvasRectangle) { }

        public void Initialize()
        {
            imageBuffer = new List<Sprite>();
            imageTriggers = new List<int>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (imageBuffer.Count > 0)
            {
                spriteBatch.Draw(imageBuffer[0].Texture,
                                 position,
                                 imageBuffer[0].SourceRectangle,
                                 Color.White,
                                 0f,
                                 Vector2.Zero,
                                 1f,
                                 SpriteEffects.None,
                                 ImageLayerDepth);
            }
        }

        public void SetImages(params Sprite[] images)
        {
            foreach (Sprite image in images)
            {
                imageBuffer.Add(image);
            }
        }

        public void SetImageTriggers(int numberOfMessages, params int[] imageTriggers)
        {
            messageCount = numberOfMessages;
            useImageTriggers = true;

            foreach (int trigger in imageTriggers)
            {
                this.imageTriggers.Add(trigger);
            }

            imageTriggerIndex = 0;
        }

        public void UpdateImageBuffer()
        {
            if (imageBuffer.Count > 0)
            {
                if (useImageTriggers)
                {
                    imageTriggerIndex++;

                    if (imageTriggers.Count > 0 &&
                        imageTriggerIndex == imageTriggers[0])
                    {
                        imageTriggers.RemoveAt(0);
                        imageBuffer.RemoveAt(0);
                    }
                    else if (imageTriggerIndex >= messageCount)
                    {
                        imageBuffer.RemoveAt(0);
                    }
                }
                else
                {
                    imageBuffer.Remove(imageBuffer[0]);
                }
            }
        }

        public override void SetDefaultPosition(Type type)
        {
            if (type == typeof(ImageMessage)
                || type == typeof(ImagePopup))
            {
                offset = ImageOffset;
            }
            else if (type == typeof(RealTimePortraitMessage)
                || type == typeof(PortraitMessage))
            {
                offset = PortraitOffset;
            }
            else
            {
                throw new ArgumentException("Invalid type.");
            }

            SetPosition();
        }
    }
}
