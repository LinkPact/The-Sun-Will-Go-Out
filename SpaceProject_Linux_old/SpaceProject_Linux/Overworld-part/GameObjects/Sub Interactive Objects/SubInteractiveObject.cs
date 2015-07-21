using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
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

    public abstract class SubInteractiveObject : GameObjectOverworld
    {
        private static int count;
        private int id;

        private bool oneTimeOnly;
        private bool cleared;
        protected string clearedText = "EMPTY";

        protected InteractionType interactionType;
        protected List<string> text;
        protected List<string> options;

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
        protected bool itemBought;
        private Item itemShopItem;
        private string inventoryFullText;

        // Random Item Get
        private Item getItemItem;

        protected SubInteractiveObject(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            SubInteractiveObject.count++;
            id = count;
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
                && PopupHandler.TextBufferEmpty)
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
                    PopupHandler.DisplayMessage(levelFailedText);
                    Game.stateManager.shooterState.GetLevel(level).Initialize();
                }

                else if (Game.stateManager.shooterState.CurrentLevel != null
                    && Game.stateManager.shooterState.CurrentLevel.Identifier.ToLower().Equals(level.ToLower())
                    && Game.stateManager.shooterState.CurrentLevel.IsObjectiveCompleted
                    && GameStateManager.currentState.ToLower().Equals("overworldstate"))
                {
                    levelCleared = true;
                    PopupHandler.DisplayMessage(levelCompletedText.ToArray());
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
                    PopupHandler.DisplayMessage(clearedText);

                    return;
                }

                switch (interactionType)
                {
                    case InteractionType.Text:
                        PopupHandler.DisplayMessage(text.ToArray());
                        break;

                    case InteractionType.LevelWithReward:
                        {
                            if (!levelCleared)
                            {
                                PopupHandler.DisplayMessage(text.ToArray());
                                startLevelWhenTextCleared = true;
                            }
                            break;
                        }

                    case InteractionType.FuelShop:
                        {
                            PopupHandler.DisplaySelectionMenu(text[0], new List<String>() { "Yes", "No" }
                                , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (StatsManager.Fuel == StatsManager.MaxFuel)
                                {
                                    PopupHandler.DisplayMessage(fuelAlreadyFullText);
                                }

                                else if (StatsManager.Rupees >= price)
                                {
                                    PopupHandler.DisplayMessage(purchaseText);
                                    StatsManager.Rupees -= price;
                                    StatsManager.Fuel = StatsManager.MaxFuel;
                                }

                                else
                                {
                                    PopupHandler.DisplayMessage(notEnoughMoneyText);
                                }
                            }, 

                            delegate 
                            {
                                PopupHandler.DisplayMessage(declinePurchaseText);
                            }
                        });

                            break;
                        }

                    case InteractionType.ItemShop:
                        {
                            PopupHandler.DisplaySelectionMenu(text[0], new List<String>() { "Yes", "No" }
                                , new List<System.Action>()
                        {
                            delegate 
                            {
                                if (!ShipInventoryManager.HasAvailableSlot())
                                {
                                    PopupHandler.DisplayMessage(inventoryFullText);
                                }

                                else if (StatsManager.Rupees >= price)
                                {
                                    itemBought = true;
                                    PopupHandler.DisplayMessage(purchaseText);

                                    StatsManager.Rupees -= price;
                                    ShipInventoryManager.AddItem(itemShopItem);
                                    if (oneTimeOnly)
                                    {
                                        cleared = true;
                                    }
                                }

                                else
                                {
                                    PopupHandler.DisplayMessage(notEnoughMoneyText);
                                }
                            }, 

                            delegate 
                            {
                                PopupHandler.DisplayMessage(declinePurchaseText);
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
                        PopupHandler.DisplayMessage(text.ToArray());
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
            String notEnoughMoneyText, String inventoryFullText, int price, bool oneTimeOnly = true)
        {
            interactionType = InteractionType.ItemShop;
            itemShopItem = item;
            text.Clear();
            text.Add(welcomeText);
            this.declinePurchaseText = declinePurchaseText;
            purchaseText = itemBoughtText;
            this.notEnoughMoneyText = notEnoughMoneyText;
            this.inventoryFullText = inventoryFullText;
            this.price = price;
            this.oneTimeOnly = oneTimeOnly;
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

        protected abstract void SetClearedText();

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            saveData.Clear();
            saveData.Add("disabled", cleared.ToString());

            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "subobject" + id, saveData);
        }

        public void Load()
        {
            cleared = Game.saveFile.GetPropertyAsBool("subobject" + id, "disabled", false);
            if (cleared)
            {
                SetClearedText();
            }
        }
    }
}
