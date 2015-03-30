using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public class MultipleShotWeapon : PlayerWeapon
    {
        public MultipleShotWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Shoots a high number of weak bullets over a wide arc";
        }

        private void Setup()
        {
            Name = "Multiple Shot";
            Kind = "Primary";
            energyCostPerSecond = 9f;
            delay = 180;
            Weight = 800;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(300, 0, 100, 100));

            bullet = new GreenBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
            numberOfShots = 5;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            double width = Math.PI / 6;

            for (double dir = -width / 2 - Math.PI / 2; dir <= width / 2 - Math.PI / 2; dir += (width / numberOfShots))
            {
                GreenBullet bullet = new GreenBullet(Game, spriteSheet);
                bullet.Position = player.Position;
                bullet.Direction = MathFunctions.DirFromRadians(dir);
                bullet.Initialize();

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
            return true;
        }
    }
}
