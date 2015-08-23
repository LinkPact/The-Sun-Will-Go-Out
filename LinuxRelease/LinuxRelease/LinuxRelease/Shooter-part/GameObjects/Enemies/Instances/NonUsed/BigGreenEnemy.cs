using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class BigGreenEnemy : EnemyShip
    {
        public BigGreenEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            ObjectName = "BigGreenEnemy";
        }

        public BigGreenEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            this.movement = movement;
            ObjectName = "BigGreenEnemy";
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            SightRange = 400;
            HP = 100.0f;
            Damage = 20;
            Speed = 0.15f;
            
            //Animationer
            anim.LoopTime = 500;
            //anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(0, 34, 18, 18)));
            //anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(18, 34, 18, 18)));

            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(90, 130, 40, 40)));

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
    }
}
