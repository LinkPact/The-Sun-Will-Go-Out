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
        private Sprite unselectedButton;
        private Sprite selectedButton;

        private int tempTimer;

        protected bool pauseGame;

        protected Popup(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            unselectedButton = spriteSheet.GetSubSprite(new Rectangle(112, 0, 66, 21));
            selectedButton = spriteSheet.GetSubSprite(new Rectangle(180, 0, 66, 21));
        }

        public virtual void Initialize()
        {
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

        public void Show()
        {
            if (pauseGame)
            {
                Game1.Paused = true;

                TextToSpeech.Speak(textBuffer[0], TextToSpeech.DefaultRate);
            }
        }

        public void SetDelay(float milliseconds)
        {
            popupTime = StatsManager.PlayTime.GetFuturePlayTime(milliseconds);
        }

        public virtual void OnPress()
        {
            if (tempTimer > 0)
            {
                return;
            }
        }

        protected virtual void Hide()
        {
            if (pauseGame)
            {
                Game1.Paused = false;
            }
        }
    }
}
