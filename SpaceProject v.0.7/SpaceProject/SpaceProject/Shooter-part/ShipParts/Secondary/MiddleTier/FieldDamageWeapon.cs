using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FieldDamageWeapon : PlayerWeapon
    {
        private readonly float blastRadius = 600;

        public FieldDamageWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
            SetShipPartVariety();
        }

        protected override String GetDescription()
        {
            return "Emits a field damaging enemies over a wide area";
        }

        private void Setup()
        {
            Name = "Field Damage";
            Kind = "Secondary";
            energyCostPerSecond = 0f;
            delay = 800;
            Weight = 400;
            ActivatedSoundID = SoundEffects.SmallLaser;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(200, 100, 100, 100));

            bullet = new BasicLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
            numberOfShots = 1;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            var gameObjectTargets = player.GetTargetsWithinRange(blastRadius);

            if (gameObjectTargets.Count == 0)
            {
                return false;
            }

            foreach (var target in gameObjectTargets)
            {
                Vector2 dir = new Vector2(target.PositionX - player.PositionX, target.PositionY - player.PositionY);
                Vector2 scaledDir = MathFunctions.ScaleDirection(dir);

                BasicLaser bullet = new BasicLaser(Game, spriteSheet);
                bullet.PositionX = player.PositionX;
                bullet.PositionY = player.PositionY;
                BasicBulletSetup(bullet);
                bullet.Direction = scaledDir;
                bullet.Speed = speed;
                bullet.Damage = damage;

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }

            return true;
        }
    }
}
