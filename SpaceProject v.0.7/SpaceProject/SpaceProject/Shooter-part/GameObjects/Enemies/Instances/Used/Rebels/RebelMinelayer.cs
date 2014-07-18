﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelMinelayer : ShootingEnemyShip
    {
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

            lootRangeMin = 4;
            lootRangeMax = 7;

            //Shooting
            shootingDelay = 1500;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 1000;
            HP = 400.0f;
            Damage = 130;
            Speed = 0.05f;

            movement = Movement.Zigzag;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(320, 80, 54, 38)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
            Vector2 centerDir = new Vector2(0, 1.0f);
            double dirRadians = GlobalMathFunctions.RadiansFromDir(centerDir);
            dirRadians += random.NextDouble() * Math.PI / 8 - Math.PI / 16;

            MineEnemy mine = new MineEnemy(Game, spriteSheet);
            mine.Position = Position;
            //mine.Direction = GlobalFunctions.DirFromRadians(dirRadians);
            mine.Direction = GlobalMathFunctions.GetRandomDownDirection();
            mine.Initialize();
            mine.Speed = 0.003f;
            mine.Duration = 6000;

            Game.stateManager.shooterState.gameObjects.Add(mine);
        }
    }
}