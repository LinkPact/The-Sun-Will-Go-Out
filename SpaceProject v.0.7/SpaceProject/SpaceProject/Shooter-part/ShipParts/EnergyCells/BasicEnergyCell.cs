using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BasicEnergyCell : PlayerEnergyCell
    {
        public BasicEnergyCell(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A simple energy cell able to keep low-range weapons firing most of the time";
        }

        private void Setup()
        {
            Name = "Basic Cell";
            Kind = "EnergyCell";
            Weight = 300;
            Capacity = 30.0f;
            Recharge = 4f;
            Value = 100;
            Tier = TierType.Average;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(800, 100, 100, 100));
        }

        public override void Initialize()
        { }
    }
}
