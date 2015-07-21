using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    class RebelHomingMissile : ShootingEnemyShip
    {
        public RebelHomingMissile(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelHomingMissile(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.pirate;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            AddPrimaryModule(1000, ShootingMode.Regular);
            
            //Egenskaper
            SightRange = 500;
            HP = 400f;
            Damage = (float)CollisionDamage.medium;
            Speed = 0.02f;

            movement = Movement.Stopping;
            PrimaryShootSoundID = SoundEffects.ClickLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(209, 63, 29, 36)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyHomingMissileBullet bullet = new EnemyHomingMissileBullet(Game, spriteSheet, player);
            bullet.Position = Position;
            bullet.SetDirectionAgainstTarget(this, ShootObject);
            bullet.Initialize();
            bullet.Speed *= 0.6f;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
