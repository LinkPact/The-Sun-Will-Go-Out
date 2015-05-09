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

    abstract class AsteroidField
    {
        private Game1 Game;
        private Sprite spriteSheet;

        private List<SubInteractiveObject> overworldObjects = new List<SubInteractiveObject>();
        private Vector2 centerCoordinate;
        public AsteroidField(Game1 Game, Sprite spriteSheet, Vector2 centerCoordinate)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.centerCoordinate = centerCoordinate;
        }

        /// <summary>
        /// Retrieves a list of asteroid objects which can be added directly to the overworld
        /// </summary>
        public List<SubInteractiveObject> GetAsteroids()
        {
            if (overworldObjects.Count == 0)
            {
                throw new ArgumentException("No objects assigned to asteroid field!");
            }

            return overworldObjects;
        }

        protected void SetupGenericAsteroidField(int count, double radius, double innerRadius = 0)
        {
            var coords = GetRandomCoordinateFields(count, radius, innerRadius);
            AssignGenericAsteroids(coords);
        }

        private void AssignGenericAsteroids(List<Vector2> coordList)
        {
            for (int n = 0; n < coordList.Count; n++)
            {
                overworldObjects.Add(new SimpleAsteroid(Game, spriteSheet, GetAbsCoord(coordList[n]), "asteroid" + n));
            }
        }

        private Vector2 GetAbsCoord(Vector2 relativeCoord)
        {
            return centerCoordinate + relativeCoord;
        }

        private List<Vector2> GetRandomCoordinateFields(int count, double radius, double emptyInnerRadius = 0)
        {
            List<Vector2> coordList = new List<Vector2>();

            for (int n = 0; n < count; n++)
            {
                double r = MathFunctions.GetExternalRandomDouble() * (radius - emptyInnerRadius) + emptyInnerRadius;
                double rad = MathFunctions.GetExternalRandomDouble() * Math.PI * 2;

                Vector2 coord = new Vector2((float)(MathFunctions.DirFromRadians(rad).X * r),
                    (float)(MathFunctions.DirFromRadians(rad).Y * r));

                coordList.Add(coord);
            }

            return coordList;
        }
    }
}
