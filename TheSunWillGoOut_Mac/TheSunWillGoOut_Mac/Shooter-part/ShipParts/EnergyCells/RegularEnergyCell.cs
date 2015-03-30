using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class RegularEnergyCell : PlayerEnergyCell
    {

        public RegularEnergyCell(Game1 Game, ItemVariety variety = ItemVariety.regular) :
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
        }

        public override void Initialize()
        { }
    }
}
