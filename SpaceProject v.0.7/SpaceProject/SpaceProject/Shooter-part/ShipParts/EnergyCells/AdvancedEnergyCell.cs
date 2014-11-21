using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AdvancedEnergyCell : PlayerEnergyCell
    {
        public AdvancedEnergyCell(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public AdvancedEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "A high-level energy cell";
        }

        private void Setup()
        {
            Name = "Advanced Cell";
            Kind = "EnergyCell";
            Weight = 300;

            Capacity = 100.0f;
            Recharge = 7.5f;

            Value = 1200;
        }
    }
}
