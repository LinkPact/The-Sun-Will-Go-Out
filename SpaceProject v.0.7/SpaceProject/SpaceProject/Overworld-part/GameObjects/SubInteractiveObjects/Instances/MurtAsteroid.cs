using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class MurtAsteroid : SubInteractiveObject
    {

        public MurtAsteroid(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(1499, 915, 142, 159));

            name = "Mining Asteroids";

            position = MathFunctions.CoordinateToPosition(new Vector2(1100, 100));
            scale = 1f;
            color = Color.White;
            layerDepth = 0.5f;

            base.Initialize();
            overworldEvent = new DisplayTextOE("A group of asteroids used for mining.");
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
