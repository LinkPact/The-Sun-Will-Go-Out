//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SpaceProject
//{
//    public class ShopInventoryEntry
//    {
//        private ShipPartAvailability shipPartAvailability;
//        public ShipPartAvailability ShipPartAvailability { get { return shipPartAvailability; } }

//        private ShopShipPartEntry partEntry;
//        public ShopShipPartEntry PartEntry { get { return partEntry; } }

//        public ShopInventoryEntry(ShipPartType shipPartType, ShipPartAvailability shipPartAvailability, ItemVariety itemVariety)
//        {
//            partEntry = new ShopShipPartEntry(shipPartType, itemVariety);
//            this.shipPartAvailability = shipPartAvailability;
//        }

//        public String GetId()
//        {
//            return partEntry.ID;
//        }
//    }
//}
