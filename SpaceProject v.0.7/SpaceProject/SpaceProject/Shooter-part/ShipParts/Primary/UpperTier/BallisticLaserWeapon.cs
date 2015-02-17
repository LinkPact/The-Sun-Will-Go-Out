using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BallisticLaserWeapon : PlayerWeapon
    {
        public BallisticLaserWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        public BallisticLaserWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots one single powerful beam at incredible speed, high damage and long range, but with slow recharge time";
        }

        private void Setup()
        {
            Name = "Ballistic Laser";
            Kind = "Primary";
            energyCostPerSecond = 10f;
            delay = 1200;
            Weight = 200;
            ActivatedSoundID = SoundEffects.BigLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(600, 0, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = 150;     // Nonsense value!
            duration = 1000;
            speed = 1.5f;     // Nonsense value!

            Value = 1000;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            BallisticLaser laser = new BallisticLaser(Game, spriteSheet);
            laser.PositionX = player.PositionX;
            laser.PositionY = player.PositionY;

            BasicBulletSetup(laser);

            Game.stateManager.shooterState.gameObjects.Add(laser);
            return true;
        }
    }
}
