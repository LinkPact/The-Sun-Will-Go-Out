using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    //Button used to run the current level
    class RunLevelButton : MapCreatorButton
    {
        public RunLevelButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 175, 61, 31));
        }

        public override void ClickAction()
        {
            AddAction(new SaveLevelAction());
            AddAction(new RunLevelAction());
        }
    }
}
