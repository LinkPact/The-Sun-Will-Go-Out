using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class InformationStation : SubInteractiveObject
    {

        public InformationStation(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            position = MathFunctions.CoordinateToPosition(new Vector2(1000, 0));
            name = "Information Station";

            base.Initialize();

            overworldEvent = new DisplayTextOE("The station seems to be abandoned. There is a strange air about it though.");
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
