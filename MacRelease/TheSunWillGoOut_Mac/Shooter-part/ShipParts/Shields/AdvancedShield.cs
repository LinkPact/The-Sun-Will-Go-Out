using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class AdvancedShield : PlayerShield
    {
        public AdvancedShield(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
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

            Capacity = 75.0f;
            Regeneration = 0.3f;
            ConversionFactor = 5;

            Value = 1000;
            Tier = TierType.Excellent;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(700, 200, 100, 100));
        }
    }
}
