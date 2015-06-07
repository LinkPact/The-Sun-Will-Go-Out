using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class LongShotWeapon : PlayerWeapon
    {
        public LongShotWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoot tight cascades of bullets at long range";
        }

        private void Setup()
        {
            Name = "LongShot";
            Kind = "Primary";
            energyCostPerSecond = 6f;
            delay = 800;
            Weight = 500;
            ActivatedSoundID = SoundEffects.MidSizeLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(500, 100, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 300;
            numberOfShots = 6;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);
            double spread = Math.PI / 32;

            for (int n = 0; n < numberOfShots; n++)
            {
                DistanceSpreadBullet bullet = new DistanceSpreadBullet(Game, spriteSheet);
                bullet.PositionX = player.PositionX;
                bullet.PositionY = player.PositionY;

                bullet.Direction = MathFunctions.SpreadDir(centerDir, spread);
                bullet.Initialize();
                bullet.Speed *= 1f;
                bullet.Damage *= 0.7f;

                bullet.SetSpreadSpeed(random, 0.2f);

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }

            return true;
        }
    }

}
