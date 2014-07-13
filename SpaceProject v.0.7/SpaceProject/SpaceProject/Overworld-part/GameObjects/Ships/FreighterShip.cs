using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FreighterShip : OverworldShip
    {
        private Rectangle view;
        private int viewRadius;
        public GameObjectOverworld destinationPlanet;
        public Vector2 destination;
        public Vector2 tempDestination;
        private Sector sector = null;
        private bool hasArrived;
        public bool HasArrived { get { return hasArrived; } private set { ;} }

        public FreighterShip(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet) { }

        public override void Initialize()
        {
            Class = "FreighterShip";
            name = "Freighter Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(2, 201, 43, 68));
            viewRadius = 300;
            position = new Vector2(0,0);
            speed = 0.25f;

            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2, sprite.SourceRectangle.Value.Height / 2);
            color = Color.White;
            scale = 1.0f;
            layerDepth = 0.6f;

            base.Initialize();
        }

        public void Initialize(Sector sec)
        {
            Initialize();

            sector = sec;
            SetRandomStartPlanet();
            SetRandomEndPlanet();
        }

        public void Initialize(Sector sec, GameObjectOverworld startingPoint, GameObjectOverworld endDestination)
        {
            Initialize();

            sector = sec;
            position = startingPoint.position;
            destinationPlanet = endDestination;
            destination = destinationPlanet.position;
        }
        public void SetSector(Sector sec) { sector = sec; }
        public void SetEndPlanet(GameObjectOverworld des) 
        { 
            destination = des.position;
            destinationPlanet = des;
        }

        public void SetRandomStartPlanet()
        {
            List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
            tempList.AddRange(sector.GetGameObjects());

            Random r = new Random(DateTime.Now.Millisecond);
            position = tempList[(int)r.Next(0, tempList.Count - 1)].position;
        }

        public void SetRandomEndPlanet()
        {
            List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
            tempList.AddRange(sector.GetGameObjects());

            Random r = new Random(DateTime.Now.Millisecond);
            destinationPlanet = tempList[(int)r.Next(0, tempList.Count - 1)];
            destination = destinationPlanet.position;

            if (position == destination)
                SetRandomEndPlanet();
        }

        public override void FinalGoodbye()
        {
            IsDead = true;
            if (sector != null)
            {
                sector.shipSpawner.RemoveFreighterShip();
            }
        }

        public void Wait()
        {
            speed = 0;
            tempDestination = destination;
            destination = Vector2.Zero;
        }

        public void Start()
        {
            speed = 0.25f;
            destination = tempDestination;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameStateManager.currentState == "OverworldState")
                IsUsed = true;
            else
                IsUsed = false;
            
            // Update view
            view = new Rectangle((int)position.X - viewRadius, (int)position.Y - viewRadius, viewRadius * 2, viewRadius * 2); 

            // Adjust course towards target
            if (destination != Vector2.Zero)
            {
                Direction.RotateTowardsPoint(this.position, destination, 0.2f);
                AddParticle();
            }
            else
                Direction = Direction.Zero;

            angle = (float)(GlobalMathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI) / 2);

            // Check if arrived at destination
            if (CollisionDetection.IsRectInRect(this.Bounds, destinationPlanet.Bounds))
            {
                hasArrived = true;
                Game.stateManager.overworldState.RemoveOverworldObject(this);
            }

            if (IsUsed)
            {
                base.Update(gameTime);
            }

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsUsed)
                base.Draw(spriteBatch);
        }

    }
}
