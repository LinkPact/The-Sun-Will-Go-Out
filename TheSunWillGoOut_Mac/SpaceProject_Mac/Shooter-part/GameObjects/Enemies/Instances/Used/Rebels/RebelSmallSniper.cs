using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class RebelSmallSniper : ShootingEnemyShip
    {
        private Animation shooting;

        public RebelSmallSniper(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            ObjectName = "RebelYellowEnemy";
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryLow;

            AddPrimaryModule(3000, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 500;
            HP = 200;
            Damage = (float)CollisionDamage.low;
            Speed = 0.1f;

            movement = Movement.Stopping;
           
            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(509, 80, 25, 30)));
            
            shooting = new Animation();
            shooting.LoopTime = 1000;
            PrimaryShootSoundID = SoundEffects.ClickLaser;
            
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(509, 80, 25, 30)));
            
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyGreenBullet bullet = new EnemyGreenBullet(Game, spriteSheet);
            bullet.PositionX = PositionX;
            bullet.PositionY = PositionY;
            bullet.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();
            bullet.Duration *= 1;
            bullet.Speed *= 0.8f;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
