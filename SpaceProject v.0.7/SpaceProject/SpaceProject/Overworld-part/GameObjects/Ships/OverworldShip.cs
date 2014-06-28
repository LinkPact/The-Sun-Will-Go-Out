using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class OverworldShip : GameObjectOverworld
    {

        #region ParticleVariables
        private List<Particle> particles = new List<Particle>();
        private List<Particle> deadParticles = new List<Particle>();

        #endregion

        public OverworldShip(Game1 game, Sprite SpriteSheet) :
            base(game, SpriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            layerDepth = 0.55f;
        }

        public override void FinalGoodbye()
        {
            base.FinalGoodbye();
        }

        public override void Update(GameTime gameTime)
        {
            #region UpdateParticles

            foreach (Particle par in particles)
            {
                par.Update(gameTime, this);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].lifeSpawn <= 0)
                {
                    deadParticles.Add(particles[i]);
                }
            }

            RemoveParticle();

            #endregion

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle par in particles)
                par.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        #region ParticleMethods

        protected void AddParticle()
        {
            Particle par = new Particle(Game, Game.spriteSheetOverworld);
            par.Initialize(this);
            particles.Add(par);
        }

        protected void RemoveParticle()
        {
            for (int i = 0; i < deadParticles.Count; i++)
            {
                particles.Remove(deadParticles[i]);
            }

            deadParticles.Clear();
        }

        #endregion
    }
}
