using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject 
{
    class SetStartTimeAction : LevelNonIncrAction
    {
        private float newStartTime;

        public SetStartTimeAction(float newStartTime)
            : base()
        {            
            this.newStartTime = newStartTime;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.SetTestStartTime(newStartTime);
        }

    }
}
