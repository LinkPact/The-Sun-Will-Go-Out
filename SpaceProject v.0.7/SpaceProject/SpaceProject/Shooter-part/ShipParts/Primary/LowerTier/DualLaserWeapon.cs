using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class DualLaserWeapon : PlayerWeapon
    {
        public DualLaserWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Basic weapon with decent damage and range that shoots two beams straight forward";
        }

        private void Setup()
        {
            Name = "Dual Laser";
            Kind = "Primary";
            energyCostPerSecond = 5.5f;
            delay = 320;
            Weight = 200;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(100, 0, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 200;
            numberOfShots = 2;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            BasicLaser laser1 = new BasicLaser(Game, spriteSheet);
            laser1.PositionX = player.PositionX - 4;
            laser1.PositionY = player.PositionY;

            BasicBulletSetup(laser1);

            BasicLaser laser2 = new BasicLaser(Game, spriteSheet);
            laser2.PositionX = player.PositionX + 4;
            laser2.PositionY = player.PositionY;

            BasicBulletSetup(laser2);

            Game.stateManager.shooterState.gameObjects.Add(laser1);
            Game.stateManager.shooterState.gameObjects.Add(laser2);
            return true;
        }
    }
}
