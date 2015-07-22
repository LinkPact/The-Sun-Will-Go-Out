using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SmallExplosion : BackgroundObject
    {
        //Game1 Game;
        //Sprite spriteSheet;
        //
        //Vector2 Position;
        //
        //protected Animation anim;
        //private Animation currentAnim;
        //
        //public Animation Anim { get { return anim; } }
        //public Animation CurrentAnim { get { return currentAnim; }
        //    set
        //    { 
        //        //Cen
        //    }
        //}

        public SmallExplosion(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            //Position = pos;

            //this.spriteSheet = spriteSheet;
            //this.Game = Game;
        }

        public override void Initialize()
        {
            base.Initialize();

            anim.LoopTime = 500;
            anim.PlaysOnce = true;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(258, 135, 32, 25)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(308, 135, 32, 25)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(358, 135, 32, 25)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(408, 135, 32, 25)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(458, 135, 32, 25)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        //public override bool CheckOutside()
        //{
        //    return false;
        //}
    }
}
