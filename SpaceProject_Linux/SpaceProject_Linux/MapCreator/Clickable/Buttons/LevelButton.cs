using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    abstract class LevelButton : MapCreatorButton
    {
        public LevelButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        { }

        public abstract void ClickAction(LevelMechanics level);
    }
}
