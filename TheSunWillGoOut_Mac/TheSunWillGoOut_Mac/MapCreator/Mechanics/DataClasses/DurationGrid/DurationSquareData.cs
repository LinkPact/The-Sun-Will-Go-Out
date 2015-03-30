using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceProject_Mac
{
    class DurationSquareData : SquareData
    {
        public DurationEventType durationEventType;

        public DurationSquareData(String loadText)
            : base()
        {
            String enemyString = loadText.Substring(0, 1);
            String movementString = loadText.Substring(1, 2);
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

            eventData = new DurationEventData(durationEventType);
            eventData.eventSettings = settings;
        }
        
        public DurationSquareData()
            : base()
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            durationEventType = DurationEventType.even;
        }

        private void SetEvent(String eventString)
        {
            durationEventType = DataConversionLibrary.GetDurationEnumFromString(eventString);
        }

        public override string ToString()
        {
            String returnString = base.ToString();

            returnString += DataConversionLibrary.GetDurationStringFromEnum(durationEventType);

            if (enemyType != EnemyType.none)
                returnString += EventEditor.GetSaveString(eventData.eventSettings);

            return returnString;
        }
    }
}
