using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class LongShotWeapon : PlayerWeapon
    {
        public LongShotWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
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

            bullet = new DistanceSpreadBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 300;
            Tier = TierType.Average;
            numberOfShots = 5;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);
            double spread = Math.PI / 32;

            for (int n = 0; n < numberOfShots; n++)
            {
                DistanceSpreadBullet shot = new DistanceSpreadBullet(Game, spriteSheet);
                shot.PositionX = player.PositionX;
                shot.PositionY = player.PositionY;

                shot.Direction = MathFunctions.SpreadDir(centerDir, spread);
                shot.Initialize();

                shot.SetSpreadSpeed(random, 0.2f);

                Game.stateManager.shooterState.gameObjects.Add(shot);
            }

            return true;
        }
    }

}
