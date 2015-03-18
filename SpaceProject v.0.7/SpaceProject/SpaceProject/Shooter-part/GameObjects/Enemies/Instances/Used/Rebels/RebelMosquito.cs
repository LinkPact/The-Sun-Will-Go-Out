using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class RebelMosquito : EnemyShip
    {
        public RebelMosquito(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
            Setup();
        }

        public RebelMosquito(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player,
            Movement movement) :
            base(Game, spriteSheet, player)
        {
            Setup();
            this.movement = movement;
        }

        private void Setup()
        {
            fraction = Fraction.pirate;
        }

        public override void Initialize()
        {
            base.Initialize();

            lootValue = LootValue.veryLow;

            //Egenskaper
            SightRange = 400;
            HP = 100.0f;
            Damage = (float)CollisionDamage.veryLow;
            Speed = 0.3f;
            TurningSpeed = 4;

            movement = Movement.Following;

            //Animationer
            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(182, 29, 27, 32)));

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
