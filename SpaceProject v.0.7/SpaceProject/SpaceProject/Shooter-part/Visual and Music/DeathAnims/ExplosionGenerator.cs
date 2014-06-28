using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ExplosionGenerator
    {
        private static Random random = new Random();

        public static Explosion GenerateFixedExplosion(Game1 game, Sprite spriteSheet,
                                                  GameObjectVertical source)
        {
            float size = source.BoundingWidth / 2;
            Vector2 position = source.Position;
            Vector2 direction = source.Direction;
            float speed = source.Speed;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[8];

            for (int i = 0; i < 8; i++)
                tempParticleArray[i] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             false, i);

            tempExplosion.setParticles(tempParticleArray);

            return tempExplosion;
        }

        public static Explosion GenerateRandomExplosion(Game1 game, Sprite spriteSheet,
                                                        GameObjectVertical source)
        {
            float size = source.BoundingWidth + source.BoundingHeight;
            Vector2 position = source.Position;
            Vector2 direction = source.Direction;
            float speed = source.Speed;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int numberOfParticles = random.Next(8, 16);

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[numberOfParticles];

            for (int i = 0; i < numberOfParticles; i++)
                tempParticleArray[i] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             true, i);

            tempExplosion.setParticles(tempParticleArray);

            game.soundEffectsManager.PlaySoundEffect(source.getDeathSoundID(), source.SoundPan);
            return tempExplosion;
        }

        public static Explosion GenerateBulletExplosion(Game1 game, Sprite spriteSheet,
                                                GameObjectVertical source)
        {
            float size = (source.BoundingWidth + source.BoundingHeight) * 3;
            Vector2 position = source.Position;
            Vector2 direction = source.Direction *-1;
            float speed = source.Speed / 10;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int numberOfParticles = 1;

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[numberOfParticles];

            tempParticleArray[0] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             true, 0);

            tempExplosion.setParticles(tempParticleArray);

            game.soundEffectsManager.PlaySoundEffect(source.getDeathSoundID(), source.SoundPan);
            return tempExplosion;
        }

        public static Explosion GenerateBombExplosion(Game1 game, Sprite spriteSheet,
                                                GameObjectVertical source)
        {
            // EXPERIMENTAL VARIABLES
            float TESTSIZE = 20;
            float TESTFRAGMENTSPEED = 0.4f;
            int TESTLIFETIME = 15;
            int TESTNBRPARTICLESBASE = 50;

            float size = TESTSIZE;
            Vector2 position = source.Position;
            Vector2 direction = source.Direction;
            float speed = source.Speed;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int numberOfParticles = random.Next(TESTNBRPARTICLESBASE, TESTNBRPARTICLESBASE + (int)(TESTNBRPARTICLESBASE * 0.5));

            ExplosionParticle[] tempParticleArray = new ExplosionParticle[numberOfParticles];

            for (int i = 0; i < numberOfParticles; i++)
                tempParticleArray[i] = new ExplosionParticle(game, spriteSheet, position,
                                                             direction, speed, size,
                                                             true, i, TESTFRAGMENTSPEED, TESTLIFETIME);

            tempExplosion.setParticles(tempParticleArray);

            game.soundEffectsManager.PlaySoundEffect(source.getDeathSoundID(), source.SoundPan);
            return tempExplosion;
        }
    }
}
