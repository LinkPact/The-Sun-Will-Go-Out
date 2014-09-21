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

        // A.I. varriables
        protected ShipAction AIManager;
        public static bool FollowPlayer = true;

        public Rectangle view;
        protected int viewRadius;
        protected Sector sector = null;
        public void SetSector(Sector sec) { sector = sec; }

        protected bool hasArrived;
        public bool HasArrived { get { return hasArrived; } private set { ; } }
        public void ResetArrived() { hasArrived = false; } 

        // Should be removed
        protected GameObjectOverworld target;
        public void SetTarget(GameObjectOverworld target) { this.target = target; }
        //
        public Vector2 destination;

        // Used to determine which level starts when player runs into this ship.
        private string level;
        public string Level { get { return level; } set { level = value; } }

        // Message shown on encounter
        private string encounterMessage;
        public string EncounterMessage { get { return encounterMessage; } set { encounterMessage = value; } }

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

        public override void Update(GameTime gameTime)
        {
            if (AIManager != null)
                AIManager.Update(gameTime);

            UpdateParticles(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle par in particles)
                par.Draw(spriteBatch); 

            base.Draw(spriteBatch);
        }
                
        #region ParticleMethods

        private void UpdateParticles(GameTime gameTime)
        {
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
        }

        public void AddParticle()
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

        public override void FinalGoodbye()
        {
            base.FinalGoodbye();
        }

        public void Destroy()
        {
            //TODO: Add explosion
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

        public void Remove()
        {
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

    }
}
