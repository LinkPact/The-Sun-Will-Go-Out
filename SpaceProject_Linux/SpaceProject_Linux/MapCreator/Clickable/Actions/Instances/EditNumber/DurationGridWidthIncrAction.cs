using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class DurationGridWidthIncrAction : LevelIncrAction
    {
        public DurationGridWidthIncrAction(int incrementAmount)
            : base(incrementAmount)
        {
            this.incrementAmount = incrementAmount;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.ChangeDurationGridWidth(incrementAmount);
        }
    }
}
