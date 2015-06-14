using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class FindTextOnAsteroidOE : OverworldEvent
    {
        private List<String> beginningText = new List<String> { "beginning1#...", "beginning2#followup", "beginning3#more#more!"};
        private List<String> allianceText = new List<String> { "alliance1#...", "alliance2#followup", "alliance3#more#more!" };
        private List<String> rebelText = new List<String> { "rebel1#...", "rebel2#followup", "rebel3#more#more!" };
        private List<String> endText = new List<String> { "end1#...", "end2#followup", "end3#more#more!" };

        public FindTextOnAsteroidOE() :
            base()
        { }

        public override Boolean Activate()
        {
            var eventTextList = new List<String>();
            Boolean successfullyActivated = false;

            if (!IsCleared())
            {
                var displayText = GetProgressBasedText();
                
                var itemOE = new DisplayTextOE(displayText);
                successfullyActivated = itemOE.Activate();
            }

            return successfullyActivated;
        }

        private String GetProgressBasedText()
        {
            var currentPhase = MissionManager.GetCurrentGamePhase();

            switch (currentPhase)
            {
                case GamePhase.beginning:
                    {
                        return MathFunctions.PickRandomFromList<String>(beginningText);
                    }
                case GamePhase.withAlliance:
                    {
                        return MathFunctions.PickRandomFromList<String>(rebelText);
                    }
                case GamePhase.withRebels:
                    {
                        return MathFunctions.PickRandomFromList<String>(allianceText);
                    }
                case GamePhase.ending:
                    {
                        return MathFunctions.PickRandomFromList<String>(endText);
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("Unknown argument given: {0}", currentPhase.ToString()));
                    }
            }
        }
    }
}
