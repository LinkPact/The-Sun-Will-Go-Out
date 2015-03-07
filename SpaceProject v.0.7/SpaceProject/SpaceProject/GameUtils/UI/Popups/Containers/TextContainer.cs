using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class TextContainer : Container
    {
        // Constants
        private readonly float TextLayerDepth = 1f;
        private readonly Vector2 CanvasTextAreaSize = new Vector2(352, 114);

        private readonly Vector2 TextMessageTextOffset = new Vector2(26, 26);
        private readonly Vector2 RealTimePortraitTextOffset = new Vector2(192, 78);
        private readonly Vector2 PortraitTextOffset = new Vector2(192, 64);
        private readonly Vector2 ImageMessageTextOffset = new Vector2(26, 242);

        // Variables
        private List<string> textBuffer;

        private SpriteFont font;
        private Vector2 fontOffset;
        private Color fontColor;
        private String text;

        private int containerWidth;

        private bool useScrolling;
        private bool flushScrollingText;
        private bool textScrollingFinished;

        // Properties
        public bool UseScrolling { get { return useScrolling; } set { useScrolling = value; } }

        public TextContainer(Vector2 canvasPosition, Rectangle canvasRectangle):
            base(canvasPosition, canvasRectangle) { }

        public void Initialize()
        {
            textBuffer = new List<String>();

            font = FontManager.GetFontStatic(14);
            fontOffset = FontManager.FontOffsetStatic;
            fontColor = FontManager.FontColorStatic;

            containerWidth = (int)CanvasTextAreaSize.X;   
        }

        public override void Update(GameTime gameTime, Vector2 canvasPosition)
        {
            base.Update(gameTime, canvasPosition);

            if (textBuffer.Count > 0)
            {
                if (useScrolling)
                {
                    text = TextUtils.WordWrap(font,
                          TextUtils.ScrollText(textBuffer[0],
                                               flushScrollingText,
                                               out textScrollingFinished),
                          containerWidth);
                }
                else
                {
                    text = TextUtils.WordWrap(font,
                                              textBuffer[0],
                                              containerWidth);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (text != null)
            {
                spriteBatch.DrawString(font,
                            text,
                            new Vector2(position.X, position.Y) + fontOffset,
                            fontColor,
                            0f,
                            Vector2.Zero,
                            1f,
                            SpriteEffects.None,
                            TextLayerDepth);
            }
        }

        public void UpdateTextBuffer()
        {
            if (textScrollingFinished)
            {
                TextUtils.RefreshTextScrollBuffer();
                TextToSpeech.Stop();

                if (textBuffer.Count > 0)
                {
                    textBuffer.Remove(textBuffer[0]);
                }

                if (textBuffer.Count > 0)
                {
                    TextToSpeech.Speak(textBuffer[0]);
                }

                flushScrollingText = false;
            }
            else
            {
                flushScrollingText = true;
            }
        }

        public void SetMessage(params string[] text)
        {
            foreach (string str in text)
            {
                if (!str.Contains('#'))
                {
                    textBuffer.Add(str);
                }
                else
                {
                    List<String> tempList = TextUtils.SplitHashTagText(str);
                    foreach (string str2 in tempList)
                    {
                        textBuffer.Add(str2);
                    }
                }
            }
        }

        public string GetCurrentMessage()
        {
            if (textBuffer.Count > 0)
            {
                return textBuffer[0];
            }
            return "EMPTY";
        }

        public bool IsTextBufferEmpty()
        {
            return textBuffer.Count <= 0;
        }

        public bool HasScrollingFinished()
        {
            return textScrollingFinished;
        }

        public override void SetDefaultPosition(Type type)
        {
            if (type == typeof(ImageMessage))
            {
                offset = ImageMessageTextOffset;
            }
            else if (type == typeof(PortraitMessage))
            {
                offset = PortraitTextOffset;
            }
            else if (type == typeof(RealTimePortraitMessage))
            {
                offset = RealTimePortraitTextOffset;
            }
            else if (type == typeof(RealTimeMessage)
                || type == typeof(TextMessage)
                || type == typeof(SelectionMenu))
            {
                offset = TextMessageTextOffset;
            }
            else
            {
                throw new ArgumentException("Invalid type.");
            }

            SetPosition();
        }
    }
}
