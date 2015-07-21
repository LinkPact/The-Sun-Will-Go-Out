using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class DurationWidthIncrButton : IncrementalButton
    {
        public DurationWidthIncrButton(Game1 game, Sprite spriteSheet, Vector2 position, IncrementalType type)
            : base(game, spriteSheet, position, type)
        {
            switch (type)
            {
                case IncrementalType.smallNegative:
                    {
                        incrementAmount = -1;
                        break;
                    }
                case IncrementalType.smallPositive:
                    {
                        incrementAmount = 1;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Unknown argument!");
                    }
            }
        }

        public override void ClickAction()
        {
            int incrementAmount;
            switch (type)
            {
                case IncrementalType.smallNegative:
                    {
                        incrementAmount = -1;
                        break;
                    }
                case IncrementalType.smallPositive:
                    {
                        incrementAmount = 1;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Unknown argument!");
                    }
            }

            AddAction(new DurationGridWidthIncrAction(incrementAmount));
        }

    }
}
