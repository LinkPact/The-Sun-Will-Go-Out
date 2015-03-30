using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class BigRedEnemy : ShootingEnemyShip
    {
        public BigRedEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            AddPrimaryModule(300, ShootingMode.Single);

            //Egenskaper
            SightRange = 250;
            HP = 250;
            Damage = 10;
            Speed = 0.3f;
            
            ObjectClass = "enemy";
            ObjectName = "BigRedEnemy";

            //Animationer
            anim.LoopTime = 500;
            
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(132, 130, 38, 40)));

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
                bullet.Direction = MathFunctions.DirFromRadians(dir);
                bullet.Initialize();
                
                Game.stateManager.shooterState.gameObjects.Add(bullet);
            }
        }

        protected override void SecondaryShootingPattern(GameTime gameTime)
        { }
    }
}
