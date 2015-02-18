using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelMissileAttack : ShootingEnemyShip
    {
         public RebelMissileAttack(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "RebelMissileAttack";
        }

        private void Setup()
        {
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryHigh;

            AddPrimaryModule(800, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            AddSecondaryModule(1000, ShootingMode.Regular);
            secondaryModule.SetFullCharge();

            Damage = 100;
            Speed = 0.04f;
            HP = 1500;
            TurningSpeed = 2;

            movement = Movement.Line;
            SightRange = 4000;
            PrimaryShootSoundID = SoundEffects.ClickLaser;
            SecondaryShootSoundID = SoundEffects.SmallLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(524, 180, 65, 81)));
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
            var spreadAngle = Math.PI / 3;

            EnemyMissileBullet missile = new EnemyMissileBullet(Game, spriteSheet);
            missile.Position = Position;
            missile.Direction = new Vector2(0, 1.0f);
            missile.Direction = MathFunctions.SpreadDir(missile.Direction, spreadAngle);
            missile.Initialize();
            missile.Speed *= 0.6f;

            Game.stateManager.shooterState.gameObjects.Add(missile);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        {
            double width = Math.PI;
            double numberOfShots = 7;

            List<double> spreadDirections = MathFunctions.GetSpreadDirList(width, numberOfShots);

            int shotNbr = 0;
            int centerShot = (int)(numberOfShots / 2) + 1;
            foreach (double dir in spreadDirections)
            {
                shotNbr++;
                if (shotNbr == centerShot)
                    continue;

                EnemyWeakRedLaser laser = new EnemyWeakRedLaser(Game, spriteSheet);
                laser.Position = Position;
                laser.Direction = MathFunctions.DirFromRadians(dir);
                laser.Initialize();

                laser.Speed *= 0.3f;
                laser.Duration *= 1.5f;

                Game.stateManager.shooterState.gameObjects.Add(laser);

            }
        }
    }
}
