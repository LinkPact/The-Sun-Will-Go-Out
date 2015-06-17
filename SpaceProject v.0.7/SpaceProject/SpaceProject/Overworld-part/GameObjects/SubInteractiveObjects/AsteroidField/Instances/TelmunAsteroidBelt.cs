using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class TelmunAsteroidBelt : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(-2800, -1500);
        private static readonly double RADIUS = 150;
        private static readonly double INNERRADIUS = 50;
        private static readonly int SIMPLE_ASTEROID_COUNT = 40;

        public TelmunAsteroidBelt(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {
            SetupGenericAsteroidFieldParams(RADIUS, INNERRADIUS);
            GenerateGenericAsteroids(SIMPLE_ASTEROID_COUNT);
        }
    }
}
