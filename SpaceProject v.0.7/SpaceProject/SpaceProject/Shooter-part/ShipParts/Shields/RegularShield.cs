using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RegularShield : PlayerShield
    {
        public RegularShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public RegularShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "All-round shield with decent recharge rate and decent capacity";
        }

        private void Setup()
        {
            Name = "Regular Shield";
            Kind = "Shield";
            Weight = 500;

            Capacity = 50.0f;
            Regeneration = 0.1f;
            ConversionFactor = 2f;

            Value = 200;
        }
    }
}
