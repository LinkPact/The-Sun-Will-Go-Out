﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelYellowEnemy : ShootingEnemyShip
    {
        private Animation shooting;

        public RebelYellowEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            ObjectName = "RebelYellowEnemy";
            fraction = Fraction.rebel;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 1;
            lootRangeMax = 4;

            //Shooting
            shootingDelay = 3000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 500;
            HP = 150;
            Damage = 50;
            Speed = 0.07f;
           
            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(401, 45, 23, 25)));
            
            shooting = new Animation();
            shooting.LoopTime = 1000;
            
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(426, 45, 23, 25)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(451, 45, 23, 25)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(476, 45, 23, 25)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(501, 45, 23, 25)));
            
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateAnimation(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsKilled)
            {
                if (lastTimeShot >= shootingDelay - 1000 && ShootObject != null)
                    spriteBatch.Draw(shooting.CurrentFrame.Texture, Position, shooting.CurrentFrame.SourceRectangle, Color.White, 0.0f, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
                else
                {
                    base.Draw(spriteBatch);
                }
            }
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (lastTimeShot >= shootingDelay - 1000)
                shooting.Update(gameTime);
            
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyGreenBullet bullet = new EnemyGreenBullet(Game, spriteSheet);
            bullet.PositionX = PositionX;
            bullet.PositionY = PositionY;
            bullet.Direction = GlobalFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();
            bullet.duration = 500;
            bullet.Speed *= 1.5f;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

    }
}