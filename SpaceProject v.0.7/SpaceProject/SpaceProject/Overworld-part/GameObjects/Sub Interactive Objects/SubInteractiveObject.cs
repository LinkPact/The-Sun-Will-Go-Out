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
        GetItem,
        Custom,
        Nothing
    }

    public class SubInteractiveObject : GameObjectOverworld
    {
        private static int count;
        private int id;

        private bool cleared;
        private string clearedText = "EMPTY";

        protected InteractionType interactionType;
        protected List<string> text;
        protected List<string> options;
        protected MessageBox messageBox;

        private string purchaseText;
        private string declinePurchaseText;
        private string notEnoughMoneyText;
        private int price;
        
        // fuel stuff
        private string fuelAlreadyFullText;

        // level stuff
        private bool levelCleared;
        private bool startLevelWhenTextCleared;
        private string level;
        private int levelMoneyReward;
        private List<Item> levelItemReward;
        private List<string> levelCompletedText;
        private string levelFailedText;

        // item shop stuff
        private Item itemShopItem;
        private string inventoryFullText;

        // Random Item Get
        private Item getItemItem;

        protected SubInteractiveObject(Game1 game, Sprite spriteSheet, MessageBox messageBox) :
            base(game, spriteSheet)
        {
            SubInteractiveObject.count++;
            id = count;

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
                    && Game.stateManager.shooterState.CurrentLevel.Identifier.ToLower().Equals(level.ToLower())
                    && Game.stateManager.shooterState.CurrentLevel.IsObjectiveFailed
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    Game.messageBox.DisplayMessage(levelFailedText, false);
                    Game.stateManager.shooterState.GetLevel(level).Initialize();
                }

                else if (Game.stateManager.shooterState.CurrentLevel != null
                    && Game.stateManager.shooterState.CurrentLevel.Identifier.ToLower().Equals(level.ToLower())
                    && Game.stateManager.shooterState.CurrentLevel.IsObjectiveCompleted
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    levelCleared = true;
                    Game.messageBox.DisplayMessage(levelCompletedText, false);
                    foreach (Item item in levelItemReward)
                    {
                        ShipInventoryManager.AddItem(item);
                    }
                    StatsManager.Rupees += levelMoneyReward;
                    cleared = true;
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
            if (!MissionManager.IsCurrentObjectiveDestination(this))
            {
                if (cleared)
                {
                    messageBox.DisplayMessage(clearedText, false);

                    return;
                }

                switch (interactionType)
                {
                    case InteractionType.Text:
                        Game.messageBox.DisplayMessage(text, false);
                        break;

                    case InteractionType.LevelWithReward:
                        {
                            if (!levelCleared)
                            {
                                Game.messageBox.DisplayMessage(text, false);
                                startLevelWhenTextCleared = true;
                            }
                            break;
                        }

                    case InteractionType.FuelShop:
                        {
                            messageBox.DisplaySelectionMenu(text[0], new List<String>() { "Yes", "No" }
                                , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (StatsManager.Fuel == StatsManager.MaxFuel)
                                {
                                    messageBox.DisplayMessage(fuelAlreadyFullText, false, 50);
                                }

                                else if (StatsManager.Rupees >= price)
                                {
                                    messageBox.DisplayMessage(purchaseText, false, 50);
                                    StatsManager.Rupees -= price;
                                    StatsManager.Fuel = StatsManager.MaxFuel;
                                }

                                else
                                {
                                    messageBox.DisplayMessage(notEnoughMoneyText, false, 50);
                                }
                            }, 

                            delegate 
                            {
                                messageBox.DisplayMessage(declinePurchaseText, false, 50);
                            }
                        });

                            break;
                        }

                    case InteractionType.ItemShop:
                        {
                            messageBox.DisplaySelectionMenu(text[0], new List<String>() { "Yes", "No" }
                                , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (!ShipInventoryManager.HasAvailableSlot())
                                {
                                    messageBox.DisplayMessage(inventoryFullText, false, 50);
                                }

                                else if (StatsManager.Rupees >= price)
                                {
                                    messageBox.DisplayMessage(purchaseText, false, 50);
                                    StatsManager.Rupees -= price;
                                    ShipInventoryManager.AddItem(itemShopItem);
                                    cleared = true;
                                }

                                else
                                {
                                    messageBox.DisplayMessage(notEnoughMoneyText, false, 50);
                                }
                            }, 

                            delegate 
                            {
                                messageBox.DisplayMessage(declinePurchaseText, false, 50);
                            }
                        });

                            break;
                        }

                    case InteractionType.GetItem:
                        if (ShipInventoryManager.HasAvailableSlot())
                        {
                            ShipInventoryManager.AddItem(getItemItem);
                            cleared = true;
                        }
                        else
                        {
                            text.Add(inventoryFullText);
                        }
                        messageBox.DisplayMessage(text, false);
                        break;

                    case InteractionType.Custom:
                    default:
                        break;
                }
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
            text.Add(welcomeText);
            this.declinePurchaseText = declineFuelPurchase;
            this.purchaseText = fuelBoughtText;
            this.notEnoughMoneyText = notEnoughMoneyText;
            this.fuelAlreadyFullText = fuelAlreadyFullText;
            this.price = price;
        }

        protected void SetupItemShop(Item item, String welcomeText, String declinePurchaseText, String itemBoughtText,
            String notEnoughMoneyText, String inventoryFullText, int price)
        {
            interactionType = InteractionType.ItemShop;
            itemShopItem = item;
            text.Add(welcomeText);
            this.declinePurchaseText = declinePurchaseText;
            purchaseText = itemBoughtText;
            this.notEnoughMoneyText = notEnoughMoneyText;
            this.inventoryFullText = inventoryFullText;
            this.price = price;
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

        protected void SetupGetItem(string text, string inventoryFullText, Item item)
        {
            interactionType = InteractionType.GetItem;

            this.text.Add(text);
            this.inventoryFullText = inventoryFullText;
            getItemItem = item;
        }

        protected void SetClearedText(string text)
        {
            this.clearedText = text;
        }

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            saveData.Clear();
            saveData.Add("disabled", cleared.ToString());
            saveData.Add("disabledText", clearedText);

            Game.saveFile.Save("save.ini", "subobject" + id, saveData);
        }

        public void Load()
        {
            cleared = Game.saveFile.GetPropertyAsBool("subobject" + id, "disabled", true);
            clearedText = Game.saveFile.GetPropertyAsString("subobject" + id, "disabledText", "");
        }
    }
}
