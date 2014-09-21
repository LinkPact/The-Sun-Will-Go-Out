using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public enum ShipType
    {
        Alliance,
        Rebel
    }

    public class AllyShip : OverworldShip
    {
        public GameObjectOverworld destinationPlanet;
        public Vector2 tempDestination;
        private ShipType type;

        public AllyShip(Game1 game, Sprite spriteSheet, ShipType type) :
            base(game, spriteSheet) 
        {
            this.type = type;
        }

        public override void Initialize()
        {
            Class = "AllyShip";
            name = "Ally Ship";

            if (type == ShipType.Alliance)
            {
                sprite = spriteSheet.GetSubSprite(new Rectangle(48, 200, 23, 28));
            }
            else if (type == ShipType.Rebel)
            {
                sprite = spriteSheet.GetSubSprite(new Rectangle(484, 0, 23, 34));
            }
            viewRadius = 300;
            position = new Vector2(0,0);
            speed = 0.4f;

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

        public void Initialize(Sector sec, Vector2 startingPoint, Vector2 endPoint)
        {
            Initialize();

            sector = sec;
            position = startingPoint;
            destination = endPoint;
        }

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

        public override void Wait()
        {
            speed = 0;
            tempDestination = destination;
            destination = Vector2.Zero;
        }

        public override void Start()
        {
            speed = 0.5f;
            destination = tempDestination;
        }

        public override void Update(GameTime gameTime)
        {
            if (target != null)
            {
                destination = target.position;
            }

            // Check if arrived at destination
            if (destinationPlanet != null)
            {
                if (CollisionDetection.IsRectInRect(this.Bounds, destinationPlanet.Bounds))
                {
                    hasArrived = true;
                    Game.stateManager.overworldState.RemoveOverworldObject(this);
                }
            }
            else
            {
                if (CollisionDetection.IsPointInsideCircle(position, destination, 200))
                {
                    this.Wait();
                    hasArrived = true;
                }
            }

            // Rotate ship
            angle = (float)(MathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI) / 2 + Math.PI);

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
