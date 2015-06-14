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

        public static OverworldEvent GetRandomCommonEvent(Game1 Game)
        {
            var eventType = MathFunctions.PickRandomFromList(randomCommon);

            switch (eventType)
            { 
                case OverworldEventType.DisplayText:
                    return new FindTextOnAsteroidOE();
                case OverworldEventType.GetItem:
                    return new FindItemOnAsteroidOE(Game);
                case OverworldEventType.PirateEncounter:
                    return new AsteroidAmbushOE();
                default:
                    throw new ArgumentException("Code does currently not cover given type!");
            }
        }
    }
}
