using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    /// <summary>
    /// Can be used to generate a list of generic asteroids around a particular location
    /// </summary>

    class AsteroidField
    {
        private List<SubInteractiveObject> overworldObjects = new List<SubInteractiveObject>();
        private Vector2 centerCoordinate;
        private readonly int RADIUS = 200;
        private readonly int ASTEROID_COUNT = 100;

        public AsteroidField(Game1 Game, Sprite spriteSheet)
        {
            this.centerCoordinate = new Vector2(1000, 0);
            var coordList = GetCoordinateList();

            for (int n = 0; n < coordList.Count; n++)
            {
                overworldObjects.Add(new SimpleAsteroid(Game, spriteSheet, GetAbsCoord(coordList[n]), "asteroid" + n));
            }
        }

        public List<SubInteractiveObject> GetAsteroids()
        {
            return overworldObjects;
        }

        private List<Vector2> GetCoordinateList()
        {
            List<Vector2> coordList = new List<Vector2>();

            for (int n = 0; n < ASTEROID_COUNT; n++)
            {
                double r = MathFunctions.GetExternalRandomDouble() * RADIUS;
                double rad = MathFunctions.GetExternalRandomDouble() * Math.PI * 2;

                Vector2 coord = new Vector2((float)(MathFunctions.DirFromRadians(rad).X * r),
                    (float)(MathFunctions.DirFromRadians(rad).Y * r));

                coordList.Add(coord);
            }

            return coordList;
        }

        private Vector2 GetAbsCoord(Vector2 relativeCoord) 
        {
            return centerCoordinate + relativeCoord;
        }
    }
}
