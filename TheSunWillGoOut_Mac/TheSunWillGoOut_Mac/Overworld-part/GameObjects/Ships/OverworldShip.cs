using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class OverworldShip : GameObjectOverworld
    {
        public static bool FollowPlayer = true;

        private ParticleManager particleManager;

        // A.I. varriables
        public ShipAction AIManager;

        private bool saveShip = true;
        public bool SaveShip { get { return saveShip; } set { saveShip = value; } }
        private bool removeOnStationEnter = true;
        public bool RemoveOnStationEnter { get { return removeOnStationEnter; } set { removeOnStationEnter = value; } }

        public Rectangle view;
        protected int viewRadius;
        protected Sector sector = null;
        public void SetSector(Sector sec) { sector = sec; }
        public Sector GetSector() { return sector; }

        public Vector2 destination;
        protected bool hasArrived;
        public bool HasArrived { get { return hasArrived; } set { hasArrived = value; } }
        public void ResetArrived() { hasArrived = false; }

        // Should be removed
        protected GameObjectOverworld target;
        public void SetTarget(GameObjectOverworld target) { this.target = target; }
        //

        // Collision behaviour 
        public CollisionEvent collisionEvent;

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

            particleManager = new ParticleManager(Game, this);
        }

        public override void Update(GameTime gameTime)
        {
            // Update view
            view = new Rectangle((int)position.X - viewRadius, (int)position.Y - viewRadius, viewRadius * 2, viewRadius * 2);

            if (AIManager != null)
                AIManager.Update(gameTime);

            // Adjust course towards target
            if (destination != Vector2.Zero)
            {
                Direction.RotateTowardsPoint(this.position, destination, 0.2f);
                particleManager.AddParticle();
            }
            else
                Direction = Direction.Zero;

            // Update the exhausts
            particleManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            particleManager.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

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
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

        public void Remove()
        {
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

        public void Explode()
        {
            Game.stateManager.overworldState.AddEffectsObject(ExplosionGenerator.GenerateOverworldExplosion(Game, Game.spriteSheetVerticalShooter, this));
            Game.stateManager.overworldState.AddEffectsObject(ExplosionGenerator.GenerateOverworldExplosion(Game, Game.spriteSheetVerticalShooter, this));
        }
    }
}
