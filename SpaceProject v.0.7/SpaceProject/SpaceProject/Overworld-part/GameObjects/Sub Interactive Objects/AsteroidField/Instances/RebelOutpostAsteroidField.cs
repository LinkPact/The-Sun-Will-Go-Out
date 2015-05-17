using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RebelOutpostAsteroidField : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(1000, 0);
        private static readonly double RADIUS = 300;
        private static readonly int COUNT = 40;

        public RebelOutpostAsteroidField(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidField(COUNT, RADIUS);
        }
    }
}
