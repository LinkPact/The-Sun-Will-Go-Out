using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    class PointSquareData : SquareData
    {
        private PointEventType pointEventType_;

        //public PointEventType pointEventType;

        public PointEventType pointEventType {
            get { return pointEventType_; }
            set { pointEventType_ = value; }
        }


        public PointSquareData(String loadText)
            : base(loadText)
        {
            String enemyString = loadText.Substring(0, 1);
            String movementString = loadText.Substring(1,2);
            String eventString = loadText.Substring(3, 1);

            SetEnemy(enemyString);
            SetMovement(movementString);
            SetEvent(eventString);

            SortedDictionary<string, int> settings = new SortedDictionary<string, int>();
            MatchCollection matches = Regex.Matches(loadText, @"([A-Z])(\d+)");
            foreach (Match match in matches)
            {
                String letter = match.Groups[1].Value;
                String valueString = match.Groups[2].Value;
                int value = Convert.ToInt32(valueString);
                settings[letter] = value;
            }

            eventData = new PointEventData(pointEventType);
            eventData.eventSettings = settings;
        }

        //Called when creating blank data
        public PointSquareData()
            : base ()
        { }

        private void SetEvent(String eventString)
        {
            pointEventType = DataConversionLibrary.GetPointEnumFromString(eventString);        
        }

        public override string ToString()
        {
            String returnString = base.ToString();

            returnString += DataConversionLibrary.GetPointStringFromEnum(pointEventType);

            if (enemyType != EnemyType.none)
            {
                returnString += EventEditor.GetSaveString(eventData.eventSettings);
            }

            return returnString;
        }
    }
}
