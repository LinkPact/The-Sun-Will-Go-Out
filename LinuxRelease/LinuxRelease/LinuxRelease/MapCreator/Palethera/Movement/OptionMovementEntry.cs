using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class OptionMovementEntry : OptionEntry
    {
        public OptionMovementEntry(Sprite spriteSheet, Vector2 position, Movement movementType)
            : base(position)
        {
            this.position = position;
            displayText = movementType.ToString();
            optionSquare = new OptionMovementSquare(spriteSheet, squarePos, movementType);
        }
    }
}
