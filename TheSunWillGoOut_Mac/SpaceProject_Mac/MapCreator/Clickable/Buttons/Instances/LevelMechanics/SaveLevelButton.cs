using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    //Button used to save the current level
    class SaveLevelButton : MapCreatorButton
    {
        public SaveLevelButton(Game1 game, Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 51, 61, 31));
        }

        public override void ClickAction()
        {
            AddAction(new SaveLevelAction());
        }
    }
}
