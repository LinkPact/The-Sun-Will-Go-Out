using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RebelOutpostAsteroidField : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(-700, -1200);
        private static readonly double RADIUS = 200;
        private static readonly int COUNT = 20;
        private static readonly int EVENT_ASTEROID_COUNT = 4;

        public RebelOutpostAsteroidField(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidFieldParams(RADIUS);
            
            GenerateGenericAsteroids(COUNT);
            GenerateEventAsteroids(EVENT_ASTEROID_COUNT);
        }
    }
}
