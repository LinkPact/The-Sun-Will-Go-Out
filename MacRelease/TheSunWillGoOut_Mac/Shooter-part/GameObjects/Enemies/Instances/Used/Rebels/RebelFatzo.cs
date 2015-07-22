using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class RebelFatzo : ShootingEnemyShip
    {
        public RebelFatzo(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelFatzo(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
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

            lootValue = LootValue.veryHigh;

            //Egenskaper
            SightRange = 4000;
            HP = 2000f;
            HPmax = HP;
            Damage = (float)CollisionDamage.extreme;
            Speed = 0.02f;

            AddPrimaryModule(700, ShootingMode.Regular);
            primaryModule.ShootsInBatchesSetup(2, 2500);
            
            AddSecondaryModule(3000, ShootingMode.Regular);
            secondaryModule.SetFullCharge();

            movement = Movement.Line;
            PrimaryShootSoundID = SoundEffects.ClickLaser;
            SecondaryShootSoundID = SoundEffects.ClickLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(170, 379, 77, 98)));

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
        {
            int activationTimeMilliseconds = 500;
            EnemyMine mine = new EnemyMine(Game, spriteSheet, player);
            mine.Initialize();
            mine.SetActivationTime(activationTimeMilliseconds);
            mine.Position = Position;

            mine.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            mine.Speed = 0.15f;

            float distance = MathFunctions.ObjectDistance(this, ShootObject);

            // Shoots at a location close to the players current
            mine.Duration = distance / mine.Speed * 0.9f;

            Game.stateManager.shooterState.gameObjects.Add(mine);
        }
    }
}
