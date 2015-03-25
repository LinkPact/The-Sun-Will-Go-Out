using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class EditAmountButton : MapCreatorButton
    {
        private int min;
        private int max;

        public EditAmountButton(Sprite spriteSheet, Vector2 position, int min, int max)
            : base(spriteSheet, position)
        {
            this.min = min;
            this.max = max;
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(99, 20, 14, 14));
        }

        public override void ClickAction()
        {
            AddAction(new EditAmountAction(min, max));
        }
    }
}
