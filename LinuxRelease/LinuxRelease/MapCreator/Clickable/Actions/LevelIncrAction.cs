using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public abstract class LevelIncrAction : LevelAction
    {
        protected int increment;

        public LevelIncrAction(int increment)
            : base()
        {
            this.increment = increment;
        }

        public Boolean PerformAction(LevelMechanics level)
        {
            if (!isActionPerformed)
            {
                ActionLogic(level);
                isActionPerformed = true;
                return true;
            }
            return false;
        }

        protected abstract void ActionLogic(LevelMechanics level);
    }
}
