using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class PirateEncounterOE : LevelOE
    {
        private readonly static List<String> rebelLevels = new List<String> { 
            "RebelPirate1", 
            "RebelPirate2",
            "RebelPirate3", 
            "RebelPirate4", 
            "RebelPirate5"};

        private static String interactText = "interact!";
        private static readonly int moneyReward = 0;
        private static readonly List<Item> itemRewards = new List<Item>();
        private static readonly String levelCompleted = "completed";
        private static readonly String levelFailed = "failed";
        
        public PirateEncounterOE() :
            base(interactText, rebelLevels[MathFunctions.GetExternalRandomInt(0, rebelLevels.Count - 1)], moneyReward, itemRewards, levelCompleted, levelFailed)
        { }
    }
}
