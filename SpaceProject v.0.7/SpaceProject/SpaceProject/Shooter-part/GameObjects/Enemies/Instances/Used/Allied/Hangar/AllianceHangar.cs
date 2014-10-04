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
            SightRange = 500;
            HP = 1000.0f;
            Damage = 0;
            Speed = 0.01f;

            movement = Movement.Line;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(0, 380, 159, 258)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            AllianceHangarAttackShip ship = new AllianceHangarAttackShip(Game, spriteSheet, player);
            ship.Position = Position;
            ship.Direction = MathFunctions.DirFromRadians(Math.PI);
            ship.Initialize();
            ship.SetMovement(Movement.SearchAndLockOn);
            ship.Speed *= 0.8f;
            ship.TurningSpeed = 2f;

            Game.AddGameObjToShooter(ship);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
