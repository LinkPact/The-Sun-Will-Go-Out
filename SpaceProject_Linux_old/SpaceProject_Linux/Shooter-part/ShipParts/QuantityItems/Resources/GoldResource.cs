using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public class GoldResource : ResourceItem
    {
        public GoldResource(Game1 Game, float quantity) :
            base(Game)
        {
            Setup();
            this.quantity = quantity;
        }

        public GoldResource(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        private void Setup()
        {
            Name = "Gold";
            Text = "A bar of gold. Can be sold for a high price.";

            Value = 2;
        }
    }
}
