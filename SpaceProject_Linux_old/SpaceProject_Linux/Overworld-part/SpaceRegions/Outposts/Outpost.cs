using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class Outpost : SpaceRegion
    {
        protected Outpost(Game1 game, Sprite spriteSheet) :
            base (game, spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
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
