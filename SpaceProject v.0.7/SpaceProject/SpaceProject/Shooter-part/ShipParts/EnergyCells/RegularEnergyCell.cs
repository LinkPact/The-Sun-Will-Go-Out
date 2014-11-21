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

        public RegularEnergyCell(Game1 Game) 
            : base(Game)
        {
            Setup();
        }

        public RegularEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Mid-range energy cell";
        }

        private void Setup()
        {
            Name = "Regular Cell";
            Kind = "EnergyCell";
            Weight = 200;

            Capacity = 50.0f;
            Recharge = 5.0f;

            Value = 600;
        }

        public override void Initialize()
        { }
    }
}
