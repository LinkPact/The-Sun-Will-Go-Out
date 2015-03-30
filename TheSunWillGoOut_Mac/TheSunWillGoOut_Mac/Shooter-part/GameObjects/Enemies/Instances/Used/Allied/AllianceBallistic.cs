using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    /**
     * Mid-tier ship that fires single bullets aimed at the player at a high speed
     */

    class AllianceBallistic : ShootingEnemyShip
    {
        public AllianceBallistic(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceBallistic(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            AddPrimaryModule(200, ShootingMode.Batches);
            primaryModule.ShootsInBatchesSetup(2, 1000);
            
            //Egenskaper
            SightRange = 600;
            HP = 700.0f;
            Damage = (float)CollisionDamage.high;
            Speed = 0.05f;

            movement = Movement.Line;
            PrimaryShootSoundID = SoundEffects.BigLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(440, 0, 42, 43)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyStrongBlueLaser bullet = new EnemyStrongBlueLaser(Game, spriteSheet);
            bullet.Position = Position;
            bullet.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();
            bullet.Speed *= 0.4f;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
