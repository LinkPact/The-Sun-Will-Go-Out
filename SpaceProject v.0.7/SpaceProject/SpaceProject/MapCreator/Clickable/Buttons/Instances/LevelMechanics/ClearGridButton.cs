using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Button used to clear current grid
    class ClearGridButton : MapCreatorButton
    {   
        public ClearGridButton(Game1 game, Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(0, 144, 61, 31));
        }

        public override void ClickAction()
        {
            AddAction(new ClearGridAction());
        }
    }
}
