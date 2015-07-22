using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public abstract class SystemAction : Action
    {
        public SystemAction() : base()
        { }

        public Boolean PerformAction(Game1 game)
        {
            if (!isActionPerformed)
            {
                ActionLogic(game);
                isActionPerformed = true;
                return true;
            }
            return false;
        }

        protected abstract void ActionLogic(Game1 game);
    }  
}
