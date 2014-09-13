using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
{
    public class GridWidthIncrButton : IncrementalButton
    {
        public GridWidthIncrButton(Game1 game, Sprite spriteSheet, Vector2 position, IncrementalType type)
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
            int incrementAmount;
            switch (type)
            {
                case IncrementalType.smallNegative:
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
                default:
                    {
                        throw new ArgumentException("No proper increase amount assigned!");
                    }
            }

            AddAction(new GridWidthIncrAction(incrementAmount));
        }
    }
}
