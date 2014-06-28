using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class BlueEnemy : ShootingEnemyShip
    {
        private Animation shooting;

        public BlueEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            lootRangeMin = 3;
            lootRangeMax = 6;

            //Shooting
            shootingDelay = 3000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 300;
            HP = 175; //80;
            Damage = 60;
            Speed = 0.07f;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(72, 34, 18, 18)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(90, 34, 18, 18)));

            shooting = new Animation();
            shooting.LoopTime = 1000;
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(108, 34, 18, 18)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(126, 34, 18, 18)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(144, 34, 18, 18)));
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(162, 34, 18, 18)));
            //

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateAnimation(gameTime);

            if (PositionX <= relativeOrigin/* -50*/)
            {
                //Game.Window.Title = "OUTSIDE!!";
                //HP = 0;
                IsOutside = true;
            }

            if (PositionX > relativeOrigin + LevelWidth/* + 50*/)
            {
                //Game.Window.Title = "OUTSIDE!!";
                //HP = 0;
                IsOutside = true;
            }
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
            int nbrOfShots = 6;
            Vector2 initDir = GlobalFunctions.ScaleDirection(ShootObject.Position - Position);  
            for (int n = 0; n < nbrOfShots; n++)
            {
                EnemyWeakRedLaser bullet = new EnemyWeakRedLaser(Game, spriteSheet);
                bullet.PositionX = PositionX;
                bullet.PositionY = PositionY;
            
                bullet.Direction = GlobalFunctions.SpreadDir(initDir, Math.PI/12);
                bullet.Initialize();
                bullet.Duration = 600;
            
                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
        }

    }
}
