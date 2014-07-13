using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class MultipleShotWeapon : PlayerWeapon
    {
        public MultipleShotWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public MultipleShotWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots bullets over a wide arc, effective again swarms of weak enemies";
        }

        private void Setup()
        {
            Name = "Multiple Shot";
            Kind = "Primary";
            energyCostPerSecond = 10f;
            delay = 120;
            Weight = 800;

            bullet = new GreenBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 400;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            double width = Math.PI / 6;
            int numberOfShots = 5;

            for (double dir = -width / 2 - Math.PI / 2; dir <= width / 2 - Math.PI / 2; dir += (width / numberOfShots))
            {
                GreenBullet bullet = new GreenBullet(Game, spriteSheet);
                bullet.Position = player.Position;
                bullet.Direction = GlobalMathFunctions.DirFromRadians(dir);
                bullet.Initialize();

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
            return true;
        }

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test2, 0f);
        }

    }
}
