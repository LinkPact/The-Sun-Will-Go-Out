using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class LargeStar : Star
    {
        public LargeStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            speedMod = 0.30f;
        }
    }
}
