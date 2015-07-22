using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceProject_Mac
{
    public class ShopInventoryEntry
    {
        private ShipPartType shipPartType;
        public ShipPartType ShipPartType { get { return shipPartType; } }

        private ItemVariety itemVariety;
        public ItemVariety ItemVariety { get { return itemVariety; } }

        private String id;
        public String ID { get { return id; } }

        private ShipPartAvailability availability;
        public ShipPartAvailability Availability { get { return availability; } }

        public ShopInventoryEntry(ShipPartType shipPartType, ShipPartAvailability shipPartAvailability, ItemVariety itemVariety)
        {
            this.shipPartType = shipPartType;
            this.itemVariety = itemVariety;
            this.availability = shipPartAvailability;
            id = shipPartType.ToString() + " " + shipPartAvailability.ToString() + " " + itemVariety.ToString();
        }

        public ShopInventoryEntry(String id)
        {
            MatchCollection matches = Regex.Matches(id, @"\w+");
            shipPartType = MathFunctions.ParseEnum<ShipPartType>(matches[0].ToString());
            availability = MathFunctions.ParseEnum<ShipPartAvailability>(matches[1].ToString());
            itemVariety = MathFunctions.ParseEnum<ItemVariety>(matches[2].ToString());
            this.id = id;
        }

        public String GetId()
        {
            return id;
        }
    }
}
