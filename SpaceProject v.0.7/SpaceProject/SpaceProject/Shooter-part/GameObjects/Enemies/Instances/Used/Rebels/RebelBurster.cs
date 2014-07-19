using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelBurster : ShootingEnemyShip
    {
        public RebelBurster(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        { }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.medium;

            SetShootingDelay(3000);
            ChargeWeapon(ChargeMode.randomCharge);

            //Egenskaper
            SightRange = 300;
            HP = 175; //80;
            Damage = 60;
            Speed = 0.07f;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(340, 220, 30, 30)));

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PositionX <= relativeOrigin/* -50*/)
            {
                IsOutside = true;
            }

            if (PositionX > relativeOrigin + LevelWidth/* + 50*/)
            {
                IsOutside = true;
            }
        }

        protected override void ShootingPattern(GameTime gameTime)
        {
            int nbrOfShots = 12;
            double spread = Math.PI / 8;

            Vector2 initDir = GlobalMathFunctions.ScaleDirection(ShootObject.Position - Position);  
            for (int n = 0; n < nbrOfShots; n++)
            {
                EnemyWeakRedLaser bullet = new EnemyWeakRedLaser(Game, spriteSheet);
                bullet.PositionX = PositionX;
                bullet.PositionY = PositionY;
            
                bullet.Direction = GlobalMathFunctions.SpreadDir(initDir, spread);
                bullet.Initialize();
                bullet.Duration = 300;

                bullet.SetSpreadSpeed(random);

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
        }

    }
}
