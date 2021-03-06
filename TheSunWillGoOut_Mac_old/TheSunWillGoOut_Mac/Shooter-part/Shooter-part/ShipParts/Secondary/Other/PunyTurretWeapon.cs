﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PunyTurretWeapon : PlayerWeapon
    {
        public PunyTurretWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Short-ranged turret that aims and shoots at enemies";
        }

        private void Setup()
        {
            Name = "Puny Turret";
            Kind = "Secondary";
            energyCostPerSecond = 0f;
            delay = 100;
            Weight = 130;
            ActivatedSoundID = SoundEffects.ClickLaser;

            bullet = new GreenBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 75;
            Tier = TierType.Average;
            ShootsInBatchesSetup(3, 1500);
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            player.SightRange = CalculateRange();
            GameObjectVertical target = player.FindAimObject();
            if (target == null) return false;

            Vector2 dir = new Vector2(target.PositionX - player.PositionX, target.PositionY - player.PositionY);
            Vector2 scaledDir = MathFunctions.ScaleDirection(dir);

            GreenBullet bul = new GreenBullet(Game, spriteSheet);
            bul.PositionX = player.PositionX;
            bul.PositionY = player.PositionY;
            BasicBulletSetup(bul);
            bul.Direction = scaledDir;
            bul.Damage *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(bul);
            return true;
        }
    }
}
