using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class BasicShield : PlayerShield
    {
        public BasicShield(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A simple shield, offering basic protection";
        }

        private void Setup()
        {
            Name = "Basic Shield";
            Weight = 700;

            Capacity = 25.0f;
            Regeneration = 0.1f;
            ConversionFactor = 5f;

            Value = 200;
        }
    }
}
