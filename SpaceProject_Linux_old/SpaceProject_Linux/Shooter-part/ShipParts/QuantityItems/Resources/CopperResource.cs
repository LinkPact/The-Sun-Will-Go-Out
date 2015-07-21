using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class CopperResource : ResourceItem
    {
        public CopperResource(Game1 Game, float quantity) :
            base(Game)
        {
            Setup();
            this.quantity = quantity;
        }

        public CopperResource(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        private void Setup()
        {
            Name = "Copper";
            Text = "A bar of copper. Can be sold for a decent price.";

            Value = 1;
        }
    }
}
