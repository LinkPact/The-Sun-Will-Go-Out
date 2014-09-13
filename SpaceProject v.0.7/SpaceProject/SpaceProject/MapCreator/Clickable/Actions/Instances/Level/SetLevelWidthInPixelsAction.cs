using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject.MapCreator
{
    class SetLevelWidthInPixelsAction : LevelNonIncrAction
    {
        private int newWidth;

        public SetLevelWidthInPixelsAction(int newWidth)
            : base()
        {            
            this.newWidth = newWidth;
        }

        protected override void ActionLogic(LevelMechanics level)
        {
            level.SetWidthInPixels(newWidth);
        }
    }
}
