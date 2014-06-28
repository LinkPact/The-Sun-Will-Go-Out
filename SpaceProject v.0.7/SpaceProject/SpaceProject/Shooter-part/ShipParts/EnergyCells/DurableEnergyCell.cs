using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class DurableEnergyCell : PlayerEnergyCell
    {
        public DurableEnergyCell(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public DurableEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "High energy storing capacity but slow recharge";
        }

        private void Setup()
        {
            Name = "Durable Cell";
            Kind = "EnergyCell";
            Weight = 300;
            Capacity = 150.0f;
            Recharge = 6f;
            Value = 300;
        }

        public override void Initialize()
        { }
    }
}
