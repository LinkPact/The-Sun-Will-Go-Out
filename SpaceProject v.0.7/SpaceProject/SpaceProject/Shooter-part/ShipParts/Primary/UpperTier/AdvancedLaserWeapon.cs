using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class AdvancedLaserWeapon : PlayerWeapon
    {
        public AdvancedLaserWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public AdvancedLaserWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots two powerful beams straight forward with high damage and range";
        }

        private void Setup()
        {
            Name = "Advanced Laser";
            Kind = "Primary";
            energyCostPerSecond = 12f;
            delay = 180;
            Weight = 400;

            bullet = new AdvancedLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 700;
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

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, 0f);
        }
    }
}
