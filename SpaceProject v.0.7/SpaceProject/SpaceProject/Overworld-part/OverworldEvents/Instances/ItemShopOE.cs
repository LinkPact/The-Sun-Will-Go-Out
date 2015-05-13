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

        public override void Activate()
        {
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
        }

    }
}
