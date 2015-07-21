using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class GridWidthIncrAction : LevelIncrAction
    {
        public GridWidthIncrAction(int incrementAmount)
            : base(incrementAmount)
        {
            this.incrementAmount = incrementAmount;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.ChangePointGridWidth(incrementAmount);
        }

    }
}
