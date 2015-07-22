using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class SimpleAsteroid : Asteroid
    {
        public SimpleAsteroid(Game1 Game, Sprite spriteSheet, Vector2 coordinates, String name) :
            base(Game, spriteSheet, coordinates)
        {
            this.name = name;
        }

        public override void Initialize()
        {

            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1075, 54, 55));
            base.Initialize();

            overworldEvent = new DisplayTextOE("A simple asteroid floating in space.");
        }

        protected override void SetClearedText()
        {
            clearedText = "Not applicable?";
        }
    }
}
