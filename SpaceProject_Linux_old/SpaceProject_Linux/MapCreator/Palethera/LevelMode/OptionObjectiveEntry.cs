using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class OptionObjectiveEntry : OptionEntry
    {
        public OptionObjectiveEntry(Sprite spriteSheet, Vector2 position, LevelObjective objectiveType)
            : base(position)
        {
            squarePos.X += 80;
            this.position = position;
            displayText = objectiveType.ToString();
            optionSquare = new OptionObjectiveSquare(spriteSheet, squarePos, objectiveType);
        }
    }
}
