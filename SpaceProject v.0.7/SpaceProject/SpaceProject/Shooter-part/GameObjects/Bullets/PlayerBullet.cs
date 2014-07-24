using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PlayerBullet : Bullet
    {
        protected Random random;

        public PlayerBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            random = new Random();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool CheckOutside()
        {
            if (PositionX + anim.Width < 0 || PositionX - anim.Width > windowWidth
                || PositionY + anim.Width < 0 || PositionY - anim.Height > windowHeight)
            {
                return true;
            }
            else
                return false;
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            if (TempInvincibility > 0)
                return;

            OnKilled();
            IsKilled = true;
            base.InflictDamage(obj);
        }

    }
}
