using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    public class TitaniumResource : ResourceItem
    {
        public TitaniumResource(Game1 Game, float quantity) :
            base(Game)
        {
            Setup();
            this.quantity = quantity;
        }

        public TitaniumResource(Game1 Game) :
            base(Game)
        {
            Setup();
            this.quantity = quantity;
        }

        private void Setup()
        {
            Name = "Titanium";
            Text = "A bar of titanium. Can be sold for a very high price.";

            Value = 3;
        }
    }
}
