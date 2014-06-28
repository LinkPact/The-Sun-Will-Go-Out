using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceProject
{
    public abstract class Action
    {
        protected Boolean isActionPerformed;

        public Action()
        {
            isActionPerformed = false;
        }

        public Boolean IsActionPerformed()
        {
            return isActionPerformed;
        }

        protected int GetNewInt(String displayText, String headerText, String inputText)
        {
            String newTimeString = Microsoft.VisualBasic.Interaction.InputBox(displayText, headerText, inputText, -1, -1);

            Match matchInteger = Regex.Match(newTimeString, @"^\d+$");

            int inputInt;
            if (matchInteger.Success)
            {
                inputInt = Convert.ToInt32(newTimeString);
                if (inputInt >= 0)
                    return inputInt;
            }
            return -1;
        }
    }
}
