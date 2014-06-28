using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RunLevelAction : SystemAndLevelAction
    {

        public RunLevelAction()
            : base()
        { }

        protected override void ActionLogic(Game1 game, LevelMechanics level)
        {
            game.stateManager.shooterState.SetupLevelTestRun(level.GetName(), level.GetTestStartTime());
            game.stateManager.shooterState.BeginLevel("testRun");
        }
    }
}
