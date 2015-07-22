using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class LavisOrbit : Outpost
    {
        public LavisOrbit(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Lavis Orbit";

            spaceRegionArea = new Rectangle(111500 , 85000, 2500, 2000);

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
