using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class EditNameButton : MapCreatorButton
    {
        public EditNameButton(Game1 game, Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 113, 61, 31));
        }

        public override void ClickAction()
        {
            AddAction(new EditNameAction());
        }
    }
}
