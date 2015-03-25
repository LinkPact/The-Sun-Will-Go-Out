using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Linux
{
    public abstract class EnemyBullet : Bullet
    {

        protected EnemyBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

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
