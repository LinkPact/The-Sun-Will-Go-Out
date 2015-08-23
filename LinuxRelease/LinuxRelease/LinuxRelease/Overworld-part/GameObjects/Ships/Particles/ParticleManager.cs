using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class ParticleManager
    {
        private Game1 game;
        private GameObjectOverworld ship;

        private List<Particle> particles;
        private List<Particle> deadParticles;

        public ParticleManager(Game1 game, GameObjectOverworld ship)
        {
            this.game = game;
            this.ship = ship;

            particles = new List<Particle>();
            deadParticles = new List<Particle>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Particle par in particles)
            {
                par.Update(gameTime, ship);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].lifeSpawn <= 0)
                {
                    deadParticles.Add(particles[i]);
                }
            }

            RemoveParticle();
        }

        public void AddParticle()
        {
            Particle par = new Particle(game, game.spriteSheetOverworld);
            par.Initialize(ship, ship.position);
            particles.Add(par);
        }

        private void RemoveParticle()
        {
            for (int i = 0; i < deadParticles.Count; i++)
            {
                particles.Remove(deadParticles[i]);
            }

            deadParticles.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle par in particles)
                par.Draw(spriteBatch);
        }
    }
}
