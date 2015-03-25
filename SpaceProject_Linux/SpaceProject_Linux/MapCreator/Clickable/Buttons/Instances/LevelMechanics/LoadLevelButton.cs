using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class LoadLevelButton : MapCreatorButton
    {
        public LoadLevelButton(Game1 game, Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 82, 61, 31));
        }

        public override void ClickAction()
        {
            AddAction(new LoadLevelAction());
        }
    }
}
