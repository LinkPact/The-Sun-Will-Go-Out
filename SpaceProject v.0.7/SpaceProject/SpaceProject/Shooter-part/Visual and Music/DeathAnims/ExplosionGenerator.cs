using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ExplosionGenerator
    {
        private static Random random = new Random();

        public static Explosion GenerateFixedExplosion(Game1 game, Sprite spriteSheet,
                                                  GameObjectVertical source)
        {
            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int nbrPartices = 8;
            float size = source.BoundingWidth / 2;
            tempExplosion.GenerateExplosionParticles(game, spriteSheet, source, nbrPartices, size);

            return tempExplosion;
        }

        public static Explosion GenerateRandomExplosion(Game1 game, Sprite spriteSheet,
                                                        GameObjectVertical source)
        {
            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int nbrParticles = random.Next(8, 16);
            float size = source.BoundingWidth + source.BoundingHeight;

            tempExplosion.GenerateExplosionParticles(game, spriteSheet, source, nbrParticles, size, randomDir: true);

            return tempExplosion;
        }

        public static Explosion GenerateShipExplosion(Game1 game, Sprite spriteSheet, GameObjectVertical source)
        {
            float size = 20;
            float fragmentSpeed = 0.2f;
            int lifeTime = 15;
            int nbrParticlesBase = 25;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int nbrParticles = random.Next(nbrParticlesBase, nbrParticlesBase + (int)(nbrParticlesBase * 0.5));

            tempExplosion.GenerateAbsoluteExplosion(game, spriteSheet, source, nbrParticles, size,
                randomDir: true, speed: source.Speed, fragmentDur: lifeTime, fragmentSpeed: fragmentSpeed);

            return tempExplosion;
        }

        public static Explosion GenerateBulletExplosion(Game1 game, Sprite spriteSheet, GameObjectVertical source)
        {
            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int nbrParticles = 1;
            float size = (source.BoundingWidth + source.BoundingHeight) * 3;
            tempExplosion.GenerateExplosionParticles(game, spriteSheet, source, nbrParticles, size, 
                randomDir: true, speedFactor: 0.1f, dirFactor: -1);
            return tempExplosion;
        }

        public static Explosion GenerateBombExplosion(Game1 game, Sprite spriteSheet, GameObjectVertical source)
        {
            float size = 20;
            float fragmentSpeed = 0.4f;
            int lifeTime = 15;
            int nbrParticlesBase = 50;

            Explosion tempExplosion = new Explosion(game, spriteSheet);

            int nbrParticles = random.Next(nbrParticlesBase, nbrParticlesBase + (int)(nbrParticlesBase * 0.5));

            tempExplosion.GenerateAbsoluteExplosion(game, spriteSheet, source, nbrParticles, size,
                randomDir: true, speed: source.Speed, fragmentDur: lifeTime, fragmentSpeed: fragmentSpeed);
            
            game.soundEffectsManager.PlaySoundEffect(source.getDeathSoundID(), source.SoundPan);
            return tempExplosion;
        }

        public static OverworldExplosion GenerateOverworldExplosion(Game1 game, Sprite spriteSheet, GameObjectOverworld source)
        {
            float size = 20;
            float fragmentSpeed = 0.4f;
            int lifeTime = 15;
            int nbrParticlesBase = 50;

            OverworldExplosion tempExplosion = new OverworldExplosion(game, spriteSheet);

            int nbrParticles = random.Next(nbrParticlesBase, nbrParticlesBase + (int)(nbrParticlesBase * 0.5));

            tempExplosion.GenerateAbsoluteExplosion(game, spriteSheet, source, nbrParticles, size,
                randomDir: true, speed: 0, fragmentDur: lifeTime, fragmentSpeed: fragmentSpeed);

            return tempExplosion;
        }

        public static OverworldExplosion GenerateSpaceDuckExplosion(Game1 game, Sprite spriteSheet, GameObjectOverworld source)
        {
            float size = 30;
            float fragmentSpeedBase = 0.8f;
            int lifeTime = (int)(30 * (float)(random.NextDouble()));
            int nbrParticles = 300;

            OverworldExplosion tempExplosion = new OverworldExplosion(game, spriteSheet);
            float fragmentSpeed = fragmentSpeedBase + (float)(random.NextDouble()) * fragmentSpeedBase;

            tempExplosion.GenerateAbsoluteExplosion(game, spriteSheet, source, nbrParticles, size,
            randomDir: true, speed: fragmentSpeed, fragmentDur: lifeTime, fragmentSpeed: fragmentSpeed);

            game.soundEffectsManager.PlaySoundEffect(source.getDeathSoundID(), source.SoundPan);

            return tempExplosion;
        }
    }
}
