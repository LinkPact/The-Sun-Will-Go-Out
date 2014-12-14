using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BulletShield : PlayerShield
    {
        public BulletShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BulletShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "High resistance to bullets while vulnerable to physical impact";
        }

        private void Setup()
        {
            Name = "Bullet Shield";
            Kind = "Shield";
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
