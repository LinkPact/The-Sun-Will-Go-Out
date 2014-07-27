using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class AllianceHangar : ShootingEnemyShip
    {
        public AllianceHangar(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public AllianceHangar(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
            ShieldSetup(CreatureShieldCapacity.high, CreatureShieldRegeneration.high);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryHigh;

            AddPrimaryModule(1500, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 4000;
            HP = 1000.0f;
            Damage = 0;
            Speed = 0.01f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(420, 340, 58, 58)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            RebelSmallAttackShip ship = new RebelSmallAttackShip(Game, spriteSheet, player);
            ship.Position = Position;
            ship.Direction = new Vector2(0, 1);
            ship.Initialize();
            ship.SetMovement(Movement.Following);

            Game.AddGameObjToShooter(ship);

            //EnemyGreenBullet bullet = new EnemyGreenBullet(Game, spriteSheet);
            //bullet.PositionX = PositionX;
            //bullet.PositionY = PositionY;
            //bullet.Direction = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);
            //bullet.Initialize();
            //bullet.Duration = 500;
            //bullet.Speed *= 1.5f;
            //
            //Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
