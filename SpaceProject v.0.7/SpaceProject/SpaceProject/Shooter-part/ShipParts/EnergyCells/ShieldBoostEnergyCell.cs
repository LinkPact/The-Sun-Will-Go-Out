using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class ShieldBoostEnergyCell : PlayerEnergyCell
    {

        public ShieldBoostEnergyCell(Game1 Game)
            : base(Game)
        {
            Setup();
        }

        public ShieldBoostEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Enhances shield regeneration while impairing weapon energy";
        }

        private void Setup()
        {
            Name = "Shield Boost Cell";
            Kind = "EnergyCell";
            Weight = 500;

            Capacity = 40.0f;
            Recharge = 6.5f;

            Value = 500;
        }

        public override void Initialize()
        { }
    }
}
