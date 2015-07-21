using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class Supplies : VerticalShooterShip
    {
        private Rectangle healthRect = new Rectangle(544, 0, 13, 13);
        private Rectangle fusionCellRect = new Rectangle(558, 0, 13, 13);
        private Rectangle ammoRect = new Rectangle(572, 0, 13, 13);

        public Supplies(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player) :
            base(Game, spriteSheet, player)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            ObjectClass = "supplies";

            PositionY = -100;
            PositionX = random.Next(0, 800);

            HPmax = 1;
            HP = 1;

            anim.LoopTime = 500;
            switch (random.Next(3))
            {
                case 0:
                    anim.AddFrame(spriteSheet.GetSubSprite(healthRect));
                    break;

                case 1:
                    anim.AddFrame(spriteSheet.GetSubSprite(fusionCellRect));
                    break;

                case 2:
                    anim.AddFrame(spriteSheet.GetSubSprite(ammoRect));
                    break;

                default:
                    anim.AddFrame(spriteSheet.GetSubSprite(healthRect));
                    break;
            }

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
            useDeathAnim = false;
        }

        public override void Update(GameTime gameTime)
        {
            PositionY += 3;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            return;
        }
    }
}
