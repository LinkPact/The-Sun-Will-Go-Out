using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class LavisAsteroidBelt : AsteroidField
    {
        // Lavis position: 28000 1000

        private static readonly Vector2 CENTERCOORD = new Vector2(1300, 1400);
        private static readonly double RADIUS = 150;
        private static readonly double INNERRADIUS = 50;
        private static readonly int SIMPLE_ASTEROID_COUNT = 20;
        private static readonly int EVENT_ASTEROID_ITEM_COUNT = 1;
        private static readonly int EVENT_ASTEROID_TEXT_COUNT = 2;
        private static readonly int EVENT_ASTEROID_AMBUSH_COUNT = 2;

        public LavisAsteroidBelt(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {
            SetupGenericAsteroidFieldParams(RADIUS, INNERRADIUS);
            GenerateGenericAsteroids(SIMPLE_ASTEROID_COUNT);
            GenerateEventAsteroids(EVENT_ASTEROID_ITEM_COUNT, EVENT_ASTEROID_TEXT_COUNT, EVENT_ASTEROID_AMBUSH_COUNT);
        }
    }
}
