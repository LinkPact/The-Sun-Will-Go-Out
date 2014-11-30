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
        public BasicEnergyCell(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BasicEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "A simple energy cell";
        }

        private void Setup()
        {
            Name = "Basic Cell";
            Kind = "EnergyCell";
            Weight = 300;
            Capacity = 30.0f;
            Recharge = 4f;
            Value = 100;
        }

        public override void Initialize()
        { }
    }
}
