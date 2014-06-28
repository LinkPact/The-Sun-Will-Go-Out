using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PlasmaEnergyCell : PlayerEnergyCell
    {
        public PlasmaEnergyCell(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public PlasmaEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Built for recharging at high speed while having limited capacity";
        }

        private void Setup()
        {
            Name = "Plasma Cell";
            Kind = "EnergyCell";
            Weight = 300;

            Capacity = 30.0f;
            Recharge = 7.5f;

            Value = 300;
        }
    }
}
