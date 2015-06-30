using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class WesternAsteroidField : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(-1700, 0);
        private static readonly double RADIUS = 200;
        private static readonly int COUNT = 20;
        private static readonly int EVENT_ASTEROID_COUNT = 8;

        public WesternAsteroidField(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidFieldParams(RADIUS);
            GenerateGenericAsteroids(COUNT);
            GenerateEventAsteroids(EVENT_ASTEROID_COUNT);
        }
    }
}
