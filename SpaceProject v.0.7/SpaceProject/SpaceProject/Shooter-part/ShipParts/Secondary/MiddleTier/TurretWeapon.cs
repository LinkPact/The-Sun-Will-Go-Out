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
        public TurretWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public TurretWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Powerful turret that aims and shoots at enemies";
        }

        private void Setup()
        {
            Name = "Turret";
            Kind = "Secondary";
            energyCostPerSecond = 4f;
            delay = 1000;
            Weight = 130;

            bullet = new BallisticLaser(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            player.SightRange = 1000;
            GameObjectVertical target = player.FindAimObject();
            if (target == null) return false;

            Vector2 dir = new Vector2(target.PositionX - player.PositionX, target.PositionY - player.PositionY);
            Vector2 scaledDir = MathFunctions.ScaleDirection(dir);

            BallisticLaser bullet = new BallisticLaser(Game, spriteSheet);
            bullet.PositionX = player.PositionX;
            bullet.PositionY = player.PositionY;
            BasicBulletSetup(bullet);
            bullet.Direction = scaledDir;
            bullet.Speed *= 0.7f;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
            return true;
        }

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, 0f);
        }
    }
}
