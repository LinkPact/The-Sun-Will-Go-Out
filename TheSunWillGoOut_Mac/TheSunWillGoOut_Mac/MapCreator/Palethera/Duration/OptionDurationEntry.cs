using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class OptionDurationEntry : OptionEntry
    {
        public OptionDurationEntry(Sprite spriteSheet, Vector2 position, DurationEventType eventType)
            : base(position)
        {
            this.position = position;
            displayText = eventType.ToString();
            optionSquare = new OptionDurationEventSquare(spriteSheet, squarePos, eventType);
        }
    }
}
