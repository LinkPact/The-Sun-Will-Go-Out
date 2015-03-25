using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class AdvancedShield : PlayerShield
    {
        public AdvancedShield(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "High level shield for top protection";
        }

        private void Setup()
        {
            Name = "Advanced Shield";
            Weight = 700;

            Capacity = 70.0f;
            Regeneration = 0.3f;
            ConversionFactor = 5;

            Value = 1000;
        }
    }
}
