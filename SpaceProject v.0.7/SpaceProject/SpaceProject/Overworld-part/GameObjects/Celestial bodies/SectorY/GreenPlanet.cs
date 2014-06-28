using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class GreenPlanet : Planet
    {
        public GreenPlanet(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(705, 22, 403, 403));

            base.Initialize();

            PlanetCodeName = "SY_GreenPlanet";
            ColonyCodeName = "SY_GreenPlanet_Colony1";

            LoadPlanetData(PlanetCodeName);
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
