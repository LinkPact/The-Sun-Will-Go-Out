using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class OptionPointEntry : OptionEntry
    {
        public OptionPointEntry(Sprite spriteSheet, Vector2 position, PointEventType eventType)
            : base(position)
        {
            this.position = position;
            displayText = eventType.ToString();
            optionSquare = new OptionPointEventSquare(spriteSheet, squarePos, eventType);
        }
    }
}
