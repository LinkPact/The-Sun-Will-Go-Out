﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class HomingMissileWeapon : PlayerWeapon
    {
        public HomingMissileWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public HomingMissileWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a single missile able to target and follow enemies";
        }

        private void Setup()
        {
            Name = "HomingMissile";
            Kind = "Secondary";
            energyCostPerSecond = 2f;
            delay = 800;
            Weight = 500;

            bullet = new HomingMissile(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 800;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            HomingMissile missile1 = new HomingMissile(Game, spriteSheet);
            //Position
            missile1.PositionX = player.PositionX;
            missile1.PositionY = player.PositionY;
            //Direction
            missile1.Direction = new Vector2(0.0f, -1.0f);
            missile1.Initialize();
            
            Game.stateManager.shooterState.gameObjects.Add(missile1);
            return true;
        }
    }
}
