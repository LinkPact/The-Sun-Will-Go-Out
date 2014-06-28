using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class DurableShield : PlayerShield
    {
        public DurableShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public DurableShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Exceeds in its high durability, while having a lower recharge speed";
        }

        private void Setup()
        {
            Name = "Durable Shield";
            Kind = "Shield";
            Weight = 700;

            Capacity = 150.0f;
            Regeneration = 0.1f;
            ConversionFactor = 5.0f;

            Value = 500;
        }
    }
}
