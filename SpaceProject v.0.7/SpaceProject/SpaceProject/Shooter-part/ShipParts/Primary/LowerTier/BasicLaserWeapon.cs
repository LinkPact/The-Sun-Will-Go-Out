using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BasicLaserWeapon : PlayerWeapon
    {
        public BasicLaserWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots a single beam straight forward with decent range but with low damage and speed";
        }

        private void Setup()
        {
            Name = "Basic Laser";
            Kind = "Primary";
            energyCostPerSecond = 5f;
            delay = 180;
            Weight = 130;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(0, 0, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 100;
            Tier = TierType.Low;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            BasicLaser laser1 = new BasicLaser(Game, spriteSheet);
            laser1.PositionX = player.PositionX;
            laser1.PositionY = player.PositionY;

            BasicBulletSetup(laser1);
            Game.stateManager.shooterState.gameObjects.Add(laser1);
            return true;
        }
    }
}
