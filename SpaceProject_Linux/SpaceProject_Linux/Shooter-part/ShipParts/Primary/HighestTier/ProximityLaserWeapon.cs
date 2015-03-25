using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class ProximityLaserWeapon : PlayerWeapon
    {
        private int sideShots = 3;

        public ProximityLaserWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular)
            : base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Fans out powerful lasers at a short range";
        }

        private void Setup()
        {
            Name = "Proximity Laser";
            Kind = "Primary";
            energyCostPerSecond = 11f;
            
            delay = 50;
            Weight = 150;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(1000, 0, 100, 100));

            bullet = new AdvancedLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            ShootsInBatchesSetup(2, 400);

            Value = 2000;
            
            numberOfShots = sideShots * 2 + 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            float speedFactor = 0.5f;
            float durationFactor = 1f;

            for (int dir = -sideShots; dir <= sideShots; dir++)
            {
                AdvancedLaser shot1 = new AdvancedLaser(Game, spriteSheet);
                shot1.PositionX = player.PositionX;
                shot1.PositionY = player.PositionY;
                BasicBulletSetup(shot1);
                shot1.Radians = MathFunctions.RadiansFromDir(shot1.Direction) - dir * Math.PI / 30;
                shot1.Direction = MathFunctions.DirFromRadians(shot1.Radians);
                shot1.Speed *= speedFactor;
                shot1.Duration *= durationFactor;

                Game.stateManager.shooterState.gameObjects.Add(shot1);
            }

            return true;
        }
    }
}
