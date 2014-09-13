using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class WidthInPixelsButton : MapCreatorButton
    {
        public WidthInPixelsButton(Sprite spriteSheet, Vector2 position, int initialDuration)
            : base(spriteSheet, position)
        {
            displayText = initialDuration.ToString();
            displayText = "";

            displaySprite = spriteSheet.GetSubSprite(new Rectangle(99, 20, 14, 14));
        }

        public override void ClickAction()
        {
            int newDuration = GetNewInt("Enter new level width:", "Level width", "");

            if (newDuration != -1)
            {
                AddAction(new SetLevelWidthInPixelsAction(newDuration));
            }
        }
    }
}
