using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class WeaponBoostEnergyCell : PlayerEnergyCell
    {

        public WeaponBoostEnergyCell(Game1 Game)
            : base(Game)
        {
            Setup();
        }

        public WeaponBoostEnergyCell(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Enhances energy to weapons while greatly impairing shield regeneration";
        }

        private void Setup()
        {
            Name = "Weapon Boost Cell";
            Kind = "EnergyCell";
            Weight = 200;

            Capacity = 75.0f;
            Recharge = 9f;

            Value = 500;
        }

        public override void Initialize()
        { }
    }
}
