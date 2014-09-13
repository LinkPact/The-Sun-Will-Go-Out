using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject.MapCreator
{
    abstract class SystemAndLevelAction : Action
    {
        public SystemAndLevelAction() : base()
        { }

        public Boolean PerformAction(Game1 game, LevelMechanics level)
        {
            if (!isActionPerformed)
            {
                ActionLogic(game, level);
                isActionPerformed = true;
                return true;
            }
            return false;
        }

        protected abstract void ActionLogic(Game1 game, LevelMechanics level);
    }
}
