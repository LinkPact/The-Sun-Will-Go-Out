using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class RebelThickShooter : ShootingEnemyShip
    {
     
        public RebelThickShooter(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "RebelThickShooter";
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.low;

            AddPrimaryModule(2000, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            Damage = (float)CollisionDamage.medium;
            Speed = 0.08f;
            HP = 200;
            TurningSpeed = 2;

            movement = Movement.Following;
            SightRange = 400;
            PrimaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(484, 0, 23, 34)));
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                base.Draw(spriteBatch);
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyWeakRedLaser laser1 = new EnemyWeakRedLaser(Game, spriteSheet);
            laser1.PositionX = PositionX - 4;
            laser1.PositionY = PositionY;
            laser1.Direction = new Vector2(0, 1.0f);
            laser1.Initialize();
            laser1.Duration *= 0.8f;

            EnemyWeakRedLaser laser2 = new EnemyWeakRedLaser(Game, spriteSheet);
            laser2.PositionX = PositionX + 4;
            laser2.PositionY = PositionY;
            laser2.Direction = new Vector2(0, 1.0f);
            laser2.Initialize();
            laser2.Duration *= 0.8f;

            Game.stateManager.shooterState.gameObjects.Add(laser1);
            Game.stateManager.shooterState.gameObjects.Add(laser2);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
