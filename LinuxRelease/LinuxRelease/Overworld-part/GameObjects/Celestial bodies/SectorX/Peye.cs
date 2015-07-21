using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Peye : Planet
    {
        public Peye(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {          
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(308, 692, 177, 177));

            base.Initialize();

            // SX_PoisonPlanet
            PlanetCodeName = "SX_Peye";
            ColonyCodeName = "SX_Peye_Colony1";

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
