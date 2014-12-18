using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public abstract class BackgroundObject : AnimatedGameObject
    {
        public BackgroundObject(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Update(GameTime gameTime)
        {
            //if (anim.HasEnded) 
            IsKilled = true;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnKilled()
        { }
    }
}