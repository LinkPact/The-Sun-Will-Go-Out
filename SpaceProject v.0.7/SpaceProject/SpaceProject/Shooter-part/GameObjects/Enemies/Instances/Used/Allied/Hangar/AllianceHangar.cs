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
            ShieldSetup(CreatureShieldCapacity.extreme, CreatureShieldRegeneration.extreme);
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryHigh;

            AddPrimaryModule(600, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);
            primaryModule.ShootsInBatchesSetup(2, 7000);

            AddSecondaryModule(2500, ShootingMode.Regular);
            secondaryModule.ShootsInBatchesSetup(4, 8000);

            SightRange = 1000;
            HP = 3000.0f;
            Damage = 10000;
            Speed = 0.01f;
            movement = Movement.Line;

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
        {
            int numberOfShots = 8;

            for (int n = 0; n < numberOfShots; n++)
            {
                EnemyStrongBlueLaser bullet = new EnemyStrongBlueLaser(Game, spriteSheet);
                bullet.Position = Position;
                bullet.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
                bullet.Direction = MathFunctions.SpreadDir(bullet.Direction, Math.PI / 16);
                bullet.Initialize();
                bullet.SetSpreadSpeed(random);
                bullet.Speed *= 0.35f;

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }


        }
    }
}
