using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
{
    public class GridHeightIncrButton : IncrementalButton
    {
        public GridHeightIncrButton(Game1 game, Sprite spriteSheet, Vector2 position, IncrementalType type)
            : base(game, spriteSheet, position, type)
        {
            switch (type)
            { 
                case IncrementalType.smallNegative :
                    {
                        incrementAmount = -1;
                        break;
                    }
                case IncrementalType.largeNegative:
                    {
                        incrementAmount = -5;
                        break;
                    }
                case IncrementalType.smallPositive:
                    {
                        incrementAmount = 1;
                        break;
                    }
                case IncrementalType.largePositive:
                    {
                        incrementAmount = 5;
                        break;
                    }
                case IncrementalType.undefined:
                    {
                        incrementAmount = 0;
                        break;
                    }
                default:
                    {
                        incrementAmount = 0;
                        break;
                    }
            }
        }

        public override void ClickAction()
        {
            switch (type)
            {
                case IncrementalType.smallNegative:
                    {
                        AddAction(new GridHeightIncrAction(-1));
                        break;
                    }
                case IncrementalType.largeNegative:
                    {
                        AddAction(new GridHeightIncrAction(-5));
                        break;
                    }
                case IncrementalType.smallPositive:
                    {
                        AddAction(new GridHeightIncrAction(1));
                        break;
                    }
                case IncrementalType.largePositive:
                    {
                        AddAction(new GridHeightIncrAction(5));
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Increase amount not properly set");
                    }
            }
        }
    }
}
