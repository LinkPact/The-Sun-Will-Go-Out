using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class LevelOE : OverworldEvent
    {
        private String interactText;
        private String level;
        private int moneyReward;
        private List<Item> itemRewards = new List<Item>();
        private List<String> levelCompletedTextStrings = new List<String>();
        private String levelFailedText;

        private Boolean levelCleared;

        public Boolean startLevelWhenTextCleared;

        public LevelOE(String interactText, String level, int moneyReward, List<Item> itemRewards, String levelCompletedText, String levelFailedText) :
            base()
        {
            this.interactText = interactText;
            this.level = level;
            this.moneyReward = moneyReward;
            this.itemRewards.AddRange(itemRewards);
            this.levelCompletedTextStrings.Add(levelCompletedText);
            this.levelFailedText = levelFailedText;

            levelCleared = false;

            if (itemRewards.Count > 0 || moneyReward > 0)
            {
                levelCompletedTextStrings.Add(GetRewardText(itemRewards, moneyReward));
            }
        }

        private String GetRewardText(List<Item> itemRewards, int moneyReward)
        {
            String rewardText = "Reward:\n\n";

            if (itemRewards.Count < 0)
            {
                foreach (var item in itemRewards)
                {
                    rewardText += item.Name + "\n";
                }
            }

            if (moneyReward > 0)
            {
                rewardText += moneyReward + " Crebits";
            }

            return rewardText;
        }

        public override Boolean Activate()
        {
            Boolean successfullyActivated = false;
            if (!IsCleared())
            {
                PopupHandler.DisplayMessage(interactText);
                startLevelWhenTextCleared = true;
                successfullyActivated = true;
            }
            return successfullyActivated;
        }

        public override void Update(Game1 game, GameTime gameTime)
        {
            if (startLevelWhenTextCleared && PopupHandler.TextBufferEmpty)
            {
                game.stateManager.shooterState.BeginLevel(level);
                startLevelWhenTextCleared = false;
            }

            if (!levelCleared && level != null && level != "")
            {
                if (game.stateManager.shooterState.CurrentLevel != null
                    && game.stateManager.shooterState.CurrentLevel.Identifier.ToLower().Equals(level.ToLower())
                    && game.stateManager.shooterState.CurrentLevel.IsObjectiveFailed
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    PopupHandler.DisplayMessage(levelFailedText);
                    game.stateManager.shooterState.GetLevel(level).Initialize();
                }

                else if (game.stateManager.shooterState.CurrentLevel != null
                    && game.stateManager.shooterState.CurrentLevel.Identifier.ToLower().Equals(level.ToLower())
                    && game.stateManager.shooterState.CurrentLevel.IsObjectiveCompleted
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    levelCleared = true;
                    PopupHandler.DisplayMessage(levelCompletedTextStrings.ToArray());
                    foreach (Item item in itemRewards)
                    {
                        ShipInventoryManager.AddItem(item);
                    }
                    StatsManager.Crebits += moneyReward;
                    ClearEvent();
                }
            }
        }
    }
}
