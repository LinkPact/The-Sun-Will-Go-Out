using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class HeavyPlating : PlayerPlating
    {
        public HeavyPlating(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Thick plating with high durability, but lower speed";
        }

        private void Setup()
        {
            Name = "Heavy Plating";

            armor = 600.0f;
            CurrentOverworldHealth = Armor;
            
            Speed = 0.12f;
            Acceleration = 0.015f;
            PrimarySlots = 3;

            Value = 500;
            Tier = TierType.Good;
        }

        public override void Initialize()
        { }
    }
}
