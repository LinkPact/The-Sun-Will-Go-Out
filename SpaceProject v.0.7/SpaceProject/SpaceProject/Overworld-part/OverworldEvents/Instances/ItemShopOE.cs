using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ItemShopOE : OverworldEvent
    {
        private Item item;
        private String welcomeText;
        private String declinePurchaseText;
        private String itemBoughtText;
        private String notEnoughMoneyText;
        private String inventoryFullText;
        private int price;
        private bool oneTimeOnly;
        private bool itemBought;

        public bool IsItemBought { get { return itemBought; } }

        public ItemShopOE(Item item, String welcome, String decline, String itemBought, String notEnoughMoney, String fullInventory, 
            int price, bool oneTimeOnly = true)
        {
            this.item = item;
            this.welcomeText = welcome;
            this.declinePurchaseText = decline;
            this.itemBoughtText = itemBought;
            this.notEnoughMoneyText = notEnoughMoney;
            this.inventoryFullText = fullInventory;
            this.price = price;
            this.oneTimeOnly = oneTimeOnly;
        }

        public override Boolean Activate()
        {
            itemBought = false;

            var responseChoices = new List<String>() { "Yes", "No" };
            PopupHandler.DisplaySelectionMenu(welcomeText, responseChoices, new List<System.Action>()
                        {
                            delegate 
                            {
                                if (!ShipInventoryManager.HasAvailableSlot())
                                {
                                    PopupHandler.DisplayMessage(inventoryFullText);
                                }

                                else if (StatsManager.Rupees >= price)
                                {
                                    PopupHandler.DisplayMessage(itemBoughtText);

                                    StatsManager.Rupees -= price;
                                    ShipInventoryManager.AddItem(item);
                                    if (oneTimeOnly)
                                    {
                                        IsCleared();
                                    }
                                    itemBought = true;
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

            return true;
        }

        public void SetNewItem(Item item)
        {
            if (welcomeText.Contains(this.item.Name))
            {
                welcomeText = welcomeText.Replace(this.item.Name, item.Name);
            }

            this.item = item;
        }
    }
}
