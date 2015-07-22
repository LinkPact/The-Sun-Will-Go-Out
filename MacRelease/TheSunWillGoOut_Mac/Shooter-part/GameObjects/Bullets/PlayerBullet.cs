using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public enum Tracker
    {
        None,
        Player,
        Ally
    }

    public class PlayerBullet : Bullet
    {
        protected Random random;
        protected Tracker tracker;
        public Tracker Tracker { get { return tracker; } set { tracker = value; } }

        public PlayerBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            random = new Random();
        }

        public override void Initialize()
        {
            base.Initialize();
            tracker = Tracker.Player;
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

            if (HP <= 1)
            {
                OnKilled();
                IsKilled = true;
            }

            base.InflictDamage(obj);
        }

    }
}
