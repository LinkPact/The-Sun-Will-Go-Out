using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class ClearGridAction : LevelNonIncrAction
    {
        public ClearGridAction()
            : base()
        { }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.ClearGrid();
        }
    }
}
