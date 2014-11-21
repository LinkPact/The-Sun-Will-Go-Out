using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BasicShield : PlayerShield
    {
        public BasicShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BasicShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "A simple shield, offering basic protection";
        }

        private void Setup()
        {
            Name = "Basic Shield";
            Kind = "Shield";
            Weight = 700;

            Capacity = 30.0f;
            Regeneration = 0.1f;
            ConversionFactor = 5f;

            Value = 200;
        }
    }
}
