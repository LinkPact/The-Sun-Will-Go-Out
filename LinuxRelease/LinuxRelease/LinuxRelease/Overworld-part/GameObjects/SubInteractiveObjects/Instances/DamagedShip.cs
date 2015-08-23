using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class DamagedShip : SubInteractiveObject
    {
        public DamagedShip(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {}

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(15, 55, 91, 37));
            position = new Vector2(100000, 110000);
            name = "Destroyed Ship";

            base.Initialize();

            overworldEvent = new DisplayTextOE("You find the remnants of a ship drifting through space. The ship seems to be of civilian origin and the hull shows signs of blast damage. But you are unable to determine who the attacker might have been.");
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
