using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class MediumStar : Star
    {
        public MediumStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            speedMod = 0.2f;
        }
    }
}
