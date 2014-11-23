using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class BursterWeapon : PlayerWeapon
    {
        public BursterWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BursterWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoot cascades of bullets at medium range";
        }

        private void Setup()
        {
            Name = "Burster";
            Kind = "Primary";
            //energyCost = 0.15f;
            energyCostPerSecond = 12f;
            delay = 400;
            Weight = 500;

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 300;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {

            Vector2 centerDir = new Vector2(0, -1.0f);
            int nbrOfShots = 12;
            double spread = Math.PI / 8;

            for (int n = 0; n < nbrOfShots; n++)
            {
                BasicLaser bullet = new BasicLaser(Game, spriteSheet);
                bullet.PositionX = player.PositionX;
                bullet.PositionY = player.PositionY;

                bullet.Direction = MathFunctions.SpreadDir(centerDir, spread);
                bullet.Initialize();
                bullet.Duration *= 0.8f;
                bullet.Speed *= 1f;
                bullet.Damage *= 0.7f;

                bullet.SetSpreadSpeed(random);

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }

            return true;
        }

        public override void PlaySound()
        {
            //Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test3, 0f);
        }
    }
}
