using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    abstract class OptionSquare : Clickable
    {
        protected Boolean readyToSetDisplay;

        protected OptionSquare(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            readyToSetDisplay = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLeftClicked())
            {
                readyToSetDisplay = true;
            }
        }

        public Boolean IsReadyToSetDisplay()
        {
            return readyToSetDisplay;
        }

        public abstract void SetDisplay(ActiveSquare display);
    }
}
