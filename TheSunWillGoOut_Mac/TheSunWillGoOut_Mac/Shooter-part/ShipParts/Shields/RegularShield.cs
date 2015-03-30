using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class RegularShield : PlayerShield
    {
        public RegularShield(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A mid range shield, offering decent protection";
        }

        private void Setup()
        {
            Name = "Regular Shield";
            Weight = 500;

            Capacity = 50.0f;
            Regeneration = 0.2f;
            ConversionFactor = 5f;

            Value = 500;
        }
    }
}
