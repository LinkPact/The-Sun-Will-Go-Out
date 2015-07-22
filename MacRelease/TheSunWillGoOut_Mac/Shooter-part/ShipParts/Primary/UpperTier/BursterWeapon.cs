using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class BursterWeapon : PlayerWeapon
    {
        public BursterWeapon(Game1 Game, ItemVariety variety = ItemVariety.Regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoot cascades of bullets at medium range";
        }

        private void Setup()
        {
            Name = "Burster";
            Kind = "Primary";
            energyCostPerSecond = 12f;
            delay = 400;
            Weight = 500;
            ActivatedSoundID = SoundEffects.BigLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(700, 0, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 900;
            Tier = TierType.Great;
            numberOfShots = 12;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, -1.0f);
            double spread = Math.PI / 8;

            for (int n = 0; n < numberOfShots; n++)
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
    }
}
