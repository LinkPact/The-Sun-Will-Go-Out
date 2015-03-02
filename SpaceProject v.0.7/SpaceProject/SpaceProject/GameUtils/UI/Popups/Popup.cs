using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class Popup
    {
        private readonly int Opacity = 230;
        private readonly float LayerDepth = 0.95f;

        protected Game1 game;

        protected Sprite canvas;
        protected Vector2 canvasPosition;
        protected float canvasScale;

        protected String text;
        protected Vector2 textPosition;
        protected bool flushScrollingText;
        protected bool textScrollingFinished;

        protected List<String> textBuffer;

        private float popupTime;
        private Sprite selectedButton;

        protected int tempTimer;
        protected bool useScrolling;
        protected bool scrollingFinished;
        private bool flushScrollText;

        protected bool usePause;

        private bool finished;
        public bool Finished { get { return finished; } private set { ; } }

        protected Popup(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            selectedButton = spriteSheet.GetSubSprite(new Rectangle(180, 0, 66, 21));
        }

        public virtual void Initialize()
        {
            if (!(this is RealTimeMessage))
            {
                usePause = true;
            }

            textBuffer = new List<String>();

            // Sets position of canvas and text
            if (GameStateManager.currentState == "OverworldState")
            {
                canvasPosition = new Vector2(game.camera.cameraPos.X, game.camera.cameraPos.Y);
                textPosition = new Vector2(game.camera.cameraPos.X - canvas.SourceRectangle.Value.Width / 2,
                                           game.camera.cameraPos.Y - canvas.SourceRectangle.Value.Height / 2 - 5);
            }
            else
            {
                canvasPosition = new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
                textPosition = new Vector2(game.Window.ClientBounds.Width / 2 - canvas.SourceRectangle.Value.Width / 2,
                                           game.Window.ClientBounds.Height / 2 - canvas.SourceRectangle.Value.Height / 2 - 5);
            }

            tempTimer = 5;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (popupTime != -1
                && StatsManager.PlayTime.HasPlayTimePassed(popupTime))
            {
                Show();
                popupTime = -1;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Draws canvas
            spriteBatch.Draw(canvas.Texture,
                 canvasPosition,
                 canvas.SourceRectangle,
                 new Color(255, 255, 255, Opacity),
                 0.0f,
                 new Vector2(canvas.SourceRectangle.Value.Width / 2,
                             canvas.SourceRectangle.Value.Height / 2),
                 canvasScale,
                 SpriteEffects.None,
                 LayerDepth);

            // Draws "okay"-button
            spriteBatch.Draw(selectedButton.Texture,
                new Vector2(canvasPosition.X, canvasPosition.Y),
                selectedButton.SourceRectangle,
                Color.White,
                0f,
                new Vector2(selectedButton.SourceRectangle.Value.Width / 2,
                    selectedButton.SourceRectangle.Value.Height / 2),
                1f,
                SpriteEffects.None,
                0.975f);

            // Draws "okay"-text
            spriteBatch.DrawString(game.fontManager.GetFont(14),
                 "Okay",
                 new Vector2(canvasPosition.X,
                         canvasPosition.Y) + game.fontManager.FontOffset,
                 Color.LightBlue,
                 0f,
                 game.fontManager.GetFont(14).MeasureString("Okay") / 2,
                 1f,
                 SpriteEffects.None,
                 1f);
        }

        public virtual void Show()
        {
            if (usePause)
            {
                Game1.Paused = true;

                TextToSpeech.Speak(textBuffer[0], TextToSpeech.DefaultRate);
            }
        }

        public virtual void OnPress(RebindableKeys key)
        {
            if (tempTimer > 0)
            {
                return;
            }

            else if (key == RebindableKeys.Action1)
            {
                if (useScrolling
                    && !scrollingFinished)
                {
                    flushScrollText = true;
                }
            }
        }

        public void SetMessage(params string[] text)
        {
            foreach (string str in text)
            {
                if (!str.Contains('#'))
                {
                    textBuffer.Add(text[0]);
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

        public void SetDelay(float milliseconds)
        {
            popupTime = StatsManager.PlayTime.GetFuturePlayTime(milliseconds);
        }

        protected virtual void Hide()
        {
            TextToSpeech.Stop();
            TextUtils.RefreshTextScrollBuffer();

            if (useScrolling
                && scrollingFinished
                && tempTimer < 0)
            {
                if (!UpdateTextBuffer()
                    && usePause)
                {
                    Game1.Paused = false;
                    finished = true;
                }
            }
            else if (!useScrolling
                && tempTimer < 0)
            {
                if (!UpdateTextBuffer()
                    && usePause)
                {
                    Game1.Paused = false;
                    finished = true;
                }
            }
        }

        /// <summary>
        /// Return true if more text is available to display, returns false otherwise
        /// </summary>
        /// <returns></returns>
        protected bool UpdateTextBuffer()
        {
            if (textBuffer.Count > 0)
            {
                textBuffer.Remove(textBuffer[0]);
            }

            if (textBuffer.Count <= 0)
            {
                return false;
            }

            else
            {
                TextToSpeech.Speak(textBuffer[0]);
            }

            scrollingFinished = false;
            flushScrollText = false;

            return true;
        }
    }
}
