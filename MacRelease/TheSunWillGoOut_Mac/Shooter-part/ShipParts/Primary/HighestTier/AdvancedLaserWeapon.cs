using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class AdvancedLaserWeapon : PlayerWeapon
    {
        public AdvancedLaserWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots two powerful beams straight forward with high damage and range";
        }

        private void Setup()
        {
            Name = "Advanced Laser";
            Kind = "Primary";
            energyCostPerSecond = 11f;
            delay = 180;
            Weight = 400;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(900, 0, 100, 100));

            bullet = new AdvancedLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 1800;
            Tier = TierType.Excellent;
            numberOfShots = 2;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            AdvancedLaser laser1 = new AdvancedLaser(Game, spriteSheet);
            laser1.PositionX = player.PositionX - 4;
            laser1.PositionY = player.PositionY;

            BasicBulletSetup(laser1);

            AdvancedLaser laser2 = new AdvancedLaser(Game, spriteSheet);
            laser2.PositionX = player.PositionX + 4;
            laser2.PositionY = player.PositionY;

            BasicBulletSetup(laser2);

            Game.stateManager.shooterState.gameObjects.Add(laser1);
            Game.stateManager.shooterState.gameObjects.Add(laser2);

            return true;
        }
    }
}
