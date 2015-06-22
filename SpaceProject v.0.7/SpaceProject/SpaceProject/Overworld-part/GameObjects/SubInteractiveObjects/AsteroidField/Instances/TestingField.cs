﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class TestingField : AsteroidField
    {
        private static readonly Vector2 CENTERCOORD = new Vector2(1700, 0);
        private static readonly double RADIUS = 50;
        private static readonly int COUNT = 0;
        private static readonly int EVENT_ASTEROID_COUNT = 20;

        public TestingField(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, CENTERCOORD)
        {            
            SetupGenericAsteroidFieldParams(RADIUS);
            GenerateGenericAsteroids(COUNT);
            GenerateEventAsteroids(EVENT_ASTEROID_COUNT);
        }
    }
}