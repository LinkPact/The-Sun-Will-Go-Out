using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public abstract class LevelNonIncrAction : LevelAction
    {
        public LevelNonIncrAction()
            : base()
        { }

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
