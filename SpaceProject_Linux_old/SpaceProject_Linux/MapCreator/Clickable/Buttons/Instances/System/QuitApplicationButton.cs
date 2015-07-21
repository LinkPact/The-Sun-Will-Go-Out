using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    //Button used to close down application.
    class QuitApplicationButton : MapCreatorButton
    {
        public QuitApplicationButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 20, 61, 31));  
        }

        public override void ClickAction()
        {
            AddAction(new QuitApplicationAction());
        }
    }
}
