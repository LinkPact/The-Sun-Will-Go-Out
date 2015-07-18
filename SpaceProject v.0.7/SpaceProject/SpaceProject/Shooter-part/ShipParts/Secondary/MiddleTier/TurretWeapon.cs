using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class TurretWeapon : PlayerWeapon
    {
        public TurretWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Powerful turret that aims and shoots at close enemies";
        }

        private void Setup()
        {
            Name = "Turret";
            Kind = "Secondary";
            energyCostPerSecond = 0f;
            delay = 600;
            Weight = 130;
            ActivatedSoundID = SoundEffects.MidSizeLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(0, 100, 100, 100));

            bullet = new TurretBullet(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
            Tier = TierType.Great;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            player.SightRange = 300;
            GameObjectVertical target = player.FindAimObject();
            if (target == null) 
            { 
                return false; 
            }

            Vector2 dir = new Vector2(target.PositionX - player.PositionX, target.PositionY - player.PositionY);
            Vector2 scaledDir = MathFunctions.ScaleDirection(dir);

            TurretBullet bullet = new TurretBullet(Game, spriteSheet);
            bullet.PositionX = player.PositionX;
            bullet.PositionY = player.PositionY;
            BasicBulletSetup(bullet);
            bullet.Direction = scaledDir;
            bullet.Speed = speed;
            bullet.Damage = damage;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
            return true;
        }
    }
}
