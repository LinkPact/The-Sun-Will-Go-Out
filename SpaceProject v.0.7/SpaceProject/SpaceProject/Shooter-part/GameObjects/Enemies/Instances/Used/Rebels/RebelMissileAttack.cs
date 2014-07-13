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
            
            lootRangeMin = 1;
            lootRangeMax = 3;

            shootingDelay = 500;
            lastTimeShot = shootingDelay * random.NextDouble();

            Damage = 100;
            Speed = 0.04f;
            HP = 400;
            TurningSpeed = 2;

            movement = Movement.Line;
            SightRange = 4000;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(380, 220, 42, 43)));
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
            EnemyMissileBullet missile = new EnemyMissileBullet(Game, spriteSheet);
            missile.Position = Position;
            missile.Direction = new Vector2(0, 1.0f);
            missile.Direction = GlobalMathFunctions.SpreadDir(missile.Direction, Math.PI / 8);
            missile.Initialize();
            missile.Duration = 500;

            double width = Math.PI / 3;
            double numberOfShots = 5;

            List<double> spreadDirections = GlobalMathFunctions.GetSpreadDirList(width, numberOfShots);

            int shotNbr = 0;
            foreach (double dir in spreadDirections)
            {
                shotNbr++;
                if (shotNbr == 3)
                    continue;

                EnemyWeakRedLaser laser = new EnemyWeakRedLaser(Game, spriteSheet);
                laser.Position = Position;
                laser.Direction = GlobalMathFunctions.DirFromRadians(dir);
                laser.Initialize();

                laser.Duration = 1000;
                laser.Speed *= 0.3f;

                Game.stateManager.shooterState.gameObjects.Add(laser);

            }

            Game.stateManager.shooterState.gameObjects.Add(missile);

            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BasicLaser, soundPan);
        }
    }

}
