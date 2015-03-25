using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class ShieldBoostEnergyCell : PlayerEnergyCell
    {

        public ShieldBoostEnergyCell(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Enhances shield regeneration while impairing weapon energy";
        }

        private void Setup()
        {
            Name = "Shield Boost Cell";
            Weight = 500;

            Capacity = 75.0f;
            Recharge = 9f;

            Value = 500;
        }

        public override void Initialize()
        { }
    }
}
