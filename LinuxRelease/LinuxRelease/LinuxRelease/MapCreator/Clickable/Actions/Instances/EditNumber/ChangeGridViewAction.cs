using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ChangeGridViewAction : LevelIncrAction
    {
        public ChangeGridViewAction(int incrementAmount)
            : base(incrementAmount)
        {
            this.incrementAmount = incrementAmount;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.ChangeGridViewFrame(incrementAmount);
        }
    }
}
