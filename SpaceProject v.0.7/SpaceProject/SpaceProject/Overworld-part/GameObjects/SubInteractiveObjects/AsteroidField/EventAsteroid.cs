using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class EventAsteroid : Asteroid
    {
        public EventAsteroid(Game1 Game, Sprite spriteSheet, Vector2 coordinates, String name) :
            base(Game, spriteSheet, coordinates)
        {
            this.name = name;
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1130, 54, 55));
            position = MathFunctions.CoordinateToPosition(coordinates);
            base.Initialize();

            overworldEvent = EventGenerator.GetRandomCommonEvent(Game);
        }

        protected override void SetClearedText()
        {
            clearedText = "Not applicable?";
        }
    }
}
