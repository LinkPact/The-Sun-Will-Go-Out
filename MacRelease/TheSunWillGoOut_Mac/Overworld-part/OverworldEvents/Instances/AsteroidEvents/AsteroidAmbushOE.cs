using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class AsteroidAmbushOE : OverworldEvent
    {
        private List<String> rebelPirates = new List<String> {
            "RebelPirate1", 
            "RebelPirate2",
            "RebelPirate3", 
            "RebelPirate4", 
            "RebelPirate5"
        };

        private List<String> alliancePirates = new List<String> {
            "AlliancePirate1", 
            "AlliancePirate2",
            "AlliancePirate3", 
            "AlliancePirate4", 
            "AlliancePirate5"
        };

        private String clearText;
        private LevelOE levelEvent;

        public AsteroidAmbushOE() :
            base()
        {
            this.clearText = "The asteroid is empty now...";
        }

        public override Boolean Activate()
        {
            var eventTextList = new List<String>();
            Boolean successfullyActivated = false;

            if (!IsCleared())
            {
                String interactText = "[SAIR] An ambush!";
                int moneyReward = 0;
                List<Item> itemRewards = new List<Item>();
                String levelCompleted = "You successfully fended of the ambush.";
                String levelFailed = "They got the better of you. Next time...";

                String levelString = GetProgressBasedLevel();

                levelEvent = new LevelOE(interactText, levelString, moneyReward, itemRewards, levelCompleted, levelFailed);
                successfullyActivated = levelEvent.Activate();

                if (successfullyActivated)
                {
                    ClearEvent();
                }
            }
            else
            {
                eventTextList.Add(clearText);
                PopupHandler.DisplayMessage(eventTextList.ToArray());
            }

            return successfullyActivated;
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (levelEvent != null)
            {
                levelEvent.Update(game, gameTime);
            }
        }

        public String GetProgressBasedLevel()
        {
            var currentPhase = MissionManager.GetCurrentGamePhase();

            switch (currentPhase)
            { 
                case GamePhase.beginning:
                case GamePhase.withAlliance:
                    return MathFunctions.PickRandomFromList(rebelPirates);
                case GamePhase.withRebels:
                case GamePhase.ending:
                    return MathFunctions.PickRandomFromList(alliancePirates);
                default:
                    throw new ArgumentException(string.Format("Unknown argument: {1}", currentPhase));
            }
        }
    }
}
