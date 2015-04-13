using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class AdvancedEnergyCell : PlayerEnergyCell
    {
        public AdvancedEnergyCell(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "A high-level energy cell able to keep high-level weapons firing most of the time";
        }

        private void Setup()
        {
            Name = "Advanced Cell";
            Weight = 300;

            Capacity = 75.0f;
            Recharge = 9f;

            Value = 1200;
        }
    }
}
