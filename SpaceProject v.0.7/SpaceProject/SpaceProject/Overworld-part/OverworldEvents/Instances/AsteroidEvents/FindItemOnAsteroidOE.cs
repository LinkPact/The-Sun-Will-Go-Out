using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class FindItemOnAsteroidOE : OverworldEvent
    {
        private Game1 Game;

        private String clearText;

        public FindItemOnAsteroidOE(Game1 Game) :
            base()
        {
            this.Game = Game;
            this.clearText = "An empty asteroid floating in space.";
        }

        public override Boolean Activate()
        {
            var eventTextList = new List<String>();
            Boolean successfullyActivated = false;

            if (!IsCleared())
            {
                var item = GetProgressBasedRandomItem(Game);
                var itemOE = new GetItemOE(item, string.Format("You found the {0}!", item.Name), "Your inventory is full!", "Cleared (is this shown?)");
                successfullyActivated = itemOE.Activate();

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

        private Item GetProgressBasedRandomItem(Game1 Game)
        {
            var currentPhase = MissionManager.GetCurrentGamePhase();

            switch (currentPhase)
            {
                case GamePhase.beginning:
                    {
                        return new BasicLaserWeapon(Game);
                    }
                case GamePhase.withAlliance:
                    {
                        return new MultipleShotWeapon(Game);
                    }
                case GamePhase.withRebels:
                    {
                        return new BursterWeapon(Game);
                    }
                case GamePhase.ending:
                    {
                        return new DualLaserWeapon(Game);
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("Unknown argument given: {0}", currentPhase.ToString()));
                    }
            }
        }
    }
}
