using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class OptionEnemySquare : OptionSquare
    {
        private EnemyType optionState;

        public OptionEnemySquare(Sprite spriteSheet, Vector2 position, EnemyType state) 
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
            optionState = state;
            color = DataConversionLibrary.GetSquareColor(state);
        }

        public override void SetDisplay(ActiveSquare display)
        {
            ((ActiveEnemySquare)display).SetDisplay(optionState);
            readyToSetDisplay = false; 
        }
    }
}
