using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FreighterShip : OverworldShip
    {
        public GameObjectOverworld destinationPlanet;
        public Vector2 tempDestination;

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
            SetDefaultBehaviour();
        }

        public void Initialize(Sector sec, GameObjectOverworld startingPoint, GameObjectOverworld endDestination)
        {
            Initialize();

            sector = sec;
            position = startingPoint.position;
            destinationPlanet = endDestination;
            destination = destinationPlanet.position;
        }
        public void SetDefaultBehaviour()
        {
            LoopAction foo = new LoopAction();
            foo.Add(new TravelAction(this, TravelAction.GetRandomPlanet(sector)));
            foo.Add(new WaitTimeAction(this, 7500));
            AIManager = foo;
            overworldEvent = new DisplayFreighterMessageOE(Game);
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

            position = tempList[(int)Game.random.Next(0, tempList.Count - 1)].position;
        }

        public void SetRandomEndPlanet()
        {
            List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
            tempList.AddRange(sector.GetGameObjects());

            destinationPlanet = tempList[(int)Game.random.Next(0, tempList.Count - 1)];
            destination = destinationPlanet.position;

            overworldEvent = new DisplayFreighterMessageOE(Game);

            if (position == destination)
                SetRandomEndPlanet();
        }

        public override void FinalGoodbye()
        {
            IsDead = true;
        }

        public override void Wait()
        {
            speed = 0;
            tempDestination = destination;
            destination = Vector2.Zero;
        }

        public override void Start()
        {
            speed = 0.25f;
            destination = tempDestination;
        }

        public override void Update(GameTime gameTime)
        {
            angle = (float)(MathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + +(Math.PI / 180) * 270);

           base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsUsed)
                base.Draw(spriteBatch);
        }

    }
}
