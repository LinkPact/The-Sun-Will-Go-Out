using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class PortraitMessage : TextMessage
    {
        public PortraitMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            useScrolling = true;
            usePause = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
