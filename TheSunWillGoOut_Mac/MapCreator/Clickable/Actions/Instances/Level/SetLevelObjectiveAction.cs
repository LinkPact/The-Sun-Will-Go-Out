using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class SetLevelObjectiveAction : LevelNonIncrAction
    {
        private LevelObjective objective;
        private int objectiveInt;

        public SetLevelObjectiveAction(LevelObjective objective, int objectiveInt)
            : base()
        {
            this.objective = objective;
            this.objectiveInt = objectiveInt;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.SetLevelObjective(objective, objectiveInt);
        }
    }
}
