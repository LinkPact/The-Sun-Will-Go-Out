﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class CollisionShield : PlayerShield
    {
        public CollisionShield(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public CollisionShield(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "High resistance to physical impact while vulnerable to bullets";
        }

        private void Setup()
        {
            Name = "Collision Shield";
            Kind = "Shield";
            Weight = 700;

            Capacity = 100.0f;
            Regeneration = 0.15f;
            ConversionFactor = 1;

            Value = 500;

            collisionDamageFactor = 0.7f;
            bulletDamageFactor = 1.5f;
        }
    }
}