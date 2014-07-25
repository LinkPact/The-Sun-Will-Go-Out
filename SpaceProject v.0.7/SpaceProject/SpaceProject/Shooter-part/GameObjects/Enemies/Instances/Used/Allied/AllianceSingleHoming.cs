using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
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
            Damage = 40;
            Speed = 0.04f;

            AddPrimaryModule(1000, ShootingMode.Regular);
            primaryModule.SetFullCharge();

            movement = Movement.SlantingLine;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(340, 340, 32, 38)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyHomingBullet bullet = new EnemyHomingBullet(Game, spriteSheet, player);
            bullet.Position = Position;
            bullet.Direction = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
