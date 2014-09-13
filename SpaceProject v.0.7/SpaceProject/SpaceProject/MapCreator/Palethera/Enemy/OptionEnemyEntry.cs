using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class OptionEnemyEntry : OptionEntry
    {
        public OptionEnemyEntry(Sprite spriteSheet, Vector2 position, EnemyType enemyType)
            : base(position)
        {
            squarePos.X += 30;

            this.position = position;
            displayText = enemyType.ToString();
            optionSquare = new OptionEnemySquare(spriteSheet, squarePos, enemyType);
        }

        public OptionEnemyEntry(Sprite spriteSheet, Vector2 position, String displayText)
            : base(position)
        {
            this.position = position;
            this.displayText = displayText;
            SetHasNoSquare();
        }
    }
}
