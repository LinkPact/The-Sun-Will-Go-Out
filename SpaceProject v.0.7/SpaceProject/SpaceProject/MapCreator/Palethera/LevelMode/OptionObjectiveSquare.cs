using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class OptionObjectiveSquare : OptionSquare
    {
        private LevelObjective optionState;

        public OptionObjectiveSquare(Sprite spriteSheet, Vector2 position, LevelObjective state) 
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
            optionState = state;
            readyToSetDisplay = false;     
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLeftClicked())
            {
                readyToSetDisplay = true;
                //ActiveData.isPointActive = true;
            }
        }

        public override void SetDisplay(ActiveSquare display)
        {
            if (readyToSetDisplay == true)
            {
                ((ActiveObjectiveSquare)display).SetDisplay(optionState, position);
                readyToSetDisplay = false;
            }
        }
    }
}
