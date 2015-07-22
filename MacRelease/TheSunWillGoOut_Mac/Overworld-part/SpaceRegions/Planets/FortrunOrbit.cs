using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class FortrunOrbit : Outpost
    {
        public FortrunOrbit(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            name = "Fortrun Orbit";

            spaceRegionArea = new Rectangle(90000, 93500, 4000, 2500);

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
