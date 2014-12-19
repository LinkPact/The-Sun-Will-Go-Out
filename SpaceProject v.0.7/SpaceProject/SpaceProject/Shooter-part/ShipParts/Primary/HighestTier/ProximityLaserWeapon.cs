﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class ProximityLaserWeapon : PlayerWeapon
    {

        public ProximityLaserWeapon(Game1 Game)
            : base(Game)
        
        {
            Setup();
        }

        public ProximityLaserWeapon(Game1 Game, ItemVariety variety)
            : base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Fans out powerful lasers at a short range";
        }

        private void Setup()
        {
            Name = "Proximity Laser";
            Kind = "Primary";
            energyCostPerSecond = 11f;
            delay = 80;
            Weight = 150;

            bullet = new AdvancedLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            ShootsInBatchesSetup(3, 500);

            Value = 2000;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            float speedFactor = 0.5f;
            float durationFactor = 1f;
            float damageFactor = 1f;

            for (int dir = -3; dir <= 3; dir++)
            {
                AdvancedLaser shot1 = new AdvancedLaser(Game, spriteSheet);
                shot1.PositionX = player.PositionX;
                shot1.PositionY = player.PositionY;
                BasicBulletSetup(shot1);
                shot1.Radians = MathFunctions.RadiansFromDir(shot1.Direction) - dir * Math.PI / 30;
                shot1.Direction = MathFunctions.DirFromRadians(shot1.Radians);
                shot1.Speed *= speedFactor;
                shot1.Duration *= durationFactor;
                shot1.Damage *= damageFactor;

                Game.stateManager.shooterState.gameObjects.Add(shot1);
            }

            return true;
        }
    }
}