using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RealTimeMessage : Popup
    {
        private readonly float TextLayerDepth = 1f;

        private float showInTime;       // How many seconds in the future this message will be shown
        private float displayTime;      // Time when this message will be shown
        private float timeShown;        // How many milliseconds this message is shown
        private float hideTime;         // Time when this message will be hidden

        public RealTimeMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(354, 0, 400, 128));
        }

        public override void Initialize()
        {
            base.Initialize();
            displayTime = -1;
            useScrolling = true;
            usePause = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            canvasPosition = new Vector2(game.camera.cameraPos.X,
                                         game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 4);

            textPosition = new Vector2(canvasPosition.X - canvas.Width / 2 + 30,
                                       canvasPosition.Y - canvas.Height / 2 + 30);

            text = TextUtils.WordWrap(game.fontManager.GetFont(14),
                                      TextUtils.ScrollText(textBuffer[0],
                                                           flushScrollingText,
                                                           out textScrollingFinished),
                                      (int)Math.Round(((float)canvas.SourceRectangle.Value.Width),
                                      0));

            if (displayTime == -1)
            {
                displayTime = StatsManager.PlayTime.GetFutureOverworldTime(showInTime);
            }

            else if (displayTime != -2
                && StatsManager.PlayTime.HasOverworldTimePassed(displayTime))
            {
                Show();
                displayTime = -2;
            }

            else if (StatsManager.PlayTime.HasOverworldTimePassed(hideTime))
            {
                Hide();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(game.fontManager.GetFont(14),
                                   text,
                                   new Vector2(textPosition.X,
                                               textPosition.Y) + game.fontManager.FontOffset,
                                   game.fontManager.FontColor,
                                   0f,
                                   Vector2.Zero,
                                   1f,
                                   SpriteEffects.None,
                                   TextLayerDepth);
        }

        public override void Show()
        {
            base.Show();

            hideTime = StatsManager.PlayTime.GetFutureOverworldTime(timeShown);
        }

        public void SetTimeShown(float milliseconds)
        {
            timeShown = milliseconds;
        }

        public void SetDisplayTime(float milliseconds)
        {
            showInTime = milliseconds;
        }
    }
}
