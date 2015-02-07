using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class OverworldExplosion : GameObjectOverworld
    {
        private ExplosionParticleOverworld[] particleArray;

        public OverworldExplosion(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Update(GameTime gameTime)
        {
            foreach (ExplosionParticleOverworld par in particleArray)
                par.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (ExplosionParticleOverworld par in particleArray)
                par.Draw(spriteBatch);
        }

        public void setParticles(ExplosionParticleOverworld[] particleArray)
        {
            this.particleArray = particleArray;
        }

        public void GenerateExplosionParticles(Game1 game, Sprite spriteSheet, GameObjectVertical source, int nbrPartices, float size,
            float speedFactor = 1, float fragmentDurFactor = 1, float dirFactor = 1, bool randomDir = false)
        {
            Vector2 position = source.Position;
            Vector2 direction = source.Direction * dirFactor;
            float speed = source.Speed * speedFactor;

            ExplosionParticleOverworld[] tempParticleArray = new ExplosionParticleOverworld[nbrPartices];

            for (int i = 0; i < nbrPartices; i++)
                tempParticleArray[i] = new ExplosionParticleOverworld(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             randomDir, i);
            particleArray = tempParticleArray;
        }

        public void GenerateAbsoluteExplosion(Game1 game, Sprite spriteSheet, GameObjectOverworld source, int nbrPartices, float size,
            float speed = 1, int fragmentDur = 1, float fragmentSpeed = 1, bool randomDir = false)
        {
            Vector2 position = source.position;
            Vector2 direction = source.Direction.GetDirectionAsVector();

            ExplosionParticleOverworld[] tempParticleArray = new ExplosionParticleOverworld[nbrPartices];

            for (int i = 0; i < nbrPartices; i++)
                tempParticleArray[i] = new ExplosionParticleOverworld(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             randomDir, i, fragmentSpeed, fragmentDur);
            particleArray = tempParticleArray;
        }

        public Boolean ExplosionFinished()
        {
            foreach (ExplosionParticleOverworld par in particleArray)
            {
                if (par.LifeTime > 0)
                    return false;
            }

            return true;
        }
    }
}
