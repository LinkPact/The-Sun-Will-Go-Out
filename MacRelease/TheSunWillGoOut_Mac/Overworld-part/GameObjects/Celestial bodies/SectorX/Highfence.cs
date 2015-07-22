using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class Highfence : Planet
    {
        public Highfence(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet, positionOffset)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(1, 835, 244, 243));
            base.Initialize();

            // Old name: SX_DustPlanet
            PlanetCodeName = "SX_Highfence";
            ColonyCodeName = "SX_Highfence_Colony1";

            LoadPlanetData(PlanetCodeName);
        }
    }
}
