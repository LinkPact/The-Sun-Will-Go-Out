using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class SetLevelDurationAction : LevelNonIncrAction
    {
        private float newDuration;

        public SetLevelDurationAction(float newDuration)
            : base()
        {
            this.newDuration = newDuration;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.SetLevelDuration(newDuration);
        }
    }
}
