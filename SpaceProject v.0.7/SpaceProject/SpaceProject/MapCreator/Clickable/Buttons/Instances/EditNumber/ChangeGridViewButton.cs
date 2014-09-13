using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject.MapCreator
{
    class ChangeGridViewButton : IncrementalButton
    {
        public ChangeGridViewButton(Game1 game, Sprite spriteSheet, Vector2 position, IncrementalType incrType)
            : base(game, spriteSheet, position, incrType)
        {
            switch (type)
            {
                case IncrementalType.upArrow:
                    {
                        incrementAmount = 1;
                        break;
                    }
                case IncrementalType.downArrow:
                    {
                        incrementAmount = -1;
                        break;
                    }
                default:
                    throw new ArgumentException("Wrong incremental type!");
            }
        }

        public override void ClickAction()
        {
            switch (type)
            {
                case IncrementalType.upArrow:
                    {
                        AddAction(new ChangeGridViewAction(1));
                        break;
                    }
                case IncrementalType.downArrow:
                    {
                        AddAction(new ChangeGridViewAction(-1));
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Couldn't handle IncrementalType");
                    }
            }
        }
    }
}
