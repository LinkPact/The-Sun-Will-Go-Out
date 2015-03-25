using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    class LoadLevelAction : LevelNonIncrAction
    {
        public LoadLevelAction()
            : base()
        { }

        protected override void ActionLogic(LevelMechanics level)
        {
            //string filepath = Microsoft.VisualBasic.Interaction.InputBox("Enter name of the saved level:", "Load level", "map1", -1, -1);
            //level.LoadFile(filepath);
            //level.ChangeName(filepath);
            throw new Exception("Currently unsupported!");
        }
    }
}
