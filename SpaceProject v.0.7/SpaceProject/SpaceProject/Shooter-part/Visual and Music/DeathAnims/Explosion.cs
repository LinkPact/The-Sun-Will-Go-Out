using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Explosion : BackgroundObject
    {
        private ExplosionParticle[] particleArray;

        public Explosion(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {

        }

        public override void Update(GameTime gameTime)
        {
            foreach (ExplosionParticle par in particleArray)
                par.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ExplosionParticle par in particleArray)
                par.Draw(spriteBatch);
        }

        public void setParticles(ExplosionParticle[] particleArray)
        {
            this.particleArray = particleArray;
        }

        public Boolean ExplosionFinished()
        {
            foreach (ExplosionParticle par in particleArray)
            {
                if (par.LifeTime > 0)
                    return false;
            }

            return true;
        }

    }

}
