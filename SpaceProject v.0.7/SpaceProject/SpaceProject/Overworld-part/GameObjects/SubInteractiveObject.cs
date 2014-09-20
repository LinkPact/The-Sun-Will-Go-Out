using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum InteractionType
    { 
        Text,
        LevelWithReward,
        FuelShop,
        ItemShop,
        Scenario,
        TextWithItem,
        Mission,
        Custom
    }

    public class SubInteractiveObject : GameObjectOverworld
    {
        protected InteractionType interactionType;
        protected List<string> text;
        protected List<string> options;
        protected MessageBox messageBox;
        
        // fuel stuff
        private string fuelWelcomeText;
        private string fuelDeclinePurchaseText;
        private string fuelBoughtText;
        private string fuelNotEnoughMoneyText;
        private string fuelAlreadyFullText;
        private int fuelPrice;

        // level stuff
        private bool levelCleared;
        private bool startLevelWhenTextCleared;
        private string level;
        private int levelMoneyReward;
        private List<Item> levelItemReward;
        private List<string> levelCompletedText;
        private string levelFailedText;

        protected SubInteractiveObject(Game1 game, Sprite spriteSheet, MessageBox messageBox) :
            base(game, spriteSheet)
        {
            this.messageBox = messageBox;
        }

        public override void Initialize()
        {
            base.Initialize();

            text = new List<string>();
            options = new List<string>();

            scale = 1f;
            layerDepth = 0.3f;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            if (startLevelWhenTextCleared
                && Game.messageBox.MessageState == MessageState.Invisible)
            {
                Game.stateManager.shooterState.BeginLevel(level);
                startLevelWhenTextCleared = false;
            }

            if (!levelCleared
                && level != null
                && level != "")
            {
                if (Game.stateManager.shooterState.CurrentLevel != null
                    && Game.stateManager.shooterState.CurrentLevel.Name.ToLower().Equals(level.ToLower())
                    && Game.stateManager.shooterState.CurrentLevel.IsGameOver
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    Game.messageBox.DisplayMessage(levelFailedText);
                    Game.stateManager.shooterState.GetLevel(level).Initialize();
                }

                else if (Game.stateManager.shooterState.CurrentLevel != null
                    && Game.stateManager.shooterState.CurrentLevel.Name.ToLower().Equals(level.ToLower())
                    && Game.stateManager.shooterState.CurrentLevel.IsObjectiveCompleted
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    levelCleared = true;
                    Game.messageBox.DisplayMessage(levelCompletedText);
                    foreach (Item item in levelItemReward)
                    {
                        ShipInventoryManager.AddItem(item);
                    }
                    StatsManager.Rupees += levelMoneyReward;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void Interact()
        {
            switch (interactionType)
            {
                case InteractionType.Text:
                    Game.messageBox.DisplayMessage(text);
                    break;

                case InteractionType.LevelWithReward:
                    {
                        if (!levelCleared)
                        {
                            Game.messageBox.DisplayMessage(text);
                            startLevelWhenTextCleared = true;
                        }
                        break;
                    }

                case InteractionType.FuelShop:
                    {
                        messageBox.DisplaySelectionMenu(fuelWelcomeText, new List<String>(){"Yes", "No"}
                            , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (StatsManager.Fuel == StatsManager.MaxFuel)
                                {
                                    messageBox.DisplayMessage(fuelAlreadyFullText, 50);
                                }

                                else if (StatsManager.Rupees >= fuelPrice)
                                {
                                    messageBox.DisplayMessage(fuelBoughtText, 50);
                                    StatsManager.Rupees -= fuelPrice;
                                    StatsManager.Fuel = StatsManager.MaxFuel;
                                }

                                else
                                {
                                    messageBox.DisplayMessage(fuelNotEnoughMoneyText, 50);
                                }
                            }, 

                            delegate 
                            {
                                messageBox.DisplayMessage(fuelDeclinePurchaseText, 50);
                            }
                        });

                        break;
                    }

                case InteractionType.ItemShop:
                    break;

                case InteractionType.Scenario:
                    break;

                case InteractionType.TextWithItem:
                    break;

                case InteractionType.Mission:
                    break;

                case InteractionType.Custom:
                default:
                    break;
            }
        }

        protected void SetupText(String text)
        {
            interactionType = InteractionType.Text;
            this.text.Add(text);
        }

        protected void SetupText(List<String> text)
        {
            interactionType = InteractionType.Text;
            this.text = text;
        }

        protected void SetupFuelShop(String welcomeText, String declineFuelPurchase, String fuelBoughtText,
            String notEnoughMoneyText, String fuelAlreadyFullText, int price)
        {
            interactionType = InteractionType.FuelShop;
            this.fuelWelcomeText = welcomeText;
            this.fuelDeclinePurchaseText = declineFuelPurchase;
            this.fuelBoughtText = fuelBoughtText;
            this.fuelNotEnoughMoneyText = notEnoughMoneyText;
            this.fuelAlreadyFullText = fuelAlreadyFullText;

            fuelPrice = price;
        }

        protected void SetupLevel(String interactText, String level, int moneyReward, List<Item> itemReward,
            String levelCompletedText, String levelFailedText)
        {
            interactionType = InteractionType.LevelWithReward;

            text.Add(interactText);
            this.level = level;
            this.levelCompletedText = new List<string>();
            this.levelCompletedText.Add(levelCompletedText);
            this.levelFailedText = levelFailedText;
            this.levelMoneyReward = moneyReward;

            if (itemReward.Count > 0)
            {
                levelItemReward = itemReward;
                string rewardText = "Reward:\n\n";
                foreach (Item item in levelItemReward)
                {
                    rewardText += item.Name + "\n";  
                }

                if (moneyReward > 0)
                {
                    rewardText += moneyReward + " Rupees";
                }

                this.levelCompletedText.Add(rewardText);
            }
        }
    }
}
