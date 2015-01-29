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
        { }

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

        public void GenerateExplosionParticles(Game1 game, Sprite spriteSheet, GameObjectVertical source, int nbrPartices, float size,
            float speedFactor = 1, float fragmentDurFactor = 1, float dirFactor = 1, bool randomDir = false)
        {
            Vector2 position = source.Position;
            Vector2 direction = source.Direction * dirFactor;
            float speed = source.Speed * speedFactor;

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[nbrPartices];

            for (int i = 0; i < nbrPartices; i++)
                tempParticleArray[i] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             randomDir, i);
            particleArray = tempParticleArray;
        }

        public void GenerateAbsoluteExplosion(Game1 game, Sprite spriteSheet, GameObjectVertical source, int nbrPartices, float size,
            float speed = 1, int fragmentDur = 1, float fragmentSpeed = 1, bool randomDir = false)
        {
            Vector2 position = source.Position;
            Vector2 direction = source.Direction;

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[nbrPartices];

            for (int i = 0; i < nbrPartices; i++)
                tempParticleArray[i] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             randomDir, i, fragmentSpeed, fragmentDur);
            particleArray = tempParticleArray;
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
