using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    //Action class which calls methods to save the level to a text file
    class SaveLevelAction : LevelNonIncrAction
    {
        public SaveLevelAction()
            : base()
        { }

        protected override void ActionLogic(LevelMechanics level)
        {
            List<String> stringList = level.GetLevelAsStrings();
            String levelName = level.GetName();
            
            WriteSave.Save(levelName, stringList);
            level.LevelSavedEvent();
        }
    }
}