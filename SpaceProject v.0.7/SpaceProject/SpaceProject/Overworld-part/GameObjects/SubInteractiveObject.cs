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
        private string welcomeText;
        private string declineFuelPurchase;
        private string fuelBoughtText;
        private string notEnoughMoneyText;
        private string fuelAlreadyFullText;
        private int fuelPrice;

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
                    break;

                case InteractionType.FuelShop:
                    {
                        messageBox.DisplaySelectionMenu(welcomeText, new List<String>(){"Yes", "No"}
                            , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (StatsManager.Fuel == StatsManager.MaxFuel)
                                {
                                    messageBox.DisplayMessage(fuelAlreadyFullText, 50);
                                }

                                else if (StatsManager.Rupees >= 100)
                                {
                                    messageBox.DisplayMessage(fuelBoughtText, 50);
                                    StatsManager.Rupees -= 100;
                                    StatsManager.Fuel = StatsManager.MaxFuel;
                                }

                                else
                                {
                                    messageBox.DisplayMessage(notEnoughMoneyText, 50);
                                }
                            }, 

                            delegate 
                            {
                                messageBox.DisplayMessage(declineFuelPurchase, 50);
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
            this.welcomeText = welcomeText;
            this.declineFuelPurchase = declineFuelPurchase;
            this.fuelBoughtText = fuelBoughtText;
            this.notEnoughMoneyText = notEnoughMoneyText;
            this.fuelAlreadyFullText = fuelAlreadyFullText;

            fuelPrice = price;
        }
    }
}
