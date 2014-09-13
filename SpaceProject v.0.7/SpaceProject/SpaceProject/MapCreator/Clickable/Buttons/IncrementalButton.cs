using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
{
    public enum IncrementalType
    {
        smallPositive,
        largePositive,
        smallNegative,
        largeNegative,
        upArrow,
        downArrow,
        undefined
    }

    public abstract class IncrementalButton : MapCreatorButton
    {
        protected int incrementAmount;
        protected IncrementalType type;

        protected IncrementalButton(Game1 game, Sprite spriteSheet, Vector2 position, IncrementalType type)
            : base(spriteSheet, position)
        {
            this.type = type;

            switch (type)
            {
                case IncrementalType.smallNegative:
                    {
                        displaySprite = spriteSheet.GetSubSprite(new Rectangle(60, 39, 19, 19));
                        break;
                    }
                case IncrementalType.largeNegative:
                    {
                        displaySprite = spriteSheet.GetSubSprite(new Rectangle(79, 39, 19, 19));
                        break;
                    }
                case IncrementalType.smallPositive:
                    {
                        displaySprite = spriteSheet.GetSubSprite(new Rectangle(79, 20, 19, 19));
                        break;
                    }
                case IncrementalType.largePositive:
                    {
                        displaySprite = spriteSheet.GetSubSprite(new Rectangle(60, 20, 19, 19));
                        break;
                    }
                case IncrementalType.upArrow:
                    {
                        //TODO: enable/disable-logic
                        enabled = spriteSheet.GetSubSprite(new Rectangle(61, 59, 18, 20));
                        disabled = spriteSheet.GetSubSprite(new Rectangle(80, 59, 18, 20));
                        displaySprite = enabled;
                        break;
                    }
                case IncrementalType.downArrow:
                    {
                        //TODO: enable/disable-logic
                        enabled = spriteSheet.GetSubSprite(new Rectangle(61, 80, 18, 20));
                        disabled = spriteSheet.GetSubSprite(new Rectangle(80, 80, 18, 20));
                        displaySprite = enabled;
                        break;
                    }
            }
        }
    }
}
