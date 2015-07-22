using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelMinelayer : ShootingEnemyShip
    {
        private const int mineActivationTime = 700;

        public RebelMinelayer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelMinelayer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.high;

            AddPrimaryModule(2000, ShootingMode.Regular);
            AddSecondaryModule(900, ShootingMode.Regular);

            //Egenskaper
            SightRange = 1000;
            HP = 700.0f;
            HPmax = HP;
            Damage = (float)CollisionDamage.high;
            Speed = 0.05f;

            movement = Movement.SmallZigzag;
            PrimaryShootSoundID = SoundEffects.ClickLaser;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(320, 80, 54, 38)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, 1.0f);
            double dirRadians = MathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * Math.PI / 8 - Math.PI / 16;

            EnemyMine mine = new EnemyMine(Game, spriteSheet, player);
            mine.Initialize();
            mine.Position = Position;
            mine.Direction = Direction;
            mine.blastRadius *= 1.5f;

            mine.SetActivationTime(mineActivationTime);

            Game.stateManager.shooterState.gameObjects.Add(mine);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        {
            double width = 2 * Math.PI;
            int numberOfShots = 6;

            double randomOffset = random.NextDouble() * Math.PI * 2;
            
            List<double> spreadDirections = MathFunctions.GetSpreadDirList(width, numberOfShots);
            foreach (double dir in spreadDirections)
            {
                EnemyWeakRedLaser laser1 = new EnemyWeakRedLaser(Game, spriteSheet);
                laser1.PositionX = PositionX;
                laser1.PositionY = PositionY;

                double shootDir = dir + randomOffset;

                laser1.Direction = MathFunctions.DirFromRadians(shootDir);
                laser1.Initialize();
                laser1.Speed *= 1.0f;
                laser1.Duration *= 0.4f;

                Game.stateManager.shooterState.gameObjects.Add(laser1);
            }
        }
    }
}
