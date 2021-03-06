﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RegularEnergyCell : PlayerEnergyCell
    {

        public RegularEnergyCell(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Mid-range energy cell able to keep mid-range weapons firing most of the time";
        }

        private void Setup()
        {
            Name = "Regular Cell";
            Weight = 200;
            Capacity = 50.0f;
            Recharge = 6f;
            Value = 600;
            Tier = TierType.Good;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(1100, 100, 100, 100));
        }

        public override void Initialize()
        { }
    }
}
