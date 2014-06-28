using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class SpreadBulletWeapon : PlayerWeapon
    {
        public SpreadBulletWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public SpreadBulletWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoot cascades of small bullets at a wide arc and a close range";
        }

        private void Setup()
        {
            Name = "Spread Bullet";
            Kind = "Primary";
            //energyCost = 0.15f;
            energyCostPerSecond = 9f;
            delay = 14;
            Weight = 500;

            bullet = new YellowBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 300;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);

            double dirRadians = GlobalFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * 2 * Math.PI / 12 - Math.PI / 12;

            YellowBullet bullet = new YellowBullet(Game, spriteSheet);
            bullet.Position = player.Position;
            bullet.Direction = GlobalFunctions.DirFromRadians(dirRadians);
            bullet.Initialize();

            bullet.Speed += ((float)random.NextDouble()) * 1.0f * bullet.Speed - 0.5f * bullet.Speed;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
            return true;
        }

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test3, 0f);
        }
    }
}
