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
        private ParticleManager particleManager;

        // A.I. varriables
        protected ShipAction AIManager;
        public static bool FollowPlayer = true;

        public Rectangle view;
        protected int viewRadius;
        protected Sector sector = null;
        public void SetSector(Sector sec) { sector = sec; }
        public Sector GetSector() { return sector; }

        public Vector2 destination;
        protected bool hasArrived;
        public bool HasArrived { get { return hasArrived; } private set { ; } }
        public void ResetArrived() { hasArrived = false; } 

        // Should be removed
        protected GameObjectOverworld target;
        public void SetTarget(GameObjectOverworld target) { this.target = target; }
        //

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
            if (GameStateManager.currentState == "OverworldState")
                IsUsed = true;
            else
                IsUsed = false;

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
            //TODO: Add explosion
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

        public void Remove()
        {
            Game.stateManager.overworldState.RemoveOverworldObject(this);
        }

    }
}
