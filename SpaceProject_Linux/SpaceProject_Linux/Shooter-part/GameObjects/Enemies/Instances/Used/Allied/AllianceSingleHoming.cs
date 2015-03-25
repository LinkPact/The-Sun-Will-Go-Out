using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    /**
     * Small, slow and weak lower-tier ship that shoots homing bullets
     */

    class AllianceSingleHoming : ShootingEnemyShip
    {
        public AllianceSingleHoming(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceSingleHoming(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            lootValue = LootValue.veryLow;

            //Egenskaper
            SightRange = 400;
            HP = 200f;
            Damage = (float)CollisionDamage.low;
            Speed = 0.04f;

            AddPrimaryModule(1000, ShootingMode.Regular);
            primaryModule.SetFullCharge();

            movement = Movement.SlantingLine;
            PrimaryShootSoundID = SoundEffects.ClickLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(290, 379, 25, 30)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyHomingBullet bullet = new EnemyHomingBullet(Game, spriteSheet, player);
            bullet.Position = Position;
            bullet.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
