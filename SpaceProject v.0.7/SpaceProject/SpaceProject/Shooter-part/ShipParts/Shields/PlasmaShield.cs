using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PlasmaShield : PlayerShield
    {
        public PlasmaShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public PlasmaShield(Game1 Game, ItemVariety variety) :
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
            Name = "Plasma Shield";
            Kind = "Shield";
            Weight = 700;

            Capacity = 60.0f;
            Regeneration = 0.25f;
            ConversionFactor = 1;

            Value = 500;
        }
    }
}
