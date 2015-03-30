using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class StartTimeButton : MapCreatorButton
    {
        public StartTimeButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            //displayText = initialDuration.ToString();
            displayText = "";

            displaySprite = spriteSheet.GetSubSprite(new Rectangle(99, 20, 14, 14));
        }

        public override void ClickAction()
        {
            int newStartTime = GetNewInt("Enter new testing starttime:", "Start time", "");

            if (newStartTime != -1)
            {
                AddAction(new SetStartTimeAction(newStartTime));
            }
        }

    }
}
