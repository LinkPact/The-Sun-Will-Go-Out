using System;
using System.Collections.Generic;

namespace SpaceProject
{
    class SmallStar : Star
    {
        public SmallStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            speedMod = 0.05f;
        }
    }
}
