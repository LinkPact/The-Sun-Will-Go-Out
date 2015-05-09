using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class SunAsteroidBelt : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(0, 0);
        private static readonly double RADIUS = 1000;
        private static readonly double INNERRADIUS = 900;
        private static readonly int COUNT = 300;

        public SunAsteroidBelt(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidField(COUNT, RADIUS, INNERRADIUS);
        }
    }
}
