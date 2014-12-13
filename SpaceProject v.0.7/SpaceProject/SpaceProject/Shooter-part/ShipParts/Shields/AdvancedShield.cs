using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AdvancedShield : PlayerShield
    {
        public AdvancedShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public AdvancedShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Specialized for fast recharge rate but with compromised durability";
        }

        private void Setup()
        {
            Name = "Advanced Shield";
            Kind = "Shield";
            Weight = 700;

            Capacity = 70.0f;
            Regeneration = 0.2f;
            ConversionFactor = 5;

            Value = 1000;
        }
    }
}
