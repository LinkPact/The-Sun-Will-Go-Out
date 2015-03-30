using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public class FineWhiskey : QuantityItem
    {
        public FineWhiskey(Game1 Game, float quantity) :
            base(Game)
        {
            Setup();
            this.quantity = quantity;
        }

        public FineWhiskey(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        private void Setup()
        {
            text = "Very fine whiskey from the old Earth, brewed before the migration.";

            Kind = "Spirit";
            Name = "Fine Whiskey";
            maxQuantity = 100;

            Value = 20;
        }

        protected override String GetDescription()
        {
            return "Very fine whiskey from the old Earth, brewed before the migration.";
        }
    }
}
