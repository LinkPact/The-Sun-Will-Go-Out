using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class EmptyShield : PlayerShield
    {

        public EmptyShield(Game1 Game, ItemVariety variety=ItemVariety.regular) :
            base(Game, variety)
        {
            Name = "---";
            Kind = "Empty";
            Capacity = 0;
            ConversionFactor = 0;
        }

        protected override string GetDescription()
        {
            return "";
        }
    }
}
