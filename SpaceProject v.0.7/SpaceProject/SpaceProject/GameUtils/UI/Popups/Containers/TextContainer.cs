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

        // Variables
        private List<string> textBuffer;

        private SpriteFont font;
        private Vector2 fontOffset;
        private Color fontColor;
        private String text;
        private Vector2 textPosition;

        private Rectangle canvasSize;
        private int containerWidth;

        private bool useScrolling;
        private bool flushScrollingText;
        private bool textScrollingFinished;

        // Properties
        public bool UseScrolling { get { return useScrolling; } set { useScrolling = value; } }

        public TextContainer(Game1 game, Rectangle canvasSize):
            base(game)
        {
            this.canvasSize = canvasSize;
        }

        public override void Initialize()
        {
            base.Initialize();

            textBuffer = new List<String>();

            font = FontManager.GetFontStatic(14);
            fontOffset = FontManager.FontOffsetStatic;
            fontColor = FontManager.FontColorStatic;

            // Sets position of text
            if (GameStateManager.currentState == "OverworldState")
            {
                textPosition = new Vector2(game.camera.cameraPos.X - canvasSize.Width / 2,
                                           game.camera.cameraPos.Y - canvasSize.Height / 2 - 5);
            }
            else
            {
                textPosition = new Vector2(game.Window.ClientBounds.Width / 2 - canvasSize.Width / 2,
                                           game.Window.ClientBounds.Height / 2 - canvasSize.Height / 2 - 5);
            }
            containerWidth = canvasSize.Width - 20;   
        }

        public override void Update(GameTime gameTime)
        {
            if (textBuffer.Count > 0)
            {
                text = TextUtils.WordWrap(font,
                                          TextUtils.ScrollText(textBuffer[0],
                                                               flushScrollingText,
                                                               out textScrollingFinished),
                                          containerWidth);
            }
        }

        public void UpdatePosition(Vector2 cameraPos)
        {
            textPosition = new Vector2(cameraPos.X - canvasSize.Width / 2,
                                           cameraPos.Y - canvasSize.Height / 2 - 5);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,
                        text,
                        new Vector2(textPosition.X,
                                    textPosition.Y) + fontOffset,
                        fontColor,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        TextLayerDepth);
        }

        public bool IsTextBufferEmpty()
        {
            return textBuffer.Count <= 0;
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
    }
}
