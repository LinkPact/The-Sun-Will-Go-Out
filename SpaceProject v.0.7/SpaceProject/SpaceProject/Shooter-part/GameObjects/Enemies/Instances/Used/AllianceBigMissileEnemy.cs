using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class AllianceBigMissileEnemy : ShootingEnemyShip
    {
        public AllianceBigMissileEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        private void Setup()
        {
            fraction = Fraction.alliance;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryHigh;

            //ShootsOnce(1000);
            AddPrimaryModule(1000, ShootingMode.Single);

            //Egenskaper
            SightRange = 250;
            HP = 500;
            Damage = 120;
            Speed = 0.15f;
            
            //Animationer
            anim.LoopTime = 500;

            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(440, 0, 42, 42)));

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

        //Hanterar "shooting"
        protected override void ShootingPattern(GameTime gameTime)
        {    
            double width = Math.PI / 6;
            int numberOfShots = 5;

            for (double dir = -width / 2 + Math.PI / 2; dir <= width / 2 + Math.PI / 2; dir += (width / numberOfShots))
            {
                EnemyHomingMissileBullet bullet = new EnemyHomingMissileBullet(Game, spriteSheet, player);
                bullet.Position = Position;
                bullet.Direction = GlobalMathFunctions.DirFromRadians(dir);
                bullet.Initialize();
                
                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
