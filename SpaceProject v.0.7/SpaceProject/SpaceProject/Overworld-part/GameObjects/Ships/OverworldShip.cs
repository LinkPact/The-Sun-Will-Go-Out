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
        private List<Particle> particles;
        private List<Particle> deadParticles;

        #endregion

        protected bool hasArrived;
        public bool HasArrived { get { return hasArrived; } private set { ; } }
        public void ResetArrived() { hasArrived = false; } 

        protected GameObjectOverworld target;

        // Used to determine which level starts when player runs into this ship.
        private string level;
        public string Level { get { return level; } set { level = value; } }

        public OverworldShip(Game1 game, Sprite SpriteSheet) :
            base(game, SpriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            layerDepth = 0.55f;

            particles = new List<Particle>();
            deadParticles = new List<Particle>();
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

        public void SetPosition(Vector2 pos) { position = pos; }
        public void SetTarget(GameObjectOverworld target) { this.target = target; }

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

        public virtual void Wait()
        {
        }

        public virtual void Start()
        {
        }

    }
}
