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
        private float timeShown;        // How many milliseconds this message is shown
        private float hideTime;         // Time when this message will be hidden

        public RealTimeMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(572, 645, 400, 128));
        }

        public override void Initialize()
        {
            base.Initialize();

            usePause = false;
            useOkayButton = false;
        }

        public override void Update(GameTime gameTime)
        {
            canvasPosition = new Vector2(game.camera.cameraPos.X,
                                         game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 4);

            textContainer.Update(gameTime);
            textContainer.UpdatePosition(game.camera.cameraPos);

            if (StatsManager.PlayTime.HasOverworldTimePassed(hideTime))
            {
                Hide();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void SetDelay(float milliseconds)
        {
            timeShown = milliseconds;   
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
    }
}
