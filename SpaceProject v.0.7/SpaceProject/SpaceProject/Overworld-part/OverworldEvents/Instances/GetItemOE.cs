using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class GetItemOE : OverworldEvent
    {
        private Item item;
        private String eventText;
        private String inventoryFullText;
        private String clearText;

        public GetItemOE(Item item, String eventText, String inventoryFullText, String clearText) :
            base()
        {
            this.item = item;
            this.eventText = eventText;
            this.inventoryFullText = inventoryFullText;
            this.clearText = clearText;
        }

        public override void Activate()
        {
            var eventTextList = new List<String>();

            if (!IsCleared())
            {
                eventTextList.Add(eventText);

                if (ShipInventoryManager.HasAvailableSlot())
                {
                    ShipInventoryManager.AddItem(item);
                    ClearEvent();
                }
                else
                {
                    eventTextList.Add(inventoryFullText);
                }
            }
            else
            {
                eventTextList.Add(clearText);
            }

            PopupHandler.DisplayMessage(eventTextList.ToArray());
        }
    }
}
