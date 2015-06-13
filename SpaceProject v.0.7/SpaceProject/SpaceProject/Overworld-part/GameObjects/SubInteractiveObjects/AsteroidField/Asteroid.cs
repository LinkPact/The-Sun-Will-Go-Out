using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public abstract class Asteroid : SubInteractiveObject
    {
        protected Vector2 coordinates;

        public Asteroid(Game1 Game, Sprite spriteSheet, Vector2 coordinates) :
            base(Game, spriteSheet)
        {
            this.coordinates = coordinates;

            this.angle = (float)(MathFunctions.GetExternalRandomDouble() * 2 * Math.PI);
        }
    }
}
