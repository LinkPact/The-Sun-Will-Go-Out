using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class BulletShield : PlayerShield
    {
        public BulletShield(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "High resistance to bullets while vulnerable to physical impact";
        }

        private void Setup()
        {
            Name = "Bullet Shield";
            Weight = 700;

            Capacity = 70.0f;
            Regeneration = 0.3f;
            ConversionFactor = 1;

            Value = 500;
            collisionDamageFactor = 1.5f;
            bulletDamageFactor = 0.7f;
        }
    }
}
