using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class EditNameAction : LevelNonIncrAction
    {

        public EditNameAction()
            : base()
        { }

        protected override void ActionLogic(LevelMechanics level)
        {
            //String currentName = level.GetName();
            //string input = Microsoft.VisualBasic.Interaction.InputBox("Enter new levelname:", "Edit name", 
            //    currentName, -1, -1);
            //level.ChangeName(input);
            throw new NotSupportedException("Visual basic reference removed!");
        }


    }
}
