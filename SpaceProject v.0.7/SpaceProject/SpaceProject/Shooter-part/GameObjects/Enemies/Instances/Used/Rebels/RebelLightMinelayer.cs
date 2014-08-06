using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class RebelLightMinelayer : ShootingEnemyShip
    {
        public RebelLightMinelayer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelLightMinelayer(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
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

            lootValue = LootValue.medium;

            AddPrimaryModule(700, ShootingMode.Regular);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 4000;
            HP = 200;
            Damage = 40;
            Speed = 0.10f;

            movement = Movement.SlantingLine;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(340, 400, 32, 38)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            Vector2 centerDir = new Vector2(0, 1.0f);
            double dirRadians = MathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * Math.PI / 8 - Math.PI / 16;

            MineEnemy mine = new MineEnemy(Game, spriteSheet);
            mine.Position = Position;
            mine.Direction = MathFunctions.DirFromRadians(dirRadians);
            mine.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(mine);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
