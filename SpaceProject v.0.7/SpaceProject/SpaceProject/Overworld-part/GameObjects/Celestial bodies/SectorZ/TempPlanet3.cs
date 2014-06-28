using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class TempPlanet3: Planet
    {
        public TempPlanet3(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 21, 237, 237));

            base.Initialize();

            PlanetCodeName = "SZ_TempPlanet3";
            ColonyCodeName = "SZ_TempPlanet3_Colony1";

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
