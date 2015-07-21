using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class DurationButton : MapCreatorButton
    {
        public DurationButton(Sprite spriteSheet, Vector2 position, float initialDuration)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(99, 20, 14, 14));
        }

        public override void ClickAction()
        {
            float newDuration = GetNewFloat("Enter new level duration:", "Level duration", "");

            if (newDuration != -1)
            {
                AddAction(new SetLevelDurationAction(newDuration));
            }
        }
    }
}
