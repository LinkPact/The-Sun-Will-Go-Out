using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    enum OverworldEventType
    { 
        DisplayText,
        GetItem,
        ItemShop,
        PirateEncounter
    }

    class EventGenerator
    {
        private static List<OverworldEventType> randomCommon = new List<OverworldEventType> { 
            OverworldEventType.DisplayText,
            OverworldEventType.GetItem,
            OverworldEventType.PirateEncounter 
        };

        private static List<String> rebelLevels = new List<String> {
            "RebelPirate1", 
            "RebelPirate2",
            "RebelPirate3", 
            "RebelPirate4", 
            "RebelPirate5"
        };

        public static OverworldEvent GetRandomCommonEvent(Game1 Game)
        {
            var eventType = MathFunctions.PickRandomFromList(randomCommon);

            switch (eventType)
            { 
                case OverworldEventType.DisplayText:
                    return new DisplayTextOE("Interesting Text About This Universe!#Follow Up Text!#Done!");
                case OverworldEventType.GetItem:
                    Item it = new BasicLaserWeapon(Game);
                    return new GetItemOE(it, "You Get Weapon-Event!", "Inv full!", "Cleared!");
                case OverworldEventType.PirateEncounter:
                    return GetRandomPirateLevelEvent();
                default:
                    throw new ArgumentException("Code does currently not cover given type!");
            }
        }

        public static OverworldEvent GetRandomPirateLevelEvent()
        { 
            String interactText = "OMG PIRATES!";
            int moneyReward = 0;
            List<Item> itemRewards = new List<Item>();
            String levelCompleted = "Defeated!";
            String levelFailed = "Loser!";

            String rebelLevel = MathFunctions.PickRandomFromList(rebelLevels);

            return new LevelOE(interactText, rebelLevel, moneyReward, itemRewards, levelCompleted, levelFailed);
        }
    }
}
