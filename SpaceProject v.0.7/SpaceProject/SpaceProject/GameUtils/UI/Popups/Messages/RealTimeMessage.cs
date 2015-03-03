using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RealTimeMessage : TextMessage
    {
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
            displayTime = -1;

            textContainer = new TextContainer(game, canvas.SourceRectangle.Value);
            textContainer.Initialize();
            textContainer.UseScrolling = true;
            usePause = false;
        }

        public override void Update(GameTime gameTime)
        {
            canvasPosition = new Vector2(game.camera.cameraPos.X,
                                         game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 4);

            textContainer.Update(gameTime);

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

            textContainer.Draw(spriteBatch);
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
