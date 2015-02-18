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

            AddPrimaryModule(2500, ShootingMode.Regular);
            primaryModule.SetFullCharge();

            movement = Movement.SmallZigzag;
            PrimaryShootSoundID = SoundEffects.BigLaser;

            //Egenskaper
            SightRange = 300;
            HP = 275;
            Damage = 60;
            Speed = 0.07f;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(544, 80, 29, 45)));

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

            Vector2 initDir = MathFunctions.ScaleDirection(ShootObject.Position - Position);  
            for (int n = 0; n < nbrOfShots; n++)
            {
                EnemyWeakRedLaser bullet = new EnemyWeakRedLaser(Game, spriteSheet);
                bullet.PositionX = PositionX;
                bullet.PositionY = PositionY;
            
                bullet.Direction = MathFunctions.SpreadDir(initDir, spread);
                bullet.Initialize();
                bullet.Duration *= 0.8f;
                bullet.Speed *= 0.8f;

                bullet.SetSpreadSpeed(random);

                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
