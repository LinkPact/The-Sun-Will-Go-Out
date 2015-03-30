using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class BigYellowEnemy : ShootingEnemyShip
    {
        private Animation shooting;

        public BigYellowEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
            ObjectName = "BigYellowEnemy";
        }

        public override void Initialize()
        {
            base.Initialize();

            AddPrimaryModule(70, ShootingMode.Batches);
            primaryModule.ShootsInBatchesSetup(5, 2000);
            primaryModule.SetRandomCharge(random);

            //Egenskaper
            SightRange = 100000;
            HP = 400;
            Damage = 10;
            Speed = 0.04f;
            
            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(210, 130, 40, 40)));

            shooting = new Animation();
            shooting.LoopTime = 1000;
            
            shooting.AddFrame(spriteSheet.GetSubSprite(new Rectangle(210, 130, 40, 40)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //UpdateAnimation(gameTime);
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    if (!IsKilled)
        //    {
        //        if (lastTimeShot >= ShootingDelay - 1000 && ShootObject != null)
        //            spriteBatch.Draw(shooting.CurrentFrame.Texture, Position, shooting.CurrentFrame.SourceRectangle, Color.White, 0.0f, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        //        else
        //        {
        //            base.Draw(spriteBatch);
        //        }
        //    }
        //}
        //
        //private void UpdateAnimation(GameTime gameTime)
        //{
        //    if (lastTimeShot >= ShootingDelay - 1000)
        //        shooting.Update(gameTime);
        //}

        protected override void ShootingPattern(GameTime gameTime)
        {
            EnemyGreenBullet bullet = new EnemyGreenBullet(Game, spriteSheet);
            bullet.PositionX = PositionX;
            bullet.PositionY = PositionY;
            bullet.Direction = MathFunctions.ScaleDirection(ShootObject.Position - Position);
            bullet.Initialize();
            bullet.Duration = 1000;

            Game.stateManager.shooterState.gameObjects.Add(bullet);
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
