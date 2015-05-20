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
        private static readonly double INNERRADIUS = 750;
        private static readonly int SIMPLE_ASTEROID_COUNT = 150;
        private static readonly int EVENT_ASTEROID_COUNT = 15;

        public SunAsteroidBelt(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidFieldParams(RADIUS, INNERRADIUS);

            GenerateGenericAsteroids(SIMPLE_ASTEROID_COUNT);
            GenerateEventAsteroids(EVENT_ASTEROID_COUNT);
        }
    }
}
