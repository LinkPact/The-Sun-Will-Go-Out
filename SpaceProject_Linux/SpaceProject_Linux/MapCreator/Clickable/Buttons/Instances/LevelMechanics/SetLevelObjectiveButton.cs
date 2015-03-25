using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class SetLevelObjectiveButton : MapCreatorButton
    {
        public SetLevelObjectiveButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(99, 20, 14, 14));
        }

        public override void ClickAction()
        {
            LevelObjective newObjective = ActiveData.levelObjective;
            int levelObjectiveInt = EventEditor.GetObjectiveInt(newObjective);

            if (levelObjectiveInt >= 0)
            {
                AddAction(new SetLevelObjectiveAction(newObjective, levelObjectiveInt));
            }
        }
    }
}
