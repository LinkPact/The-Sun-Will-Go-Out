using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelSmallEnemy : ShootingEnemyShip
    {
        public RebelSmallEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelSmallEnemy(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player, Movement movement) :
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

            lootValue = LootValue.low;

            //Shooting
            shootingDelay = 3000;
            lastTimeShot = shootingDelay * random.NextDouble();

            //Egenskaper
            SightRange = 400;
            HP = 400.0f;
            Damage = 60;
            Speed = 0.05f;

            movement = Movement.Zigzag;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(384, 80, 41, 34)));

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

        }
    }
}
