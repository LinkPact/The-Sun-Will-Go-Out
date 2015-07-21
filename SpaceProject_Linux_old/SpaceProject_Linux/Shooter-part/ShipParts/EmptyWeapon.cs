using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public class EmptyWeapon : PlayerWeapon
    {
        public EmptyWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Name = "---";
            Kind = "Empty";
            energyCostPerSecond = 0;
            delay = 0;
        }

        protected override String GetDescription()
        {
            return "";
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            return false;
        }
    }
}
